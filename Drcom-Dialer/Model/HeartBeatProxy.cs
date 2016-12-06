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
        public static event EventHandler<int> HeartbeatExited;

        /// <summary>
        ///     初始化心跳包
        /// </summary>
        public static HeadBeatStatus Init()
        {
            try
            {
                string keep_alive1_flag = "6a\0";
                string log = ".\\gdut-drcom.log\0";
                GDUT_Drcom.set_enable_crypt(1);
                GDUT_Drcom.set_remote_ip(DialerConfig.AuthIP, DialerConfig.AuthIP.Length);
                GDUT_Drcom.set_keep_alive1_flag(keep_alive1_flag, keep_alive1_flag.Length);
                GDUT_Drcom.set_log_file(log, log.Length);
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
                HeartBeatThread = new Thread(() =>
                {
                    int res = GDUT_Drcom.auth();
                    HeartbeatExited?.Invoke(null, res);
                });
                HeartBeatThread.Start();
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
            if (HeartBeatThread == null)
            {
                return;
            }
            if (HeartBeatThread.IsAlive)
            {
                // todo:
                // 线程还是没有干掉啊
                //HeadBeatThread.Abort();
            }
            HeartBeatThread = null;
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
        private static Thread HeartBeatThread
        {
            set;
            get;
        }
    }
}