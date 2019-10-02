using System;
using System.Collections.Concurrent;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Pmp.Camera.Lib
{
    public interface IReceiverProducer<TR, TP> : IProducer<TP>, IReceiver<TR>
    {
        
    }
    
    public interface IProducer<T> : IDisposable
    {
        ChannelReader<T> Reader { get; }
        Task TryStart();
    }
}