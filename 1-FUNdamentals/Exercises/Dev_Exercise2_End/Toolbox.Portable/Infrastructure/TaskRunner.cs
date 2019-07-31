using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Toolbox.Portable.Infrastructure
{
    public static class TaskRunner
    {
        public static async Task RunSafe(Task task, Action<Exception> onError = null, CancellationToken token = default(CancellationToken))
        {
            Exception exception = null;

            try
            {
                if (!token.IsCancellationRequested)
                {
                    await task;
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task Cancelled");
            }
            catch (AggregateException e)
            {
                var ex = e.InnerException;
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                exception = ex;
            }
            catch (Exception e)
            {
                exception = e;
            }

            if (exception != null)
            {
                Debug.WriteLine(exception);
                onError?.Invoke(exception);
            }
        }
    }
}
