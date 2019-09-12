using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pmp.Camera.WebApp
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class caminfo
    {

        private string modelField;

        /// <remarks/>
        public string model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }
    }


    public class OiCamera : IDisposable
    {
        public OiCamera(IPAddress address = default, string dcimDir = "/DCIM/100OLYMP")
        {
            if (address == default)
            {
                this.Address = new IPAddress(new byte[] { 192, 168, 0, 10 });
            }
            else
            {
                this.Address = address;
            }

            this.Client = new HttpClient();
            this.Client.BaseAddress = new Uri("http://" + this.Address.ToString());
            this.DcimDir = dcimDir;
        }

        public async Task<(byte[], string)> GetThumbnail(string photo)
        {
            var result = await this.Client.GetAsync(WithDir(FormatRequest("get_thumbnail"), photo));

            if (result.IsSuccessStatusCode)
            {
                return (await result.Content.ReadAsByteArrayAsync(), result.Content.Headers.ContentType.Parameters.FirstOrDefault()?.Value);
            }

            return default;
        }

        public async Task<string> GetConnectMode()
        {
            var result = await this.Client.GetAsync(FormatRequest("get_connectmode"));

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

                var result = await this.Client.GetAsync(WithDirDirectory(FormatRequest("get_imglist")));

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
            var result = await this.Client.GetAsync(WithDir(FormatRequest("get_thumbnail"), photo));

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


        string FormatRequest(string r) => $"{r}.cgi";
        string WithDirDirectory(string r) => $"{r}?DIR={DcimDir}";
        string WithDir(string r, string dir) => $"{r}?DIR={DcimDir}/{dir}";
        string WithMode(string r, Mode mode) => $"{r}?mode={Enum.GetName(typeof(Mode), mode)}";
        string WithShutter(string r, string shutter) => $"{r}?com={shutter}";
        string WithPhoto(string r, string dir) => $"{r}/{dir}";

        public async Task<string> Name()
        {
            const string name = "get_caminfo";

            using (var result = await this.Client.GetStreamAsync(FormatRequest(name)))
            {
                var reader = new XmlSerializer(typeof(caminfo));
                var type = (caminfo)reader.Deserialize(result);
                return type.model;
            }
        }

        public IPAddress Address { get; }
        public HttpClient Client { get; }
        public string DcimDir { get; }

        public void Dispose()
        {

        }


        public async Task<bool> Shutter(Shutter s)
        {
            var shutter = Enum.GetName(typeof(Shutter), s).Substring(1);
            var result = await this.Client.GetAsync(WithShutter(FormatRequest("exec_shutter"), shutter));

            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return default;
        }

        public async Task<bool> SetMode(Mode mode)
        {
            var r = WithMode(FormatRequest("switch_cammode"), mode);
            var result = await this.Client.GetAsync(r);

            if (result.IsSuccessStatusCode)
            {
                return true;
            }

            return default;
        }

        public async Task<IPEndPoint> Record()
        {
            var r1 = await this.Client.GetAsync("switch_cammode.cgi?mode=rec&lvqty=0320x0240");
            if (r1.IsSuccessStatusCode)
            {
                var r2 = await this.Client.GetAsync("exec_takemisc.cgi?com=startliveview&port=54578");

                if (r2.IsSuccessStatusCode)
                {
                    var e = new IPEndPoint(this.Address, 54578);
                    return e;
                }
            }


            return default;
        }
    }

    public enum Shutter
    {
        _1stpush,
        _2ndpush,
        _1st2ndpush,
        _2nd1strelease,
        _2ndrelease,
        _1strelease
    }

    public enum Mode
    {
        play = 2,
        shutter = 4
    }

    public class OiCameraBootstraper
    {
        public static OiCamera Camera { get; private set; }

        private static object @lock = new object();
        public static async Task<OiCamera> Connect(CancellationToken ct = default)
        {

            if (Camera != null)
            {
                return Camera;
            }
            try
            {
                var camera = new OiCamera();
                var name = await camera.Name();
                Camera = camera;
            }
            catch (Exception)
            {
                return null;
            }
            return Camera;
        }
    }
}
