using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Pmp.Camera.Lib
{
    public class OiUdpStreamToJpegBlock : BaseProducer<PictureFrame>
    {
        private int? CurrentFrame { get; set; }
        private List<byte> WorkingImageBytes { get; set; }
        public static byte[] StartFrameBytes => new byte[] { 0x90, 0x60 };
        public static byte[] MidFrameBytes => new byte[] { 0x80, 0x60 };
        public static byte[] EndFrameBytes => new byte[] { 0x80, 0xe0 };
        public static byte[] StartJpgBytes => new byte[] { 0xff, 0xd8 };
        public static byte[] EndJpgBytes => new byte[] { 0xff, 0xd9 };
        public byte[] EndHeadFrame;
        private int FuckedUpFrames;

        public byte[] TryGetFrame(byte[] bytes)
        {
            if (this.Check(bytes))
            {
                var frameNumber = FrameNumer(bytes);
                if (bytes.StartsWith(StartFrameBytes))
                {
                    this.FuckedUpFrames = 0;
                    this.EndHeadFrame = bytes.Skip(10).Take(2).ToArray();
                    var index = bytes.Locate(StartJpgBytes, 0).First();
                    this.WorkingImageBytes = new List<byte>(1280*1080);
                    this.WorkingImageBytes.AddRange(bytes.Skip(index));
                    this.CurrentFrame = frameNumber + 1;
                    return null;
                }
                if (bytes.StartsWith(MidFrameBytes) && this.WorkingImageBytes != null)
                {
                    var jpegIndex = bytes.Locate(this.EndHeadFrame, 6).First();
                    this.WorkingImageBytes.AddRange(bytes.Skip(jpegIndex + 2));
                    return null;
                }
                if (bytes.StartsWith(EndFrameBytes) && this.WorkingImageBytes != null)
                {
                    var jpegIndex = bytes.Locate(this.EndHeadFrame, 6).First();
                    var endJpegIndex = bytes.Locate(EndJpgBytes, jpegIndex + 2).First();
                    this.WorkingImageBytes.AddRange(bytes.Skip(jpegIndex + 2).Take(endJpegIndex - jpegIndex));
                    var br = this.WorkingImageBytes.ToArray();
                    this.WorkingImageBytes = null;
                    this.CurrentFrame = null;
                    if (this.FuckedUpFrames < 3)
                    {
                        return br;
                    }
                }
            }

            // Just skip
            return null;
        }

        private bool Check(byte[] bytes)
        {
            var l = FrameNumer(bytes);
            if (this.CurrentFrame == null)
            {
                this.CurrentFrame = l;
            }
            else
            {
                //if ((this.CurrentFrame + 1) != l)
                //{
                //    Console.Error.WriteLine($"Frame misscount, {this.CurrentFrame}, {l}");
                //    this.FuckedUpFrames++;
                //}
                this.CurrentFrame = l;
            }
            return true;
        }

        public async Task Continue(RawFrame raw)
        {
            var result = this.TryGetFrame(raw.Data);
            if (result != null)
            {
                await this.Writer.WriteAsync(new PictureFrame { Data = result });
            }
        }

        public static short FrameNumer(byte[] bytes)
        {
            var currentBytes = bytes.Skip(2).Take(2).ToArray();
            var l = BitConverter.ToInt16(currentBytes.Reverse().ToArray(), 0);
            return l;
        }

        public override Task TryStart()
        {
            var channel = Channel.CreateBounded<PictureFrame>(new BoundedChannelOptions(20)
            {
                AllowSynchronousContinuations = false,
                Capacity = 2,
                FullMode = BoundedChannelFullMode.DropOldest
            });

            this.Reader = channel.Reader;
            this.Writer = channel.Writer;
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.Writer.TryComplete();
        }
    }
}