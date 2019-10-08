using System.Collections.Generic;
using System.Linq;

namespace MisteriousMachine.COM
{
    public static class ByteArrayExtensions
    {
        public static byte CheckSum(this IEnumerable<byte> @array)
        {
            var sum = @array.Aggregate((x, acc) => (byte)(x ^ acc));
            return sum;
        }
    }
}