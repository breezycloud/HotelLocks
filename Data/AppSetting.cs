﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProRFL.UI.Data
{
    public class AppSetting
    {
        public static string? Token = $@"{Application.LocalUserAppDataPath}\token.txt";
        public static string? Url = $@"{Application.LocalUserAppDataPath}\url.txt";
        public static string? Cards = $@"{Application.LocalUserAppDataPath}\issued.txt";

    }
}
