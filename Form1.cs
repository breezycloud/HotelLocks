using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using ProRFL.UI.Data;
using ProRFL.UI.Handlers;
using ProRFL.UI.Services;
using System.ComponentModel.Design;

namespace ProRFL.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (!File.Exists(AppSetting.Url))
            {
                File.WriteAllText(AppSetting.Url!, "http://95.217.230.104:8083/");
            }

            if (!File.Exists(AppSetting.Cards))
            {
                File.WriteAllText(AppSetting.Cards!, "0");
            }

            if (!File.Exists(AppSetting.Token))
            {
                string[] strings = new string[] { "", "" };
                File.WriteAllLines(AppSetting.Token!, strings);
            }

            if (!File.Exists(AppSetting.Cert))
            {
                File.WriteAllText(AppSetting.Cert!, "-----BEGIN RSA PRIVATE KEY-----\r\nMIICXAIBAAKBgQCqGKukO1De7zhZj6+H0qtjTkVxwTCpvKe4eCZ0FPqri0cb2JZfXJ/DgYSF6vUp\r\nwmJG8wVQZKjeGcjDOL5UlsuusFncCzWBQ7RKNUSesmQRMSGkVb1/3j+skZ6UtW+5u09lHNsj6tQ5\r\n1s1SPrCBkedbNf0Tp0GbMJDyR4e9T04ZZwIDAQABAoGAFijko56+qGyN8M0RVyaRAXz++xTqHBLh\r\n3tx4VgMtrQ+WEgCjhoTwo23KMBAuJGSYnRmoBZM3lMfTKevIkAidPExvYCdm5dYq3XToLkkLv5L2\r\npIIVOFMDG+KESnAFV7l2c+cnzRMW0+b6f8mR1CJzZuxVLL6Q02fvLi55/mbSYxECQQDeAw6fiIQX\r\nGukBI4eMZZt4nscy2o12KyYner3VpoeE+Np2q+Z3pvAMd/aNzQ/W9WaI+NRfcxUJrmfPwIGm63il\r\nAkEAxCL5HQb2bQr4ByorcMWm/hEP2MZzROV73yF41hPsRC9m66KrheO9HPTJuo3/9s5p+sqGxOlF\r\nL0NDt4SkosjgGwJAFklyR1uZ/wPJjj611cdBcztlPdqoxssQGnh85BzCj/u3WqBpE2vjvyyvyI5k\r\nX6zk7S0ljKtt2jny2+00VsBerQJBAJGC1Mg5Oydo5NwD6BiROrPxGo2bpTbu/fhrT8ebHkTz2epl\r\nU9VQQSQzY1oZMVX8i1m5WUTLPz2yLJIBQVdXqhMCQBGoiuSoSjafUhV7i1cEGpb88h5NBYZzWXGZ\r\n37sJ5QsW+sJyoNde3xH8vdXhzU7eT82D6X/scw9RZz+/6rCJ4p0=\r\n-----END RSA PRIVATE KEY-----");
            }
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddAuthorizationCore();
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
            });
            services.AddHttpClient<CustomAuthenticationStateProvider>();
            services.AddHttpClient<IUserService, UserService>(client => { client.BaseAddress = new Uri(File.ReadAllText(AppSetting.Url!)); });
            services.AddHttpClient<IBookingService, BookingService>(client => { client.BaseAddress = new Uri(File.ReadAllText(AppSetting.Url!)); });
            services.AddScoped<ICardService, CardService>();
            services.AddSingleton<IAppSetting, AppSetting>();
            services.AddSingleton<BackgroundJob>();

            services.AddDbContext<AppDbContext>();

            services.AddScoped<AuthenticationStateProvider>(options => options.GetRequiredService<CustomAuthenticationStateProvider>());
            services.AddTransient<CustomAuthorizationHandler>();
            services.AddWindowsFormsBlazorWebView();
            var provider = services.BuildServiceProvider();
            var context = provider.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
            webView.HostPage = @"wwwroot\index.html";
            webView.Services = provider;
            webView.RootComponents.Add<App>("#app");            
            
        }


        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            File.Delete(AppSetting.Token!);
            Environment.Exit(0);
        }
    }
}