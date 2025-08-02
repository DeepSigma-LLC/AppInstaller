using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Dispatching;

namespace AppInstallerUI.Classes
{
        public static class DispatcherQueueExtensions
        {
            public static Task EnqueueAsync(this DispatcherQueue dispatcherQueue, Action callback)
            {
                var tcs = new TaskCompletionSource<object?>();

                dispatcherQueue.TryEnqueue(() =>
                {
                    try
                    {
                        callback();
                        tcs.SetResult(null);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }
                });

                return tcs.Task;
            }
        }
}
