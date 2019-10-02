using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pmp.Camera.Lib;

namespace Pmp.Camera.Api.Controllers
{
    internal class ImageStream : IActionResult
    {
        private readonly CancellationToken ct;
        private readonly byte[] newLine;
        public readonly string Boundary = "DupaDupa";

        public OiCamera Camera { get; }

        public ImageStream(OiCamera camera, CancellationToken ct)
        {
            this.newLine = Encoding.UTF8.GetBytes("\r\n");
            this.Camera = camera;
            this.ct = ct;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var writer = context.HttpContext.Response.Body;
            var sw = Stopwatch.StartNew();
            context.HttpContext.Response.StatusCode = 200;
            context.HttpContext.Response.ContentType = "multipart/x-mixed-replace;boundary=" + this.Boundary;
            var frame = this.Camera.CurrentFrame;
            while (!this.ct.IsCancellationRequested)
            {
                var currentFrame = this.Camera.CurrentFrame;
                if (sw.ElapsedMilliseconds > 20 && currentFrame != frame)
                {
                    var header = $"--{this.Boundary}\r\nContent-Type: image/jpeg\r\nContent-Length: {frame.Data.Length}\r\n\r\n";
                    var headerData = Encoding.UTF8.GetBytes(header);
                    await writer.WriteAsync(headerData, 0, headerData.Length);
                    await writer.WriteAsync(currentFrame.Data);
                    await writer.WriteAsync(this.newLine, 0, this.newLine.Length, this.ct);
                    await writer.WriteAsync(this.newLine, 0, this.newLine.Length, this.ct);
                    await writer.FlushAsync(this.ct);
                    frame = currentFrame;
                    sw.Restart();
                }
                else
                {
                    await Task.Yield();
                }
            }
        }
    }
}