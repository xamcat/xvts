using System;
using System.Collections.Generic;

namespace Algorithms.Common
{
    public static class Helpers
    {
        /// <summary>
        /// Swaps two values in an IList<T> collection given their indexes.
        /// </summary>
        public static void Swap<T>(this IList<T> list, int firstIndex, int secondIndex)
        {
            if (list.Count < 2 || firstIndex == secondIndex)
                return;

            var temp = list[firstIndex];
            list[firstIndex] = list[secondIndex];
            list[secondIndex] = temp;
        }
    }
}
