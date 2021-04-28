using System.Collections.Generic;

namespace Albion.Code
{
    public static class Extensions
    {
        public static IEnumerable<T> ConcatSafe<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first != null)
            {
                foreach (var item in first)
                    yield return item;
            }

            if (second != null)
            {
                foreach (var item in second)
                    yield return item;
            }
        }
    }
}
