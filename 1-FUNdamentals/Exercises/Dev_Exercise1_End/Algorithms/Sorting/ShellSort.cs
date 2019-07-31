using System.Collections.Generic;
using System.Threading;
using Algorithms.Common;

namespace Algorithms.Sorting
{
    public static class ShellSorter
    {
        public static void ShellSort<T>(this IList<T> collection, CancellationToken token, Comparer<T> comparer = null)
        {
            comparer = comparer ?? Comparer<T>.Default;
            collection.ShellSortAscending(token, comparer);
        }

        public static void ShellSortAscending<T>(this IList<T> collection, CancellationToken token, Comparer<T> comparer)
        {
            bool flag = true;
            int d = collection.Count;
            while (flag || (d > 1))
            {
                flag = false;
                d = (d + 1) / 2;
                for (int i = 0; i < (collection.Count - d); i++)
                {
                    token.ThrowIfCancellationRequested();

                    if (comparer.Compare(collection[i + d], collection[i]) < 0)
                    {
                        collection.Swap(i + d, i);
                        flag = true;
                    }
                }
            }
        }
        
        public static void ShellSortDescending<T>(this IList<T> collection, CancellationToken token, Comparer<T> comparer)
        {
            bool flag = true;
            int d = collection.Count;
            while (flag || (d > 1))
            {
                flag = false;
                d = (d + 1) / 2;
                for (int i = 0; i < (collection.Count - d); i++)
                {
                    token.ThrowIfCancellationRequested();

                    if (comparer.Compare(collection[i + d], collection[i])>0)
                    {
                        collection.Swap(i+d,i);
                        flag = true;
                    }
                }
            }
        }
    }
}