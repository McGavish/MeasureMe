using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pmp.Camera.Lib
{
    public class OiCameraBootstraper
    {
        public static OiCamera Camera { get; private set; }

        public static async Task<OiCamera> Connect(CancellationToken ct = default)
        {

            if (Camera != null)
            {
                return Camera;
            }
            try
            {
                var camera = new OiCamera();
                var name = await camera.GetName();
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
