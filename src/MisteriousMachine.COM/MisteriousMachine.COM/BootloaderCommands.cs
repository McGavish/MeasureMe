using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MisteriousMachine.COM
{
    public class BootloaderCommands : Commands
    {
        public BootloaderUcSpecs DownloadedSpecs { get; private set; }

        public UcClient Device { get; }

        public BootloaderCommands(UcClient api)
        {
            this.Device = api;
        }

        private static async Task ReadMem(BootloaderCommands commands, uint address)
        {
            commands.BootloaderReadMem(address);
        }

        private static async Task WriteProgram(BootloaderCommands commands)
        {
            Console.WriteLine("Input file:");
            var file = "D:\\Bins\\power-board.bin"; //Console.ReadLine();
            var fi = new FileInfo(file);
            var bytes = File.ReadAllBytes(fi.FullName);
            var startAddress = 0x08_00_00_00u;
            commands.BootloaderWriteToMemoryFromAddress(startAddress, bytes);
            commands.BootloadeGo(startAddress);
        }

        public void InvokeSymetrical(byte command)
        {
            var xored = (byte)(command ^ 0xff);
            this.Device.InvokeWithoutConfirmation(new[] { (byte)command, (byte)xored });
        }

        public bool EnterBootLoaderMode()
        {
            for (int i = 0; i < 5; i++)
            {
                var attemps = this.Device.Invoke(c => c.UpdateFirmware());
                var line = this.Device.Serial.ReadLine();
                Console.WriteLine(line);
            }
            return true;
        }


        public bool BootloaderStartSession()
        {
            this.Device.InvokeWithoutConfirmation(new[] { (byte)0x7f });
            var @byte = this.Device.ReadByte();
            if (@byte == UcClient.ACK)
            {
                return true;
            }
            return false;
        }

        public BootloaderUcSpecs BootloaderGetCommands()
        {
            this.InvokeSymetrical(0x00);
            if (this.Device.IsNextByteAck())
            {
                var result = new BootloaderUcSpecs();
                var max = BootloaderUcSpecs.GetCommandCount<BootloaderUcSpecs>();

                var commandsLength = this.Device.ReadByte();
                if (commandsLength == max)
                {
                    var bytes = new List<byte>();
                    for (int i = 0; i <= max; i++)
                    {
                        var commandByte = this.Device.ReadByte();
                        bytes.Add(commandByte);
                    }

                    if (this.Device.IsNextByteAck())
                    {
                        result.Initialize(bytes.ToArray());
                        this.DownloadedSpecs = result;
                        return result;
                    }
                }
                Console.WriteLine("BootloaderGetCommands ok");
            }
            else
            {
                Console.WriteLine("BootloaderGetCommands failed");
            }
            return null;
        }

        internal bool BootloaderUnprotectMemory()
        {
            this.InvokeSymetrical(this.DownloadedSpecs.WriteUnprotect);
            var result = this.Device.ReadByte();
            if (this.Device.Parse(result))
            {
                result = this.Device.ReadByte();
                if (this.Device.Parse(result))
                {
                    return true;
                }
            }

            return false;
        }

        public bool BootloaderWriteToMemoryFromAddress(uint address, byte[] content)
        {
            uint word = 127;

            for (uint i = 0; i < content.Length / word; i++)
            {
                Console.WriteLine($"Progress: {i * word} from {content.Length} bytes");
                var data = content.Skip((int)(i * word)).Take((int)word).Select(x => (byte)0xff).ToArray();

                var result = this.BootloaderWriteDataChunkFromAddress((address + word * i), data, word);
                if (!result)
                {
                    Debugger.Break();
                }
            }

            return true;
        }

        public bool BootloadeGo(uint address)
        {
            var bytes = address.GetBytes();
            this.InvokeSymetrical(this.DownloadedSpecs.Go);
            if (Device.IsNextByteAck())
            {
                Device.InvokeWithoutConfirmation(bytes);
                Device.InvokeWithoutConfirmation(bytes.CheckSum());
            }
            return true;
        }

        public bool BootloaderReadMem(uint addressInt)
        {
            this.InvokeSymetrical(this.DownloadedSpecs.ReadMemory);
            if (this.Device.IsNextByteAck())
            {
                var result = this.SendAddress(addressInt);
                if (result)
                {
                    this.InvokeSymetrical(16);
                    if (this.Device.IsNextByteAck())
                    {
                        Console.WriteLine("Bytes:");
                        for (var i = 0; i < (16 / 4) ; i++)
                        {
                            var data1 = this.Device.ReadByte();
                            var data2 = this.Device.ReadByte();
                            var data3 = this.Device.ReadByte();
                            var data4 = this.Device.ReadByte();
                            var iny = BitConverter.ToString(new[] { data1, data2, data3, data4 });
                            Console.Write(iny);
                        }
                        Console.WriteLine("");
                        return true;
                    }
                }
                else
                {
                    throw new Exception("Bad address format");
                }
            }
            return false;
        }

        public bool BootloaderWriteDataChunkFromAddress(uint addressInt, byte[] content, uint words)
        {
            this.InvokeSymetrical(this.DownloadedSpecs.WriteUnprotect);
            var initialAkc = this.Device.ReadByte();
            if (this.Device.Parse(initialAkc))
            {
                var addressAkc = this.SendAddress(addressInt);
                if (addressAkc)
                {
                    var messageLength = (byte)(words - 1);
                    var sum = new[] { messageLength }.Concat(content).CheckSum();
                    var toSend = new byte[] { messageLength }.Concat(content).Concat(new[] { sum }).ToArray();
                    this.Device.InvokeWithoutConfirmation(toSend);

                    if (!this.Device.IsNextByteAck())
                    {
                        throw new Exception("Writing to memory exception");
                    }
                    else
                    {
                        Console.WriteLine($"Succesfully sent to {addressInt}");
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SendAddress(uint addressInt)
        {
            var address = addressInt.GetBytes();
            this.Device.InvokeWithoutConfirmation(address);
            var checksum = address.CheckSum();
            this.Device.InvokeWithoutConfirmation(new[] { checksum });
            Console.WriteLine($"Checksum ok {checksum}");

            return this.Device.IsNextByteAck();
        }
    }

    public static class IntExtensions
    {
        public static byte[] GetBytes(this uint u)
        {
            return BitConverter.GetBytes(u).Reverse().ToArray();
        }
    }
}