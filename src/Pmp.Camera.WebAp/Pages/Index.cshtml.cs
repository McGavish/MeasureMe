using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pmp.Camera.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        public string Name { get; private set; }
        public string[] Urls { get; private set; } = { };
        public async Task OnGetAsync()
        {
            var camera = await OiCameraBootstraper.Connect(CancellationToken.None);
            try
            {
                this.Name = await camera.Name();
                this.Urls = await camera.ListFiles();
            }
            catch (Exception)
            {

            }
        }
    }

}
