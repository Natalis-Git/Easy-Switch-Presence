
using System;
using System.Diagnostics;
using System.Windows.Threading;




namespace EasySwitchPresence.Models
{

    /// <summary>
    /// A (very) crude time counter that wraps System.Diagnostics.Stopwatch and System.Windows.Threading.DispatcherTimer
    /// to regularly fire a DispatcherTimer.Tick event at the interval provided.
    /// </summary>
    /// <remarks>
    /// Using a dispatcher internally allows for controlling what thread to run the timer on. In the case of 
    /// GUI applications for example, it can be ran on the UI thread to reduce tickrate issues with a bound UI element.
    /// </remarks>
    public class DispatchCounter
    {
        /// <summary>
        /// Proxy for Tick EventHandler of internal DispatcherTimer instance. Attach handler here using assignment
        /// </summary>
        public EventHandler OnTickProxy { set => _timer.Tick += value; }

        /// <summary>
        /// Total elapsed time since Start(). Resets on Stop()
        /// </summary>
        public TimeSpan TimeElapsed { get => _stopwatch.Elapsed; }


        private Stopwatch _stopwatch;    
        private DispatcherTimer _timer;


        public DispatchCounter(TimeSpan interval, Dispatcher dispatcher, 
            DispatcherPriority priority = DispatcherPriority.Normal)
        {
            _stopwatch = new Stopwatch();

            _timer = new DispatcherTimer(priority, dispatcher);
            _timer.Interval = interval;
        }


        public void Start()
        {
            _timer.Start();
            _stopwatch.Start();
        }


        public void Stop()
        {
            _timer.Stop();
            _stopwatch.Stop();
            _stopwatch.Reset();
        }


        public void Restart()
        {
            Stop();
            Start();
        }
    }

}
