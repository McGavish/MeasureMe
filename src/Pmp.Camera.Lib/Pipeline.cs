using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pmp.Camera.Lib
{
    public class Pipeline
    {
        private readonly IReceiverProducer<object, RawFrame> receiver;

        public Pipeline(IReceiverProducer<object, RawFrame> receiver)
        {
            this.receiver = receiver;
        }

        public async Task<IProducer<PictureFrame>> Start(CancellationToken cts)
        {
            var jpegConverter = new OiUdpStreamToJpegBlock();
            await jpegConverter.TryStart();

            var converterTask = Task.Run(async () =>
            {
                try
                {
                    while (await this.receiver.Reader.WaitToReadAsync() && !cts.IsCancellationRequested)
                    {
                        if (this.receiver.Reader.TryRead(out var frame) && frame != null)
                        {
                            await jpegConverter.Continue(frame);
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }, cts);

            return jpegConverter;
        }
    }
}