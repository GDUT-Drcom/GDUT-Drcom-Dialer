using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Drcom_Dialer.Model.Utils
{
    internal static class GDUT_Drcom
    {
        public static readonly string fileName = ".\\gdut-drcom.dll";

        public delegate int auth_t();
        public delegate void set_str_t(string str, int len);
        public delegate void set_enable_crypt_t(int enable);
        public delegate void get_version_t(IntPtr version);

        private static get_version_t get_version;
        public static auth_t auth;
        public static auth_t exit_auth;
        public static set_enable_crypt_t set_enable_crypt;
        public static set_str_t set_remote_ip;
        public static set_str_t set_keep_alive1_flag;
        public static set_str_t set_log_file;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        private static IntPtr Handle = IntPtr.Zero;

        public static void Load()
        {
            Handle = LoadLibrary(fileName);
            if (Handle == IntPtr.Zero)
            {
                Log4Net.WriteLog($"加载DLL失败({Marshal.GetLastWin32Error()})");
            }
            else
            {
                try
                {
                    get_version = Marshal.GetDelegateForFunctionPointer<get_version_t>(GetProcAddress(Handle, nameof(get_version)));
                    auth = Marshal.GetDelegateForFunctionPointer<auth_t>(GetProcAddress(Handle, nameof(auth)));
                    exit_auth = Marshal.GetDelegateForFunctionPointer<auth_t>(GetProcAddress(Handle, nameof(exit_auth)));
                    set_enable_crypt = Marshal.GetDelegateForFunctionPointer<set_enable_crypt_t>(GetProcAddress(Handle, nameof(set_enable_crypt)));
                    set_remote_ip = Marshal.GetDelegateForFunctionPointer<set_str_t>(GetProcAddress(Handle, nameof(set_remote_ip)));
                    set_keep_alive1_flag = Marshal.GetDelegateForFunctionPointer<set_str_t>(GetProcAddress(Handle, nameof(set_keep_alive1_flag)));
                    set_log_file = Marshal.GetDelegateForFunctionPointer<set_str_t>(GetProcAddress(Handle, nameof(set_log_file)));
                }
                catch (Exception e)
                {
                    Log4Net.WriteLog("加载DLL函数失败", e);
                }
            }
        }

        public static void Unload()
        {
            if (Handle != IntPtr.Zero)
            {
                FreeLibrary(Handle);
                Handle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// DLL的版本
        /// </summary>
        public static string Version
        {
            get
            {
                try
                {
                    var ver = Marshal.AllocHGlobal(Marshal.SizeOf<byte>() * 128);
                    get_version(ver);
                    string sver = Marshal.PtrToStringAnsi(ver);
                    Marshal.FreeHGlobal(ver);
                    return sver;
                }
                catch (Exception e)
                {
                    Log4Net.WriteLog(e.Message, e);
                    return "0.0.0";
                }
            }
        }
    }
}
