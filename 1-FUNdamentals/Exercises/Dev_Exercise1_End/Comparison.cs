
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Algorithms.Sorting;

namespace async_ex
{
    public class Comparison
    {
        public async Task Compare()
        {
            var stopwatch = Stopwatch.StartNew();
            System.Console.WriteLine(await SortListsBest(1_000, 1_000));
            System.Console.WriteLine($"Comparison took {stopwatch.Elapsed.TotalSeconds} s \n");

            stopwatch.Restart();
            System.Console.WriteLine(await SortLists(1_000, 1_000));
            System.Console.WriteLine($"Comparison took {stopwatch.Elapsed.TotalSeconds} s \n");       
        }

        async Task<string> SortLists(double numTries = 1_000, double collectionCount = 1_000)
        {
            long bubbleWins = 0;
            long insertionWins = 0;
            long quickWins = 0;
            long shellWins = 0;
            long mergeWins = 0;
            long linqWins = 0;

            for (int i = 0; i < numTries; i++)
            {
                var lists = GetIntLists((int)collectionCount, 6);

                var cts = new CancellationTokenSource();

                var result = await Task.WhenAny( 
                    Benchmark.Quantify((ct) => lists[0].BubbleSort(ct), SortType.Bubble, cts),
                    Benchmark.Quantify((ct) => lists[1].InsertionSort(ct), SortType.Insertion, cts),
                    Benchmark.Quantify((ct) => lists[2].ShellSort(ct), SortType.Shell, cts),
                    Benchmark.Quantify((ct) => lists[3].QuickSort(ct), SortType.Quick, cts),
                    Benchmark.Quantify((ct) => lists[4].MergeSort(ct), SortType.Merge, cts),
                    Benchmark.Quantify((ct) => lists[5].OrderBy(x => x), SortType.Linq, cts)
                ).ConfigureAwait(false);

                var winner = await result.ConfigureAwait(false);
                cts.Cancel();

                switch (winner.sortType)
                {
                    case SortType.Bubble:
                        Interlocked.Increment(ref bubbleWins);
                        break;
                    case SortType.Insertion:
                        Interlocked.Increment(ref insertionWins);
                        break;
                    case SortType.Shell:
                        Interlocked.Increment(ref shellWins);
                        break;
                    case SortType.Quick:
                        Interlocked.Increment(ref quickWins);
                        break;
                    case SortType.Merge:
                        Interlocked.Increment(ref mergeWins);
                        break;
                    case SortType.Linq:
                        Interlocked.Increment(ref linqWins);
                        break;
                }
            }

            var fastest = new [] 
            {
                (bubbleWins, SortType.Bubble), 
                (insertionWins, SortType.Insertion), 
                (shellWins, SortType.Shell),
                (quickWins, SortType.Quick),
                (mergeWins, SortType.Merge),
                (linqWins, SortType.Linq)
            }.Aggregate((curMax, x) => (x.Item1) > curMax.Item1 ? x : curMax);

            return $"----------STATISTICS-----------\n"+
                   $"{fastest.Item2.ToString()} won! \n"+
                   $"Fastest: {(fastest.Item1 / numTries) * 100}%\n";
        }

        // The gate allowance for the semaphore depends on the amount of cores your machine has
        static SemaphoreSlim semphore = new SemaphoreSlim(4);
        async Task<string> SortListsBest(double numTries = 1_000, double collectionCount = 1_000)
        {
            long bubbleWins = 0;
            long insertionWins = 0;
            long quickWins = 0;
            long shellWins = 0;
            long mergeWins = 0;
            long linqWins = 0;

            await Task.WhenAll(new int[(int)numTries].Select(async (i)=>{
                try
                {
                    await semphore.WaitAsync().ConfigureAwait(false);
    
                    var lists = GetIntLists((int)collectionCount, 6);
                    var cts = new CancellationTokenSource();

                    var result = await Task.WhenAny( 
                        Benchmark.Quantify((ct) => lists[0].BubbleSort(ct), SortType.Bubble, cts),
                        Benchmark.Quantify((ct) => lists[1].InsertionSort(ct), SortType.Insertion, cts),
                        Benchmark.Quantify((ct) => lists[2].ShellSort(ct), SortType.Shell, cts),
                        Benchmark.Quantify((ct) => lists[3].QuickSort(ct), SortType.Quick, cts),
                        Benchmark.Quantify((ct) => lists[4].MergeSort(ct), SortType.Merge, cts),
                        Benchmark.Quantify((ct) => lists[5].OrderBy(x => x), SortType.Linq, cts)
                    ).ConfigureAwait(false);

                    var winner = await result;
                    cts.Cancel();

                    switch (winner.sortType)
                    {
                        case SortType.Bubble:
                            Interlocked.Increment(ref bubbleWins);
                            break;
                        case SortType.Insertion:
                            Interlocked.Increment(ref insertionWins);
                            break;
                        case SortType.Shell:
                            Interlocked.Increment(ref shellWins);
                            break;
                        case SortType.Quick:
                            Interlocked.Increment(ref quickWins);
                            break;
                        case SortType.Merge:
                            Interlocked.Increment(ref mergeWins);
                            break;
                        case SortType.Linq:
                            Interlocked.Increment(ref linqWins);
                            break;
                    }
                }
                finally
                {
                    semphore.Release();
                }
            }));

            var fastest = new [] 
            {
                (bubbleWins, SortType.Bubble), 
                (insertionWins, SortType.Insertion), 
                (shellWins, SortType.Shell),
                (quickWins, SortType.Quick),
                (mergeWins, SortType.Merge),
                (linqWins, SortType.Linq)
            }.Aggregate((curMax, x) => (x.Item1) > curMax.Item1 ? x : curMax);

            return $"----------STATISTICS-----------\n"+
                   $"{fastest.Item2.ToString()} won! \n"+
                   $"Fastest: {(fastest.Item1 / numTries) * 100}%\n";
        }

        Random rnd = new Random();
        List<List<int>> GetIntLists(int length, int numLists)
        {
            var lists = new List<List<int>>();
            for (int i = 0; i < numLists; i++)
            {
                lists.Add(new List<int>());
            }

            for (int i = 0; i<length; i++)
            {
                var next = rnd.Next(0,length);
                // var next = length - i;
                // var next = i;
                for (int j = 0; j < numLists; j++)
                {
                    lists[j].Add(next);
                }
            }

            return lists;
        }
    }
}