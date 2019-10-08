using System.Linq;
using System.Runtime.Serialization;

namespace MisteriousMachine.COM
{
    public class BootloaderUcSpecs
    {
        public static int GetCommandCount<T>()
        {
            var props = typeof(T).GetProperties();

            var result = props
                 .Select(x => (x, x.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault() as DataMemberAttribute))
                 .Where(x => x.Item2 != null).Max(x => x.Item2.Order);

            return result;
        }

        public void Initialize(byte[] data)
        {
            var props = this.GetType().GetProperties();
            var withDataMembers = props
                 .Select(x => (x, x.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault() as DataMemberAttribute))
                 .Where(x => x.Item2 != null);

            var ordered = withDataMembers.OrderBy(x => x.Item2.Order).ToArray();

            for (int i = 0; i < ordered.Max(x => x.Item2.Order); i++)
            {
                var element = ordered.ElementAt(i);
                var dataItem = data.ElementAt(i);

                var prop = withDataMembers.FirstOrDefault(x => x.Item2.Order == i);
                prop.x.SetValue(this, dataItem);
            }
        }

        [DataMember(Order = 0)]
        public byte Version { get; set; }

        [DataMember(Order = 1)]
        public byte Get { get; set; }
        [DataMember(Order = 2)]
        public byte GetVersionandReadProtectionStatus { get; set; }
        [DataMember(Order = 3)]
        public byte GetID { get; set; }
        [DataMember(Order = 4)]
        public byte ReadMemory { get; set; }
        [DataMember(Order = 5)]
        public byte Go { get; set; }
        [DataMember(Order = 6)]
        public byte WriteMemory { get; set; }
        [DataMember(Order = 7)]
        public byte Erase { get; set; }
        [DataMember(Order = 8)]
        public byte WriteProtect { get; set; }
        [DataMember(Order = 9)]
        public byte WriteUnprotect { get; set; }
        [DataMember(Order = 10)]
        public byte ReadProtect { get; set; }
        [DataMember(Order = 11)]
        public byte ReadUnprotect { get; set; }

    }
}