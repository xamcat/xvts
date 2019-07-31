using System;
using System.Collections.Generic;
using System.Threading;
using Algorithms.Common;

namespace Algorithms.Sorting
{
    public static class QuickSorter
    {
        public static void QuickSort<T>(this IList<T> collection, CancellationToken token, Comparer<T> comparer = null)
        {
            int startIndex = 0;
            int endIndex = collection.Count - 1;

            comparer = comparer ?? Comparer<T>.Default;

            collection.InternalQuickSort(startIndex, endIndex, token, comparer);
        }

        private static void InternalQuickSort<T>(this IList<T> collection, int leftmostIndex, int rightmostIndex, CancellationToken token, Comparer<T> comparer)
        {
            if (leftmostIndex < rightmostIndex)
            {
                token.ThrowIfCancellationRequested();
                
                int wallIndex = collection.InternalPartition(leftmostIndex, rightmostIndex, comparer);
                collection.InternalQuickSort(leftmostIndex, wallIndex - 1, token, comparer);
                collection.InternalQuickSort(wallIndex + 1, rightmostIndex, token, comparer);
            }
        }

        private static int InternalPartition<T>(this IList<T> collection, int leftmostIndex, int rightmostIndex, Comparer<T> comparer)
        {
            int wallIndex, pivotIndex;

            // Choose the pivot
            pivotIndex = rightmostIndex;
            T pivotValue = collection[pivotIndex];

            // Compare remaining array elements against pivotValue
            wallIndex = leftmostIndex;

            for (int i = leftmostIndex; i <= (rightmostIndex - 1); i++)
            {
                if (comparer.Compare(collection[i], pivotValue) <= 0)
                {
                    collection.Swap(i, wallIndex);
                    wallIndex++;
                }
            }

            collection.Swap(wallIndex, pivotIndex);

            return wallIndex;
        }

    }

}
