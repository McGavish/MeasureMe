using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MisteriousMachine.COM
{
    public static class MethodInfoExtension
    {
        public static string MethodSignature(this MethodInfo mi)
        {
            String[] param = mi.GetParameters()
                          .Select(p => String.Format("{0} {1}", p.ParameterType.Name, p.Name))
                          .ToArray();


            string signature = String.Format("{0} {1}({2})", mi.ReturnType.Name, mi.Name, String.Join(",", param));

            return signature;
        }
    }
    public class Commands
    {
        public string UpdateFirmware() => Frame("FirmwareUpdate");

        public string Frame(string command) => $"~{command}@";
        public string Move(int number, int steps) => Frame($"sm{number}.{steps}");
        public string ChangeSpeed(int number, int speed) => Frame($"sm{number}.speed.{speed}");
        public string Set(int number, bool value) => Frame((value ? $"on" : $"off") + "." + number);
        public string ReturnSpeed(int number) => ChangeSpeed(number, 0);

        public IEnumerable<string> Help()
        {
            var methods = this.GetType().GetMethods();

            foreach (var item in methods)
            {
                yield return item.MethodSignature();
            }
        }

        public string Match(IEnumerable<KeyValuePair<string, string>> query)
        {
            var methods = typeof(Commands).GetMethods();
            var action = query.FirstOrDefault(q => q.Key.Equals("action", StringComparison.InvariantCultureIgnoreCase)).Value ?? query.FirstOrDefault().Value;
            var matched = methods.FirstOrDefault(x => x.Name.Equals(action, StringComparison.InvariantCultureIgnoreCase));

            if (matched != null)
            {
                var parameters = matched.GetParameters();
                var parametersWithEqualiventFromQuery = new List<KeyValuePair<string, object>>();
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    var found = query.FirstOrDefault(q => q.Key.Equals(parameter.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (found.Key == null)
                    {
                        found = query.ElementAt(i + 1);
                    }
                    object r = found.Value;
                    if (parameter.ParameterType != typeof(string))
                    {
                        r = Convert.ChangeType(r, parameter.ParameterType);
                    }
                    parametersWithEqualiventFromQuery.Add(new KeyValuePair<string, object>(parameter.Name, r));
                }

                return (string)matched.Invoke(new Commands(), parametersWithEqualiventFromQuery.Select(x => x.Value).ToArray());
            }
            return null;
        }
    }
}
