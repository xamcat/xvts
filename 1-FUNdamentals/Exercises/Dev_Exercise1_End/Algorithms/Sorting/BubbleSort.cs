using System.Collections.Generic;
using System.Threading;
using Algorithms.Common;

namespace Algorithms.Sorting
{
    public static class BubbleSorter
    {
        public static IList<T> BubbleSort<T>(this IList<T> collection, CancellationToken ct, Comparer<T> comparer = null)
        {
            comparer = comparer ?? Comparer<T>.Default;
            return collection.BubbleSortAscending(ct, comparer);
        }

        /// <summary>
        /// Public API: Sorts ascending
        /// </summary>
        public static IList<T> BubbleSortAscending<T>(this IList<T> collection, CancellationToken token, Comparer<T> comparer)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                for (int index = 0; index < collection.Count - 1; index++)
                {
                    token.ThrowIfCancellationRequested();

                    if (comparer.Compare(collection[index], collection[index + 1])>0)
                    {
                        collection.Swap(index,index+1);
                    }
                }
            }

            return collection;
        }

        /// <summary>
        /// Public API: Sorts descending
        /// </summary>
        public static IList<T> BubbleSortDescending<T>(this IList<T> collection, CancellationToken token, Comparer<T> comparer)
        {
            for (int i = 0; i < collection.Count-1; i++)
            {
                for (int index = 1; index < collection.Count - i; index++)
                {
                    token.ThrowIfCancellationRequested();
                    
                    if (comparer.Compare(collection[index], collection[index - 1]) > 0)
                    {
                        collection.Swap(index-1, index);
                    }
                }
            }

            return collection;
        }
    }
}