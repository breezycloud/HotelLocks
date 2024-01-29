using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ProRFL.UI.Data
{
    public class BackgroundJob
    {
        bool _Running = false;
        private System.Timers.Timer? _Timer;
        public class JobExecutedEventArgs : EventArgs { }
        public event EventHandler<JobExecutedEventArgs>? JobExecuted;

        void OnJobExecuted()
        {
            JobExecuted?.Invoke(this, new JobExecutedEventArgs());
        }

        public void StartExecuting()
        {
            if (!_Running)
            {
                _Timer = new System.Timers.Timer();
                _Timer.Interval = 60000;
                _Timer.Elapsed += HandleTimer!;
                _Timer.AutoReset = true;
                _Timer.Enabled = true;

                _Running = true;
            }
        }

        void HandleTimer(object source, ElapsedEventArgs e)
        {
            OnJobExecuted();
        }

        public void Dispose()
        {
            if (_Running)
            {
                _Timer = null;
                _Timer?.Dispose();
            }
        }
    }
}
