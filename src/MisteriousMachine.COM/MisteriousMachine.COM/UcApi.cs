using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace MisteriousMachine.COM
{

    public class UcClient : IUcScannerClient
    {
        public const byte ACK = 0x79;
        public const byte NACK = 0x1f;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsConnected { get; private set; }
        public string ComPort { get; }
        public SerialPort Serial { get; }

        public Commands Commands { get; set; }

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
            if (response == UcClient.ACK)
            {
                return true;
            }
            else if (response == UcClient.NACK)
            {
                return false;
            }
            throw new InvalidOperationException();
        }

        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
        }

        public UcClient(SerialPortDescriptor portDescriptor) : this(portDescriptor.Port)
        {
            this.Commands = new Commands();

        }

        public UcClient(string comPort)
        {
            this.ComPort = comPort;
            this.Serial = this.Create(comPort);
        }

        public Task TryStartAsync()
        {
            this.IsConnected = true;
            this.Serial.Open();
            return Task.CompletedTask;
        }

        public bool IsNextByteAck()
        {
            return this.ReadByte() == ACK;
        }

        public bool IsNextByteNotNck()
        {
            return this.ReadByte() != NACK;
        }

        public void InvokeWithoutConfirmation(params byte[] bytes)
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
        static public SerialPortDescriptor GetFirstConnected()
        {
            Console.WriteLine("Discovering bts.");
            var bts = GetBtNameToPort();
            var active = default(string);
            foreach (var item in bts)
            {
                Console.WriteLine($"Found {item.Key} {item.Value}");
                var status = CheckAvailibility(item.Key);
                if (status)
                {
                    active = item.Key;
                }
                Console.WriteLine($"Status: {status}");
                Console.WriteLine();
            }

            if (active == null)
            {
                Console.WriteLine("No active device found.");
                return default;
            }

            Console.WriteLine($"Using {active}");
            var com = bts[active];
            return new SerialPortDescriptor { Port = com };
        }

        static public Dictionary<string, string> GetBtNameToPort()
        {
            var bts = new List<ManagementObject>();
            using (var searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_PnPEntity where Caption like ""UC#%"""))
            {
                using (var collection = searcher.Get())
                {
                    bts = collection.Cast<ManagementObject>().ToList();
                }
            }

            var ports = new List<ManagementObject>();
            using (var searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_PnPEntity where Caption like ""Standard Serial over Bluetooth link%"""))
            {
                using (var collection = searcher.Get())
                {
                    ports = collection.Cast<ManagementObject>().ToList();
                }
            }
            var endpointIdKey = "AssociationEndpointID";
            var matchingDictionary = new Dictionary<string, string>();

            foreach (var bt in bts)
            {
                var matchString = bt.Path.RelativePath.Split("_").Last();
                matchString = matchString.Substring(0, matchString.Length - 1);
                var matchedPort = ports.FirstOrDefault(x => x.Path.RelativePath.Split("&").LastOrDefault()?.Split("_").FirstOrDefault() == matchString);
                var friendlyName = matchedPort?.GetPropertyValue("Caption") as string;
                var com = friendlyName.Split(new[] { "(", ")" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                matchingDictionary[bt.GetPropertyValue("Caption") as string] = com;
            }

            return matchingDictionary;
        }

        static bool CheckAvailibility(string name)
        {
            var btToPort = GetBtNameToPort();
            if (btToPort.TryGetValue(name, out var val))
            {
                try
                {
                    using (var api = new UcClient(val))
                    {
                        api.TryStartAsync().Wait();
                        api.Invoke(x => x.ReturnSpeed(1));
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;

                }
            }
            return false;
        }
    }
}
