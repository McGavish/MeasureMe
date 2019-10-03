using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Pmp.Camera.Lib;
using Xunit;

namespace Pmp.Camera.Tests
{


    public class CameraTest
    {
        [Fact]
        public void GenerateApi()
        {
            var item = CommandsResponseReader.Read();
            var lines = new List<string>();
            lines.Add("public class GeneratedApi {");

            foreach (var cgi in item.Cgi)
            {
                var name = cgi.Name;
                var method = cgi.Http_method;
                var parametersTuple = new List<(string, string)[]>();

                if (method.Cmd1 != null)
                {
                    if (method.Cmd1.Param1 != null && method.Cmd1.Param1.Any())
                    {
                        foreach (var l1 in method.Cmd1.Param1)
                        {
                            var parameters = new[] { (method.Cmd1.Name, l1.Name) }.ToList();
                            if (l1.Cmd3 != null)
                            {
                                parameters.Add((l1.Cmd3.Name, l1.Cmd3.Param3.Name));
                            }

                            if (l1.Cmd2 != null)
                            {
                                var cmd2 = l1.Cmd2;
                                if (cmd2.Param2 != null && cmd2.Param2.Any())
                                {
                                    foreach (var i in cmd2.Param2)
                                    {
                                        parameters.Add((l1.Cmd2.Name, i.Name));
                                    }
                                }
                                else
                                {
                                    parameters.Add((l1.Cmd2.Name, "value"));
                                }
                            }
                            else
                            {
                                parametersTuple.Add(parameters.ToArray());
                            }
                        }
                    }
                    else
                    {
                        parametersTuple.Add(new (string, string)[] { (method.Cmd1.Name, "value") });

                    }
                }
                else
                {
                    parametersTuple.Add(new (string, string)[] { });
                }



                foreach (var tuple in parametersTuple.Distinct())
                {
                    var result = $@"
                    public string {method.Type}_{name}({String.Join(", ", tuple.Select(x => $"string {x.Item1} = \"{x.Item2}\""))}){{
                        return """";
                    }}";
                    lines.Add(result);
                }
            }
            lines.Add("}");
            File.WriteAllLinesAsync("./../../../test.cs", lines);
        }

        [Fact]
        public async void CanListFiles()
        {
            var camera = await OiCameraBootstraper.Connect();
            var mode = await camera.SetMode(Mode.play);
            var files = await camera.ListFiles();

            Assert.NotEmpty(files);
        }

        [Fact]
        public async void SetPlay()
        {
            var camera = await OiCameraBootstraper.Connect();
            var mode = await camera.SetMode(Mode.play);

            Assert.NotEqual(default, mode);
        }

        [Fact]
        public async void ListItemsStartingWith()
        {
            var di = new DirectoryInfo(@"D:\datas\360e01af-69d7-4688-a386-4ddcc4b291fb1280x0960");
            var files = di.EnumerateFiles();
            var filesWithContent = files.ToArray().ToDictionary(x => Int32.Parse(x.FullName.Split("test-")[1].Split(".")[0]), x => File.ReadAllBytes(x.FullName));

            var startingWithStartFrame = filesWithContent.Where(x => x.Value[0] == 0x90 && x.Value[1] == 0x60).OrderBy(x => x.Key).ToArray();
            var startingWithEndFram = filesWithContent.Where(x => x.Value[0] == 0x80 && x.Value[1] == 0x60).OrderBy(x => x.Key).ToArray();
            var middleFrames = filesWithContent.Where(x => x.Value[0] == 0x80 && x.Value[1] == 0x60).OrderBy(x => x.Key).ToArray();
        }


        //public async void TestFrame()
        //{
        //    var di = new DirectoryInfo(@"D:\datas\360e01af-69d7-4688-a386-4ddcc4b291fb1280x0960");
        //    var files = di.EnumerateFiles();
        //    var filesWithContent = files.ToArray().ToDictionary(x => Int32.Parse(x.FullName.Split("test-")[1].Split(".")[0]), x => File.ReadAllBytes(x.FullName));
        //    var o = new OiFrameReader();
        //    var items = filesWithContent.Select(x => (x, OiFrameReader.FrameNumer(x.Value))).ToArray();
        //    var ordered = items.OrderBy(x => x.Item2).ToArray();

        //    foreach (var item in filesWithContent.OrderBy(x => x.Key))
        //    {
        //        o.AppendFrame(item.Value);
        //    }
        //}

        //public async void Repeat()
        //{
        //    while (true)
        //    {
        //        await AreFramesOk();
        //        await Task.Delay(400);
        //    }
        //}

        [Fact]
        public async Task ShouldReadFrames()
        {
            var root = new DirectoryInfo(@"D:\datas");
            var dirs = root.EnumerateDirectories();

            foreach (var directoryInfo in dirs)
            {
                var di = new DirectoryInfo($@"D:/datas/results/{directoryInfo.Name}");
                di.Create();

                var cts = new CancellationTokenSource();
                var token = cts.Token;
                var udpClient = new FakePacketProducer(directoryInfo);
                var jpegConverter = new OiUdpStreamToJpegBlock();
                await udpClient.TryStart();
                await jpegConverter.TryStart();

                var task = Task.Run(async () =>
                {
                    while (await udpClient.Reader.WaitToReadAsync(cts.Token) && udpClient.Reader.TryRead(out var frame) && !token.IsCancellationRequested)
                    {
                        if (await jpegConverter.Writer.WaitToWriteAsync(cts.Token))
                        {
                            await jpegConverter.Continue(frame);
                        }
                    }
                    jpegConverter.Dispose();
                });
                var sw = Stopwatch.StartNew();
                var task2 = Task.Run(async () =>
                 {
                     while (sw.ElapsedMilliseconds < 2000)
                     {
                         await udpClient.Continue();
                     }
                     udpClient.Dispose();
                 });

                var i = 0;
                while (await jpegConverter.Reader.WaitToReadAsync() && jpegConverter.Reader.TryRead(out var item) && item != null)
                {
                    i++;
                    await File.WriteAllBytesAsync($@"D:/datas/results/{directoryInfo.Name}/r-{i}.jpeg", item.Data);
                }

                Console.WriteLine(directoryInfo.Name + " " + i);

                cts.Cancel();
            }
        }

        [Fact]
        public async void GetConnectMode()
        {
            var camera = await OiCameraBootstraper.Connect();
            var mode = await camera.GetConnectMode();
            Assert.NotNull(mode);
        }

        [Fact]
        public async Task Test()
        {
           
            var item = CommandsResponseReader.Read();
            var f = item.Cgi.First(x => x.Name == "get_camprop");
            var desc = f.Http_method.Cmd1.Param1.First(x => x.Name == "desc");
      
            var parsedDescs = CommandsResponseReader.GetDescList();

            var objs = OiCamera.ExtractButtondescription(parsedDescs);
            throw new Exception(Newtonsoft.Json.JsonConvert.SerializeObject(objs));
        }

        [Fact]
        public async void SetShutter()
        {
            var camera = await OiCameraBootstraper.Connect();
            var mode = await camera.SetMode(Mode.shutter);
            Assert.NotEqual(default, mode);
        }

        [Fact]
        public async void GoPlayModeDoPhoto()
        {
            var camera = await OiCameraBootstraper.Connect();
            var mode = await camera.SetMode(Mode.shutter);
            if (mode)
            {
                await Task.Delay(5000);
                mode = await camera.Shutter(Shutter._1st2ndpush);
                await Task.Delay(5000);
                mode = await camera.Shutter(Shutter._2nd1strelease);
            }

            Assert.NotEqual(default, mode);
        }
    }
}