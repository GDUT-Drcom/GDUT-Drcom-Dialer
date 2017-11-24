using System;
using System.Threading;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    /// 简单的PING类
    /// </summary>
    internal static class SimplePing
    {
        /// <summary>
        /// Ping返回状态
        /// </summary>
        public enum Status
        {
            Success = 1,
            Timeout = 0,
            Fail = -1,
            Expection = -2
        }
        /// <summary>
        /// PING
        /// </summary>
        /// <param name="addr">要PING的地址</param>
        /// <returns></returns>
        public static Status Ping(string addr)
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(addr);

                if (reply != null)
                {
                    switch (reply.Status)
                    {
                        case IPStatus.Success:
                            return Status.Success;
                        case IPStatus.TimedOut:
                            return Status.Timeout;
                        default:
                            return Status.Fail;
                    }
                }
                return Status.Expection;
            }
            catch (Exception e)
            {
                Log4Net.WriteLog(e.Message, e);
                return Status.Expection;
            }
        }
    }

    /// <summary>
    /// 检测是否断网
    /// </summary>
    internal static class NetworkCheck
    {
        /// <summary>
        /// 最大重试次数
        /// </summary>
        public static int MaxRetry = 3;
        /// <summary>
        /// 内网断网事件
        /// </summary>
        public static EventHandler InnerNetworkCheckFailed;
        /// <summary>
        /// 外网断网事件
        /// </summary>
        public static EventHandler OuterNetworkCheckFailed;
        /// <summary>
        /// 外网又连上的事件
        /// </summary>
        public static EventHandler OuterNetworkCheckSuccessed;

        private static Task pingThread
        {
            set;
            get;
        }

        private static bool _exit = false;

        /// <summary>
        /// 检测
        /// 校内内网网址还有114,114,114,114
        /// </summary>
        /// <param name="InnerIPAddr">内网IP</param>
        /// <param name="OuterIPAddr">外网IP</param>
        private static void Check(string InnerIPAddr = "10.0.3.2", string OuterIPAddr = "119.29.29.29")
        {
            int innerRetry = 0, outerRetry = 0;
            _exit = false;
            while (!_exit)
            {
                switch (SimplePing.Ping(InnerIPAddr))
                {
                    case SimplePing.Status.Success:
                        innerRetry = 0;
                        break;
                    case SimplePing.Status.Timeout:
                    case SimplePing.Status.Fail:
                        innerRetry++;
                        break;
                    case SimplePing.Status.Expection:
                        //这就很尴尬了
                        break;
                }

                switch (SimplePing.Ping(OuterIPAddr))
                {
                    case SimplePing.Status.Success:
                        if (outerRetry > MaxRetry)
                        {
                            OuterNetworkCheckSuccessed?.Invoke(null, null);
                        }
                        outerRetry = 0;
                        break;
                    case SimplePing.Status.Timeout:
                    case SimplePing.Status.Fail:
                        outerRetry++;
                        break;
                    case SimplePing.Status.Expection:
                        //这就很尴尬了
                        break;
                }

                if (innerRetry > MaxRetry)
                {
                    //事件通知下主线程，重新拨号
                    InnerNetworkCheckFailed.Invoke(null, null);
                    return;
                }
                if (outerRetry == MaxRetry + 1)//仅仅产生一次提示事件
                {
                    //事件提示下外网断了
                    OuterNetworkCheckFailed?.Invoke(null, null);
                }

                // 休息下
                if (innerRetry == 0 || outerRetry == 0)
                    Thread.Sleep(5 * 1000);
            }
        }
        /// <summary>
        /// 循环检测
        /// </summary>
        public static void LoopCheck()
        {
            try
            {
                if (pingThread != null)
                {
                    _exit = true;
                    pingThread.Dispose();
                }
            }
            catch(Exception e)
            {
                Log4Net.WriteLog(e.Message, e);
            }
          
            pingThread = new Task(() =>
            {
                Check(DialerConfig.AuthIP);
            });
            pingThread.Start();
        }

        /// <summary>
        /// 停止ping检测
        /// </summary>
        public static void StopCheck()
        {
            if (pingThread == null)
            {
                return;
            }

            _exit = true;
            if(!pingThread.Wait(3000))
            {
                Log4Net.WriteLog("wait ping thread exit timed out");
            }
            pingThread.Dispose();
        }
    }
}
