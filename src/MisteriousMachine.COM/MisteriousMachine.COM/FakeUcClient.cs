using Pmp.Camera.Lib;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading.Tasks;

namespace MisteriousMachine.COM
{
    public class FakeUcClient : IUcScannerClient
    {
        private bool isConnected = true;

        public string ComPort => "NO DEVICE";

        public bool IsConnected
        {
            get => isConnected; set
            {
                isConnected = value;
                PropertyHelper.RaisePropertyChanged(this, () => this.IsConnected, this.PropertyChanged);
            }
        }
        public SerialPort Serial => throw new NotImplementedException();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            Trace.WriteLine("Dispose");
        }
        public string Invoke(Func<Commands, string> f)
        {
            Trace.WriteLine("Invoke " + f.ToString());
            return "";
        }
        public string Invoke(string command)
        {
            Trace.WriteLine("Invoke " + command);
            return "";
        }
        public void InvokeWithoutConfirmation(byte[] bytes)
        {
            Trace.WriteLine("InvokeWithoutConfirmation " + bytes);

        }
        public bool Parse(byte response)
        {
            Trace.WriteLine("Parse " + response);
            return true;
        }
        public byte ReadByte()
        {
            Trace.WriteLine("ReadByte ");
            return 0;
        }
        public Task TryStartAsync()
        {
            Task.Run(async() =>
            {
                while(true)
                {
                    await Task.Delay(500);
                    this.IsConnected = !this.IsConnected;
                }
            });
            Trace.WriteLine("ReadTryStartAsync");
            return Task.CompletedTask;
        }

    }
}
