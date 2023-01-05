using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProRFL.UI.Data;

public partial class ProRFL
{
    const string NativeLib = "proRFL";
    public static byte flagUSB = Convert.ToByte(1);
    static ProRFL()
    {
        NativeLibrary.SetDllImportResolver(typeof(ProRFL).Assembly, ImportResolver);
    }

    private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        IntPtr libHandle = IntPtr.Zero;
        //you can add here different loading logic
        if (libraryName == NativeLib && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            NativeLibrary.TryLoad("proRFL.dll", out libHandle);
        }        
        return libHandle;
    }
    [DllImport(NativeLib, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetDLLVersion([MarshalAs(UnmanagedType.LPStr)] StringBuilder sDllVer);

    [DllImport(NativeLib, EntryPoint = "initializeUSB", CallingConvention = CallingConvention.StdCall)]
    public static extern int initializeUSB(byte fUSB);

    [DllImport(NativeLib, EntryPoint = "Buzzer", CallingConvention = CallingConvention.StdCall)]
    public static extern int Buzzer(byte fUSB, int t);

    [DllImport(NativeLib, CallingConvention = CallingConvention.StdCall)]
    public static extern int ReadCard(byte fUSB, [MarshalAs(UnmanagedType.LPStr)] StringBuilder Buffer);

    [DllImport(NativeLib, CallingConvention = CallingConvention.StdCall)]
    public static extern int GuestCard(byte fUSB, int dlsCoID, int CardNo, int dai, int llock, int pdoors, string BTime, string ETime, string LockNo,
        [MarshalAs(UnmanagedType.LPStr)] StringBuilder CardHexStr);

    [DllImport(NativeLib, CallingConvention = CallingConvention.StdCall)]
    public static extern int CardErase(byte fUSB, int dlsCoID, [MarshalAs(UnmanagedType.LPStr)] StringBuilder CardHexStr);

    [DllImport(NativeLib, CallingConvention = CallingConvention.StdCall)]
    public static extern int LimitCard(byte fUSB, int dlsCoID, int CardNo, int dai, string BTime, string LCardNo,
        [MarshalAs(UnmanagedType.LPStr)] StringBuilder CardHexStr);

    [DllImport(NativeLib, CallingConvention = CallingConvention.StdCall)]
    public static extern int hex_a(string hex, string a, int length);

    [DllImport(NativeLib, CallingConvention = CallingConvention.StdCall)]
    public static extern int a_hex(string a, string hex, int length);

    [DllImport(NativeLib, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetCardTypeByCardDataStr(string CardDataStr, string CardType);

    [DllImport(NativeLib, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetGuestLockNoByCardDataStr(int dlsCoID, string CardDataStr, string LockNo);

    [DllImport(NativeLib, CallingConvention = CallingConvention.StdCall)]
    public static extern int GetGuestETimeByCardDataStr(int dlsCoID, string CardDataStr, string ETime);
}

