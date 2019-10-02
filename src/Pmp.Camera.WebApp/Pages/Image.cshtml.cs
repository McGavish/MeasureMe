using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pmp.Camera.Lib;

namespace Pmp.Camera.WebApp.Pages
{

    public class ImageModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string File { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var camera = await OiCameraBootstraper.Connect();
                var result = await camera.GetPhoto(this.File);

                return this.File(result.Item1, result.Item2 ?? "image/jpeg");
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
}