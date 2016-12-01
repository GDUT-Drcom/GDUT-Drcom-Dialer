using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Drcom_Dialer.Model.Utils;

namespace Drcom_Dialer.Model
{
    /// <summary>
    ///     心跳包代理类
    /// </summary>
    internal static class HeartBeatProxy
    {
        public enum HeadBeatStatus
        {
            Success = 0,
            BindPortFail = -1,
            PermissonDeny = -2,
            RecvTimedOut = -3,
            Unknown = -99
        }

        /// <summary>
        ///     心跳线程退出事件
        /// </summary>
        public static event Action<int> HeartbeatExited;

        /// <summary>
        ///     初始化心跳包
        /// </summary>
        public static HeadBeatStatus Init()
        {
            try
            {
                byte[] ipaddr = Encoding.Default.GetBytes(DialerConfig.AuthIP);
                byte[] flag = Encoding.Default.GetBytes("6a");
                GDUT_Drcom.set_enable_crypt(1);
                GDUT_Drcom.set_remote_ip(ref ipaddr, ipaddr.Length);
                GDUT_Drcom.set_keep_alive1_flag(ref flag, flag.Length);
                return HeadBeatStatus.Success;
            }
            catch (Exception e)
            {
                Log4Net.WriteLog(e.Message, e);
                return HeadBeatStatus.Unknown;
            }
        }

        /// <summary>
        ///     进行心跳操作
        /// </summary>
        public static HeadBeatStatus Heartbeat()
        {
            try
            {
                HeadBeatThread = new Thread(() =>
                {
                    int res = GDUT_Drcom.auth();
                    HeartbeatExited?.Invoke(res);
                });
                HeadBeatThread.Start();
                return HeadBeatStatus.Success;
            }
            catch
            {
                return HeadBeatStatus.Unknown;
            }
        }

        /// <summary>
        ///     终止线程
        /// </summary>
        public static void Kill()
        {
            if (HeadBeatThread == null)
            {
                return;
            }
            if (HeadBeatThread.IsAlive)
            {
                HeadBeatThread.Abort();
            }
            HeadBeatThread = null;
        }

        /// <summary>
        ///     反初始化
        /// </summary>
        public static HeadBeatStatus Deinit()
        {
            return HeadBeatStatus.Success;
        }

        /// <summary>
        ///     心跳线程
        /// </summary>
        private static Thread HeadBeatThread
        {
            set;
            get;
        }
    }
}