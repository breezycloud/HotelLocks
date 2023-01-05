using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using ProRFL.UI.Data;
using ProRFL.UI.Handlers;
using ProRFL.UI.Services;

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
                File.WriteAllText(AppSetting.Token!, "");
            }
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddAuthorizationCore();
            services.AddMudServices();
            services.AddHttpClient<CustomAuthenticationStateProvider>();
            services.AddHttpClient<IUserService, UserService>(client => { client.BaseAddress = new Uri(File.ReadAllText(AppSetting.Url!)); });
            services.AddHttpClient<IBookingService, BookingService>(client => { client.BaseAddress = new Uri(File.ReadAllText(AppSetting.Url!)); });
            services.AddScoped<ICardService, CardService>();

            services.AddScoped<AuthenticationStateProvider>(options => options.GetRequiredService<CustomAuthenticationStateProvider>());
            services.AddTransient<CustomAuthorizationHandler>();
            services.AddWindowsFormsBlazorWebView();
            webView.HostPage = @"wwwroot\index.html";
            webView.Services = services.BuildServiceProvider();
            webView.RootComponents.Add<App>("#app");            
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            File.Delete(AppSetting.Token!);
            Environment.Exit(0);

        }
    }
}