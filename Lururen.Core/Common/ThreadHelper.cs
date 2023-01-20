﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

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
        public static CancellationTokenSource StartPeriodicThread(Action action, TimeSpan interval, TextWriter? tw = null)
        {
            CancellationTokenSource ts = new();
            new Thread(() =>
            {
                Stopwatch sw = new();
                while (!ts.Token.IsCancellationRequested)
                {
                    sw.Restart();
                    action.Invoke();
                    while (sw.Elapsed < interval) { }
                }
            }).Start();
            return ts;
        }
    }
}
