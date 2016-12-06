using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Drcom_Dialer.Model.Utils
{
    static class GDUT_Drcom
    {
        /// <summary>
        /// 认证
        /// 感觉有点问题
        /// </summary>
        /// <returns></returns>
        [DllImport("gdut-drcom.dll")]
        public static extern int auth();

        [DllImport("gdut-drcom.dll", CharSet = CharSet.Ansi)]
        public static extern void set_remote_ip(string ip, int len);

        [DllImport("gdut-drcom.dll", CharSet = CharSet.Ansi)]
        public static extern void set_keep_alive1_flag(string flag, int len);

        [DllImport("gdut-drcom.dll")]
        public static extern void set_enable_crypt(int enable);

        [DllImport("gdut-drcom.dll")]
        private static extern void get_version(IntPtr version);

        [DllImport("gdut-drcom.dll", CharSet = CharSet.Ansi)]
        public static extern void set_log_file(string log_file, int len);

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
                    return "";
                }
            }
        }
    }
}
