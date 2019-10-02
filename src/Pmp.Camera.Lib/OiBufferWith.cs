using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pmp.Camera.Lib
{
    public class OiBufferWith : BaseProducer<Byte[]>, IReceiverProducer<Byte[], byte[]>
    {
        public Dictionary<short, Byte[]> SortedSet;
        public int Treshold = 30;
        public OiBufferWith()
        {
            this.SortedSet = new Dictionary<short, byte[]>();
        }

        public async Task Continue(byte[] t = null)
        {
            if (t != null)
            {
                lock (this.SortedSet)
                {
                    var fn = OiUdpStreamToJpegBlock.FrameNumer(t);
                    this.SortedSet.Add(fn, t);
                    if (this.SortedSet.Count > this.Treshold)
                    {
                        this.SortedSet = new Dictionary<short, Byte[]>(this.SortedSet.TakeLast(this.Treshold));
                        var minKey = this.SortedSet.Keys.Min();
                        var f = this.SortedSet[minKey];

                        if (f != null)
                        {
                            this.SortedSet.Remove(minKey);
                            this.Writer.TryWrite(f);
                        }
                    }
                }
            }
        }

        public override void Dispose()
        {

        }
    }
}