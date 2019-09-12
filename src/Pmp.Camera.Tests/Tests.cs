using Pmp.Camera.WebApp;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xunit;

namespace Pmp.Camera.Tests
{
    public class UnitTest1
    {
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
        public async void Record()
        {
            var camera = await OiCameraBootstraper.Connect();
            var endpoint = await camera.Record();
            
            var socket = new UdpClient(endpoint.Port);
            while (true)
            {
                var bytes = socket.Receive(ref endpoint);
                using (var stream = new FileStream("test.bin", FileMode.Append))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }

            }
            socket.Close();

            Assert.NotEqual(default, endpoint);
        }

        [Fact]
        public async void GetConnectMode()
        {
            var camera = await OiCameraBootstraper.Connect();
            var mode = await camera.GetConnectMode();

            Console.WriteLine(mode);

            Assert.NotNull(mode);
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
                //mode = await camera.SetMode(Mode.play);
            }

            Assert.NotEqual(default, mode);
        }
    }
}