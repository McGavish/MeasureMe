using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pmp.Camera.Lib;

namespace Pmp.Camera.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CameraController : ControllerBase
    {
        private OiCamera camera;

        public CameraController(OiCamera camera)
        {
            this.camera = camera;
        }

        [HttpGet("connected")]
        public async Task<IActionResult> GetConnected()
        {
            return this.Ok(this.camera.IsConnected);
        }

        [HttpGet("stream")]
        public async Task<IActionResult> GetVideoStream(CancellationToken ct)
        {
            var imageStream = new ImageStream(this.camera, ct);
            return imageStream;
        }
    }
}