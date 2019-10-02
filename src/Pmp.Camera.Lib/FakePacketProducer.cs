using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pmp.Camera.Lib
{
    public class RawFrame
    {
        public byte[] Data;
    }

    public class FakePacketProducer : BaseProducer<RawFrame>, IReceiverProducer<object, RawFrame>
    {
        private readonly DirectoryInfo directoryInfo;

        public FakePacketProducer(): this(new DirectoryInfo(@"D:\datas\4ae6fe3f-ce10-4e4b-871a-9bde265ceb7c"))
        {

        }

        public FakePacketProducer(DirectoryInfo directoryInfo)
        {
            this.directoryInfo = directoryInfo;
        }

        public FileInfo[] Files { get; set; }
        public int Index { get; set; }

        public override Task TryStart()
        {
            this.Index = 0;
            this.Files = this.directoryInfo.EnumerateFiles().OrderBy(x => Int32.Parse((string)x.FullName.Split("test-")[1].Split(".")[0])).ToArray();
            Task.Run(async () =>
            {
                while (true)
                {
                    if (this.Index >= this.Files.Length)
                    {
                        this.Reset();
                        return;
                    }
                    else
                    {
                        var content = await File.ReadAllBytesAsync(this.Files[this.Index++].FullName);
                        if (await this.Writer.WaitToWriteAsync())
                        {
                            this.Writer.TryWrite(new RawFrame
                            {
                                Data = content
                            });
                        }
                        return;
                    }
                    await Task.Delay(1);
                }
            });
            return base.TryStart();
        }

        public void Reset()
        {
            this.Index = 0;
        }

        public async Task Continue(object o = default)
        {
          
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}