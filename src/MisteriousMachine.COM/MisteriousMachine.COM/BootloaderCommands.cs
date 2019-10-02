using System;
using System.Linq;
using System.Runtime.Serialization;

namespace MisteriousMachine.COM
{
    public class BootloaderCommands
    {
        [DataMember(Order = 0)]
        public byte Get { get; set; }
        [DataMember(Order = 1)]
        public byte GetVersionandReadProtectionStatus { get; set; }
        [DataMember(Order = 2)]
        public byte GetID { get; set; }
        [DataMember(Order = 3)]
        public byte ReadMemory { get; set; }
        [DataMember(Order = 4)]
        public byte Go { get; set; }
        [DataMember(Order = 5)]
        public byte WriteMemory { get; set; }
        [DataMember(Order = 6)]
        public byte Erase { get; set; }
        [DataMember(Order = 7)]
        public byte WriteProtect { get; set; }
        [DataMember(Order = 8)]
        public byte WriteUnprotect { get; set; }
        [DataMember(Order = 9)]
        public byte ReadProtect { get; set; }
        [DataMember(Order = 10)]
        public byte ReadUnprotect { get; set; }

        public bool EnterBootloaderMode(UcClient api)
        {
            api.InvokeWithoutConfirmation(new[] { (byte)0x7f });
            var @byte = api.ReadByte();
            if (@byte == UcClient.ACK)
            {
                return true;
            }
            return false;
        }

        public void InvokeSymetrical(UcClient api, byte command)
        {
            var xored = (byte)(command ^ 0xff);
            api.InvokeWithoutConfirmation(new[] { (byte)command, (byte)xored });
        }

        public bool Write(UcClient api, byte[] address, byte[] content)
        {
            this.InvokeSymetrical(api, this.WriteMemory);
            var initialAkc = api.ReadByte();
            if (api.Parse(initialAkc))
            {
                api.InvokeWithoutConfirmation(address);
                var checksum = (byte)(address[0] ^ address[1] ^ address[2] ^ address[3]);
                api.InvokeWithoutConfirmation(new[] { checksum });
                var addressAkc = api.ReadByte();

                var data = content;
                if (content.Length % 4 != 0)
                {
                    var missing = 4 - (content.Length % 4);
                    data = content.Concat(Enumerable.Range(0, missing).Select(x => (byte)0x00)).ToArray();
                }

                if (api.Parse(addressAkc))
                {
                    for (var offset = 0; offset < data.Length / 4; offset++)
                    {
                        var bytesToSend = data.Skip(offset * 4).Take(4);
                        var bytesChecksum = bytesToSend.Aggregate((x, y) => (byte)(x ^ y));
                        var valid = api.ReadByte();
                        if (!api.Parse(valid))
                        {
                            throw new Exception("Writing to memory exception");
                        }
                    }
                }
            }
            return false;
        }

        public bool GetCommands(UcClient api)
        {
            this.InvokeSymetrical(api, 0x00);
            var akc = api.ReadByte();
            if (akc == UcClient.NACK)
            {
                return false;
            }

            var props = this.GetType().GetProperties();
            var withDataMembers = props
                 .Select(x => (x, x.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault() as DataMemberAttribute))
                 .Where(x => x.Item2 != null);

            var max = (withDataMembers.Max(x => x.Item2.Order));
            var hexByte = byte.Parse(max.ToString(), System.Globalization.NumberStyles.HexNumber);
            var commandsLength = api.ReadByte();

            if (commandsLength == max + 1)
            {
                var bootloaderVersionByte = api.ReadByte();
                var bootloaderVersion = bootloaderVersionByte;

                for (int i = 0; i <= max; i++)
                {
                    var commandByte = api.ReadByte();
                    var prop = withDataMembers.FirstOrDefault(x => x.Item2.Order == i);
                    prop.x.SetValue(this, commandByte);
                }
                var enfOfFrame = api.ReadByte();
                if (enfOfFrame == UcClient.ACK)
                {
                    return true;
                }
            }

            return false;
        }
    }
}