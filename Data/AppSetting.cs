using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProRFL.UI.Data
{
    public partial interface IAppSetting
    {
        void LogOut();
        void ClearToken();
    }
    public partial class AppSetting : IAppSetting
    {
        private NavigationManager _nav = default!;

        public static string? Sqlite = $@"{Application.LocalUserAppDataPath}\locks.db";
        public static string? Token = $@"{Application.LocalUserAppDataPath}\token.txt";
        public static string? Rooms = $@"C:\Users\nerdyamin\AppData\Local\ProRFL.UI\ProRFL.UI\1.0.0\data.txt";
        public static string? Url = $@"{Application.LocalUserAppDataPath}\url.txt";
        public static string? Cards = $@"{Application.LocalUserAppDataPath}\issued.txt";
        public static string? Cert = $@"{Application.LocalUserAppDataPath}\cert.txt";
        public static string? SuperUsername = "master@hotel.com";
        public static string? SuperPassword = "master#*321";

        public AppSetting(NavigationManager nav)
        {
            _nav = nav;
        }

        public void LogOut() 
        {
            string[] strings = ["", ""];
            File.WriteAllLines(Token!, strings);
            _nav.NavigateTo("/", true);
        }

        public void ClearToken()
        {
            string[] strings = ["", ""];
            File.WriteAllLines(Token!, strings);
        }
    }
}
