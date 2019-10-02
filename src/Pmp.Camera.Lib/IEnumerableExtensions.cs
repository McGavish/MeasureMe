using System.Collections.Generic;
using System.Linq;

namespace Pmp.Camera.Lib
{
    public static class EnumerableExtensions
    {
        public static int[] Locate<T>(this T[] self, T[] candidate, int from = 0)
        {
            if (IsEmptyLocate<T>(self, candidate))
                return new int[] { };

            var list = new List<int>();

            for (int i = from; i < self.Length; i++)
            {
                if (!IsMatch<T>(self, i, candidate))
                    continue;

                list.Add(i);
            }

            return list.Count == 0 ? new int[] { } : list.ToArray();
        }

        static bool IsMatch<T>(T[] array, int position, T[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
                if (!array[position + i].Equals(candidate[i]))
                    return false;

            return true;
        }

        static bool IsEmptyLocate<T>(T[] array, T[] candidate)
        {
            return array == null
                || candidate == null
                || array.Length == 0
                || candidate.Length == 0
                || candidate.Length > array.Length;
        }

        public static bool StartsWith<T>(this IEnumerable<T> @this, IEnumerable<T> second, int index = 0)
        {
            var secondCount = second.Count();
            if (@this.Count() < index + secondCount)
            {
                return false;
            }
            for (int i = 0; i < secondCount; i++)
            {
                var el = second.ElementAt(i);
                var el2 = @this.ElementAt(index + i);
                if (!el.Equals(el2))
                {
                    return false;
                }
            }
            return true;
        }
    }
}