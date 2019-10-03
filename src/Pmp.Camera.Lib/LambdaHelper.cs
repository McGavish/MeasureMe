using System;

namespace Pmp.Camera.Lib
{
    public static class LambdaHelper
    {
        public static Func<T> _<T>(this Func<T> t)
        {
            return t;
        }

        public static Func<T, T1> _<T, T1>(this Func<T, T1> t)
        {
            return t;
        }


        public static Func<T, T1, T2> _<T, T1, T2>(this Func<T, T1, T2> t)
        {
            return t;
        }
    }
}
