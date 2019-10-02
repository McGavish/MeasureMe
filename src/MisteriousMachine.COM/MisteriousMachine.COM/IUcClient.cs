using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading.Tasks;

namespace MisteriousMachine.COM
{
    public interface IUcScannerClient: IDisposable, INotifyPropertyChanged
    {
        string ComPort { get; }
        bool IsConnected { get; }
        SerialPort Serial { get; }

        void Dispose();
        string Invoke(Func<Commands, string> f);
        string Invoke(string command);
        void InvokeWithoutConfirmation(byte[] bytes);
        bool Parse(byte response);
        byte ReadByte();
        Task TryStartAsync();
    }
}