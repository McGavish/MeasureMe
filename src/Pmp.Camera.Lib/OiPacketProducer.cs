using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Pmp.Camera.Lib
{
    public class OiUdpStreamBlock : BaseProducer<RawFrame>, IReceiverProducer<object, RawFrame>
    {
        private IPEndPoint _Endpoint;

        private UdpClient Client { get; set; }
        public IPEndPoint Endpoint { get => this._Endpoint; private set => this._Endpoint = value; }


        public OiUdpStreamBlock(OiCamera camera)
        {
            this.Endpoint = camera.IPEndPoint;
        }

        private bool isDisposed = false;

        public async override Task TryStart()
        {
            if (this.Client == null)
            {
                this.Client = new UdpClient(this.Endpoint.Port);
                this.Client.DontFragment = true;
                await base.TryStart();
                await Task.Run(() =>
                {
                    this.Client.BeginReceive(this.ReceiveCallback, this._Endpoint);
                });
            }
          
        }
        public void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Byte[] receiveBytes = this.Client.EndReceive(ar, ref this._Endpoint);

                this.Writer.TryWrite(new RawFrame { Data = receiveBytes });
                if (!this.isDisposed)
                {
                    this.Client.BeginReceive(this.ReceiveCallback, this._Endpoint);
                }
            }
            catch (Exception ex)
            {
            }
        }


        public async Task Continue(object t = null)
        {
        }

        public override void Dispose()
        {
            if (this.Client != null)
            {
                this.isDisposed = true;
                this.Client.Close();
                this.Client.Dispose();
            }
        }
    }
}