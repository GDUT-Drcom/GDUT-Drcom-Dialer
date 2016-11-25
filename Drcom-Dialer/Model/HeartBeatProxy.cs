using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drcom_Dialer.Model
{
    /// <summary>
    /// 心跳包代理类
    /// </summary>
    static class HeartBeatProxy
    {
        /// <summary>
        /// 初始化心跳包
        /// </summary>
        public static status init()
        {
            try
            {
                byte[] ipaddr = System.Text.Encoding.Default.GetBytes(DialerConfig.AuthIP);
                byte flag = 0x6a;
                Utils.GDUT_Drcom.set_enable_crypt(1);
                Utils.GDUT_Drcom.set_remote_ip(ref ipaddr, ipaddr.Length);
                Utils.GDUT_Drcom.set_keep_alive1_flag(ref flag, 1);
                return status.Success;
            }
            catch(Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
                return status.Unknown;
            }
            
        }
        /// <summary>
        /// 进行心跳操作
        /// </summary>
        public static status heartbeat()
        {
            try
            {
                if (Utils.GDUT_Drcom.auth() == -1)
                    return status.BindPortFail;
                return status.Success;
            }
            catch
            {
                return status.Unknown;
            }
            
        }
        /// <summary>
        /// 反初始化
        /// </summary>
        public static status deinit()
        {
            return status.Success;
        }

        public enum status
        {
            Success = 0,
            BindPortFail = -1,
            PermissonDeny = -2,
            RecvTimedOut = -3,
            Unknown = -99
        }
    }
}
