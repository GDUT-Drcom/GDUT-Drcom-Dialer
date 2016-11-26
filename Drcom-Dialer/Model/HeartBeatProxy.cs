using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
                byte[] ipaddr = Encoding.Default.GetBytes(DialerConfig.AuthIP);
                byte[] flag = Encoding.Default.GetBytes("6a");
                Utils.GDUT_Drcom.set_enable_crypt(1);
                Utils.GDUT_Drcom.set_remote_ip(ref ipaddr, ipaddr.Length);
                Utils.GDUT_Drcom.set_keep_alive1_flag(ref flag, flag.Length);
                return status.Success;
            }
            catch(Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
                return status.Unknown;
            }
            
        }

        /// <summary>
        /// 心跳线程
        /// </summary>
        private static Thread hbthd = null;
        /// <summary>
        /// 心跳线程退出事件
        /// </summary>
        public static event Action<int> HeartbeatExited;
        /// <summary>
        /// 进行心跳操作
        /// </summary>
        public static status heartbeat()
        {
            try
            {
                hbthd = new Thread(() =>
                {
                    int res = Utils.GDUT_Drcom.auth();
                    HeartbeatExited?.Invoke(res);
                });
                hbthd.Start();
                return status.Success;
            }
            catch
            {
                return status.Unknown;
            }

        }
        /// <summary>
        /// 终止线程
        /// </summary>
        public static void kill()
        {
            if (hbthd == null)
                return;
            if(hbthd.IsAlive)
                hbthd.Abort();
            hbthd = null;
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
