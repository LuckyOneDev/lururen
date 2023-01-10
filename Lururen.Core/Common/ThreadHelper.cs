using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lururen.Core.Common
{
    public static class ThreadHelper
    {
        /// <summary>
        /// Starts periodic task on a new thread with provided cancellation token
        /// </summary>
        /// <param name="action"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static CancellationTokenSource PeriodicThread(Action action, TimeSpan interval)
        {
            using CancellationTokenSource source = new CancellationTokenSource();
            var cancellationToken = source.Token;
            new Thread(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    action();
                    await Task.Delay(interval, cancellationToken);
                }
            }).Start();
            return source;
        }
    }
}
