using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Pmp.Camera.Lib.InternalApiModels;

namespace Pmp.Camera.Lib
{

    public class OiCamera : IDisposable, INotifyPropertyChanged
    {
        public IPAddress Address { get; }
        public HttpClient Client { get; }
        public string DcimDir { get; }
        public CancellationToken CancellationToken { get; }
        public PictureFrame CurrentFrame { get; private set; }
        public bool IsConnected { get; set; }
        public IPEndPoint IPEndPoint;
        private OiUdpStreamBlock udpStreamBlock;
        private OiUdpStreamToJpegBlock streamToJpegBlock;
        private CancellationTokenSource CancellationTokenSource;
        private Task streamContinuationTask;

        public event PropertyChangedEventHandler PropertyChanged;

        string FormatRequest(string r) => $"{r}.cgi";
        string WithDirDirectory(string r) => $"{r}?DIR={this.DcimDir}";
        string WithDir(string r, string dir) => $"{r}?DIR={this.DcimDir}/{dir}";
        string WithMode(string r, Mode mode) => $"{r}?mode={Enum.GetName(typeof(Mode), mode)}";
        string WithShutter(string r, string shutter) => $"{r}?com={shutter}";
        string WithPhoto(string r, string dir) => $"{r}/{dir}";

        public OiCamera(IPAddress address = default, string dcimDir = "/DCIM/100OLYMP")
        {
            this.CancellationTokenSource = new CancellationTokenSource();
            if (address == default)
            {
                this.Address = new IPAddress(new byte[] { 192, 168, 0, 10 });
            }
            else
            {
                this.Address = address;
            }
            var e = new IPEndPoint(this.Address, 54578);
            this.IPEndPoint = e;
            this.Client = new HttpClient();
            this.Client.BaseAddress = new Uri("http://" + this.Address.ToString());
            this.CancellationToken = this.CancellationTokenSource.Token;
            this.Client.Timeout = TimeSpan.FromSeconds(5);
            this.DcimDir = dcimDir;
        }

