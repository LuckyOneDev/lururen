using System;
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
        public static System.Timers.Timer PeriodicThread(Action action, TimeSpan interval)
        {
            // Create a timer with a two second interval.
            var aTimer = new System.Timers.Timer(interval);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (object source, ElapsedEventArgs e) => { action(); };
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            return aTimer;
        }
    }
}
