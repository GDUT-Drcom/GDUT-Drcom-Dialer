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
            return status.Success;
        }
        /// <summary>
        /// 进行心跳操作
        /// </summary>
        public static status heartbeat()
        {
            return status.Success;
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
