using Microsoft.AspNetCore.Components;
using MudBlazor;
using ProRFL.UI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static ProRFL.UI.Data.BackgroundJob;

namespace ProRFL.UI.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        MudTheme DefaultTheme = new MudTheme
        {
            Palette = new PaletteLight
            { 
                AppbarBackground = "#1B6EC2",
                Primary = "#1B6EC2"
            }
        };
        [Inject] private BackgroundJob BackgroundJob { get; set; } = default!;
        protected override void OnInitialized()
        {
            BackgroundJob.StartExecuting();
        }
    }
}
