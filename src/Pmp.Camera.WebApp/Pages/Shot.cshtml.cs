using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pmp.Camera.WebApp.Pages
{

    public class ShotModel : PageModel
    {

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var camera = await OiCameraBootstraper.Connect();
                var mode = await camera.SetMode(Mode.shutter);
                if (mode)
                {
                    await Task.Delay(2000);
                    mode = await camera.Shutter(Shutter._1st2ndpush);
                    await Task.Delay(2000);
                    mode = await camera.Shutter(Shutter._2nd1strelease);
                    await Task.Delay(2000);
                    mode = await camera.SetMode(Mode.play);
                }


            }
            catch (Exception)
            {
                throw;
            }

            return this.Redirect("/");
        }
    }
}