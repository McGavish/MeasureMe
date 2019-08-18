using System;
using System.IO.Ports;
using System.Text;

namespace MisteriousMachine.COM
{

    public class UcApi : IDisposable
    {
        public string ComPort { get; }
        public SerialPort Serial { get; }

        protected virtual SerialPort Create(string comPort)
        {
            var sp = new SerialPort(comPort);
            sp.BaudRate = 38400;
            sp.Encoding = Encoding.ASCII;
            sp.Handshake = Handshake.None;
            sp.Parity = Parity.Even;
            sp.StopBits = StopBits.One;
            sp.DataBits = 8;
            sp.WriteTimeout = 500;
            sp.ReadTimeout = 1500;
            sp.DtrEnable = false;
            sp.RtsEnable = false;
            sp.NewLine = "\r";
            return sp;
        }

        public bool Parse(byte response)
        {
            if(response == UcApi.ACK)
            {
                return true;
            }
            else if(response == UcApi.NACK)
            {
                return false;
            }
            throw new InvalidOperationException();
        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
        }

        public UcApi(string comPort)
        {
            this.ComPort = comPort;
            this.Serial = this.Create(comPort);
            this.Serial.Open();
            Console.WriteLine("OK");
        }

        public const byte ACK = 0x79;
        public const byte NACK = 0x1f;

        public void InvokeWithoutConfirmation(byte[] bytes)
        {
            this.Serial.Write(bytes, 0, bytes.Length);
        }

        public byte ReadByte()
        {
            return (byte)this.Serial.ReadByte();
        }

        public string Invoke(string command)
        {
            var asciiBytes = Encoding.ASCII.GetBytes(command);
            this.Serial.Write(asciiBytes, 0, command.Length);
            var c = string.Empty;
            return c;
        }

        public string Invoke(Func<Commands, string> f)
        {
            var value = f(new Commands());
            return this.Invoke(value);
        }

        public void Dispose()
        {
            if (this.Serial != null)
            {
                this.Serial.Close();
                this.Serial.Dispose();
            }
        }
    }
}
