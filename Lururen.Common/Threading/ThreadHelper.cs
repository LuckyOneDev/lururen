using System.Diagnostics;

namespace Lururen.Common.Threading
{
    public static class ThreadHelper
    {
        /// <summary>
        /// Starts periodic task on a new thread with provided cancellation token
        /// </summary>
        /// <param name="action"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static CancellationTokenSource StartPeriodicThread(Action<double> action, TimeSpan interval)
        {
            CancellationTokenSource ts = new();
            new Thread(() =>
            {
                Stopwatch sw = new();
                while (!ts.Token.IsCancellationRequested)
                {
                    sw.Restart();
                    action.Invoke(sw.ElapsedTicks); // Questionable line. Probably doesn't work as intended
                    while (sw.Elapsed < interval) { }
                }
            }).Start();
            return ts;
        }
    }
}
