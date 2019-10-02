using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace MisteriousMachine.COM
{
    public class BtManager
    {
       
    }

    class Program
    {
        

        static async Task Main(string[] args)
        {
            var com  = UcClient.GetFirstConnected();
            using (var api = new UcClient(com))
            {
                var command = new Commands();
                Console.WriteLine("Commands:");
                command.Help().ToList().ForEach(x => Console.WriteLine(x));

                while (true)
                {
                    var line = Console.ReadLine();
                    var entries = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    var query = entries.Select(x => x.Split("=", StringSplitOptions.RemoveEmptyEntries)).Select(x => new KeyValuePair<string, string>(x.First(), x.Last()));
                    if (query.Any())
                    {
                        var commandString = command.Match(query);
                        Console.WriteLine(commandString);
                        if (commandString != null)
                        {
                            api.Invoke(commandString);
                            var result = api.Serial.ReadLine();
                            Console.WriteLine(result);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Try again");
                    }
                }

            }
        }

        private static void Sp_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
        }

        private static void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
