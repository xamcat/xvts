using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Algorithms.Sorting;

namespace async_ex
{
    public static class Benchmark
    {
        public static Task<(long time, SortType sortType)> Quantify(Action<CancellationToken> action, SortType sortType, CancellationTokenSource cts = null)
        {
            var tcs = new TaskCompletionSource<(long, SortType)>();

            var token = cts.Token;
            Task.Run(() =>
            {
                var stopwatch = Stopwatch.StartNew();

                action.Invoke(token);

                var time = stopwatch.ElapsedMilliseconds;
                tcs.TrySetResult((time, sortType));
            }, token);

            return tcs.Task;
        }
    }
}