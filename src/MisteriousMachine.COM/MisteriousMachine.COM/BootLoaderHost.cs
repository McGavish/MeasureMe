using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MisteriousMachine.COM
{
    public class BootLoaderHost
    {
        public BootLoaderHost(UcApi api)
        {
            this.Api = api;
        }

        public UcApi Api { get; }

        public async Task OpenToWrite()
        {
            for (int i = 0; i < 5; i++)
            {
                var attemps = this.Api.Invoke(c => c.UpdateFirmware());
                var line = this.Api.Serial.ReadLine();
                Console.WriteLine(line);
            }

            var commands = new BootloaderCommands();
            var isInBootloader = commands.EnterBootloaderMode(this.Api);
            await Task.Delay(300);
            if (isInBootloader)
            {
                var fetchCommands = commands.GetCommands(this.Api);
                if (fetchCommands)
                {
                    throw new Exception("Could not fetch commands");
                }
            }
            else
            {
                throw new Exception("Could not enter bootloader");
            }
        }

        public async Task Write()
        {
            throw new NotImplementedException();
        }
    }
}
