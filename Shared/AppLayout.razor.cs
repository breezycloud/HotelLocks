using Microsoft.AspNetCore.Components;
using ProRFL.UI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProRFL.UI.Data.BackgroundJob;

namespace ProRFL.UI.Shared
{
    public partial class AppLayout : LayoutComponentBase, IDisposable
    {
        [Inject] private BackgroundJob BackgroundJob { get; set; } = default!;
        [Inject] private IAppSetting _app { get; set; } = default!;

        Point cursorPoint;
        int minutesIdle = 0;

        protected override void OnInitialized()
        {
            BackgroundJob.JobExecuted += IdleTimer!;
        }
        private bool isIdle(int minutes)
        {
            return minutesIdle >= minutes;
        }
        void IdleTimer(object sender, JobExecutedEventArgs args)
        {
            if (Cursor.Position != cursorPoint)
            {
                // The mouse moved since last check
                minutesIdle = 0;
            }
            else
            {
                // Mouse still stopped
                minutesIdle++;
                if (isIdle(5))
                {
                    _app.ClearToken();
                    nav.NavigateTo("/", true);
                }
            }
            // Save current position
            cursorPoint = Cursor.Position;
        }

        public void Dispose()
        {
            BackgroundJob.JobExecuted -= IdleTimer!;
        }
    }
}
