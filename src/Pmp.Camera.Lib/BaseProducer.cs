using System;
using System.Collections.Concurrent;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Pmp.Camera.Lib
{
    public abstract class BaseProducer<T> : IProducer<T>
    {
        private Channel<T> channel;
        public ChannelReader<T> Reader { get; set; }
        public ChannelWriter<T> Writer;

        public virtual Task TryStart()
        {
            this.channel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions()
            {
                AllowSynchronousContinuations = true
            });
            this.Reader = this.channel.Reader;
            this.Writer = this.channel.Writer;
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            this.Writer.Complete();
        }
    }
}