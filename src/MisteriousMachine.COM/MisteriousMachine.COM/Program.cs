using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Threading.Tasks;

namespace MisteriousMachine.COM
{
    class Program
    {
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
            var endpointIdKEy = "AssociationEndpointID";
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
                    using (var api = new UcApi(val))
                    {

                        api.Invoke(x => x.ReturnSpeed(1));
                        var speed = api.Serial.ReadLine();
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

        static async Task Main(string[] args)
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
                return;
            }

            Console.WriteLine($"Using {active}");
            var com = bts[active];
            using (var api = new UcApi(com))
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
