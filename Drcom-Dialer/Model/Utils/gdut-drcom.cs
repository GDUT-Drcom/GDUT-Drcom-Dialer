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

        [DllImport("gdut-drcom.dll")]
        public static extern void set_remote_ip(ref byte[] ip, int len);

        [DllImport("gdut-drcom.dll")]
        public static extern void set_keep_alive1_flag(ref byte[] flag, int len);

        [DllImport("gdut-drcom.dll")]
        public static extern void set_enable_crypt(int enable);

        [DllImport("gdut-drcom.dll")]
        private static extern void get_version(ref byte[] version);

        /// <summary>
        /// DLL的版本
        /// </summary>
        public static string Version
        {
            get
            {
                byte[] ver = new Byte[128];
                try
                {
                    get_version(ref ver);
                    return Encoding.Default.GetString(ver);
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
