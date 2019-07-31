using System;
using System.Collections.Generic;
using System.Threading;

namespace Algorithms.Sorting
{
    public static class InsertionSorter
    {
        public static void InsertionSort<T>(this IList<T> list, CancellationToken token, Comparer<T> comparer = null)
        {
            comparer = comparer ?? Comparer<T>.Default;

            // Do sorting if list is not empty.
            int i, j;
            for (i = 1; i < list.Count; i++)
            {
                token.ThrowIfCancellationRequested();

                T value = list[i];
                j = i - 1;

                while ((j >= 0) && (comparer.Compare(list[j], value) > 0))
                {
                    list[j + 1] = list[j];
                    j--;
                }

                list[j + 1] = value;
            }
        }
    }
}