        public async Task TryStart()
        {
            try
            {
                await this.GetName();
                var connectionTask = Task.Run(async () =>
                {
                    while (!this.CancellationToken.IsCancellationRequested || !this.IsConnected)
                    {
                        await Task.Delay(500);
                        await this.GetName();
                    }
                }, this.CancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Trace.Fail(ex.ToString(), ex.Message.ToString());
            }
        }

        public async Task<(byte[], string)> GetThumbnail(string photo)
        {
            var result = await this.Client.GetAsync(this.WithDir(this.FormatRequest("get_thumbnail"), photo));
            if (result.IsSuccessStatusCode)
            {
                return (await result.Content.ReadAsByteArrayAsync(), result.Content.Headers.ContentType.Parameters.FirstOrDefault()?.Value);
            }
            return default;
        }

        public async Task<string> GetConnectMode()
        {
            var result = await this.Client.GetAsync(this.FormatRequest("get_connectmode"));
            if (result.IsSuccessStatusCode)
            {
                var rSTring = await result.Content.ReadAsStringAsync();
                return rSTring;
            }
            return default;
        }

        public async Task<String[]> ListFiles()
        {
            var mode = await this.SetMode(Mode.play);
            if (mode)
            {
                var result = await this.Client.GetAsync(this.WithDirDirectory(this.FormatRequest("get_imglist")));
                if (result.IsSuccessStatusCode)
                {
                    var rSTring = await result.Content.ReadAsStringAsync();
                    var lines = rSTring.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                    var content = lines.Skip(1);
                    var files = content.Select(x => x.Split(',')[1]).ToArray();
                    return files;
                }
            }
            return default;
        }

        public async Task<(byte[], string)> GetPic(string photo)
        {
            var result = await this.Client.GetAsync(this.WithDir(this.FormatRequest("get_thumbnail"), photo));
            if (result.IsSuccessStatusCode)
            {
                return (await result.Content.ReadAsByteArrayAsync(), result.Content.Headers.ContentType.Parameters.FirstOrDefault()?.Value);
            }
            return default;
        }

        public async Task<(byte[], string)> GetPhoto(string photo)
        {
            var result = await this.Client.GetAsync(string.Format("{0}/{1}", this.DcimDir, photo));
            if (result.IsSuccessStatusCode)
            {
                return (await result.Content.ReadAsByteArrayAsync(), result.Content.Headers.ContentType.Parameters.FirstOrDefault()?.Value);
            }
            return default;
        }

        public async Task<string> GetName()
        {
            const string name = "get_caminfo";
            try
            {
                using (var result = await this.Client.GetStreamAsync(this.FormatRequest(name)))
                {
                    var reader = new XmlSerializer(typeof(caminfo));
                    var type = (caminfo)reader.Deserialize(result);
                    this.IsConnected = true;
                    PropertyHelper.RaisePropertyChanged(this, () => this.IsConnected, this.PropertyChanged);
                    return type.model;
                }
            }
            catch (Exception ex)
            {
                this.IsConnected = false;
                PropertyHelper.RaisePropertyChanged(this, () => this.IsConnected, this.PropertyChanged);
                Trace.TraceError(ex.ToString(), ex);
                return null;
            }
        }



        public void Dispose()
        {
            if (this.streamToJpegBlock != null)
            {
                this.streamToJpegBlock.Dispose();
            }
            if (this.udpStreamBlock != null)
            {
                this.udpStreamBlock.Dispose();
            }
            this.CancellationTokenSource.Cancel();
        }

        public async Task<bool> Shutter(Shutter s)
        {
            var shutter = Enum.GetName(typeof(Shutter), s).Substring(1);
            var result = await this.Client.GetAsync(this.WithShutter(this.FormatRequest("exec_shutter"), shutter));
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            return default;
        }

        public async Task<bool> SetMode(Mode mode)
        {
            var r = this.WithMode(this.FormatRequest("switch_cammode"), mode);
            var result = await this.Client.GetAsync(r);

            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return default;
        }

        private async Task TryStartCamera()
        {
            if (this.udpStreamBlock == null)
            {
                this.udpStreamBlock = new OiUdpStreamBlock(this);
                this.streamToJpegBlock = new OiUdpStreamToJpegBlock();
                await this.streamToJpegBlock.TryStart();
                await this.udpStreamBlock.TryStart();
                var continuation = Task.Run(async () =>
                {
                    while (await this.udpStreamBlock.Reader.WaitToReadAsync() && !this.CancellationToken.IsCancellationRequested)
                    {
                        if (this.udpStreamBlock.Reader.TryRead(out var frame))
                        {
                            await this.streamToJpegBlock.Continue(frame);
                        }
                    }
                    this.streamToJpegBlock.Dispose();
                });

                var jpegContinuation = Task.Run(async () =>
                {
                    while (await this.streamToJpegBlock.Reader.WaitToReadAsync() && !this.CancellationToken.IsCancellationRequested)
                    {
                        if (this.streamToJpegBlock.Reader.TryRead(out var frame))
                        {
                            this.CurrentFrame = frame;
                        }
                    }
                });
                this.streamContinuationTask = continuation;
            }
        }

        //    < param2 name="0320x0240" />
        //    < param2 name="0640x0480" />
        //    < param2 name="0800x0600" />
        //    < param2 name="1024x0768" />
        //    < param2 name="1280x0960" />

        public async Task<string> Exec(string command)
        {
            var r1 = await this.Client.GetAsync(command);
            if (r1.IsSuccessStatusCode)
            {
                return await r1.Content.ReadAsStringAsync();
            }
            else
            {
                return r1.ReasonPhrase;
            }
        }

        public async Task<string> ExecPost(string command, string post)
        {
            var r1 = await this.Client.PostAsync(command, new StringContent("<?xml version=\"1.0\"?><set><value>" + post + "</value></set>"));
            if (r1.IsSuccessStatusCode)
            {
                return await r1.Content.ReadAsStringAsync();
            }
            else
            {
                return r1.ReasonPhrase;
            }
        }

        public async Task<ButtonDescription[]> GetDefinition()
        {
            var result = new ButtonDescription[] { };

            var item = CommandsResponseReader.Read();
            var f = item.Cgi.First(x => x.Name == "get_camprop");
            var desc = f.Http_method.Cmd1.Param1.First(x => x.Name == "desc");

            //var d = new Dictionary<string, string>();
            //foreach (var x in desc.Cmd2.Param2)
            //{
            //    var response = await this.Exec("get_camprop.cgi?com=desc&param=" + x.Name);
            //    d[x.Name] = response;
            //}
            //var descList = d["desclist"];
            //var parsedDescs = CommandsResponseReader.GetDescList(declist);
            var parsedDescs = CommandsResponseReader.GetDescList();
            result = ExtractButtondescription(parsedDescs).ToArray();
            return result;
        }

        public static IEnumerable<ButtonDescription> ExtractButtondescription(CommandsResponseReader.Desclist parsedDescs)
        {
            var s = LambdaHelper._(((string method, string name) => $"{method}_{name}.cgi?com={method}&propname={name}"));
            foreach (var parsed in parsedDescs.Desc)
            {
                var bd = new ButtonDescription();
                bd.Name = parsed.Propname;
                if (parsed.Enum != null && parsed.Enum.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray() is var entries && entries.Any())
                {
                    bd.PossibleValues = entries;
                }
                if (parsed.Attribute.Contains("set"))
                {
                    bd.SetCommand = s("set", parsed.Propname);
                }
                if (parsed.Attribute.Contains("get"))
                {
                    bd.GetCommand = s("get", parsed.Propname);
                }
                yield return bd;
            }
        }

        public async Task<bool> Record()
        {
            var r1 = await this.Client.GetAsync("switch_cammode.cgi?mode=rec&lvqty=1280x0960");
            if (r1.IsSuccessStatusCode)
            {
                var r2 = await this.Client.GetAsync("exec_takemisc.cgi?com=startliveview&port=" + this.IPEndPoint.Port);

                if (r2.IsSuccessStatusCode)
                {
                    await this.TryStartCamera();
                    return true;
                }
            }

            return default;
        }
    }
}
