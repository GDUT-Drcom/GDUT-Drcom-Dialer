using System;
using System.Net.NetworkInformation;

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
        /// 检测
        /// 校内内网网址还有114,114,114,114
        /// </summary>
        /// <param name="InnerIPAddr">内网IP</param>
        /// <param name="OuterIPAddr">外网IP</param>
        public static void Check(string InnerIPAddr = "10.0.3.2",string OuterIPAddr = "119.29.29.29")
        {
            int innerRetry = 0,outerRetry = 0;

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
                    if(outerRetry > MaxRetry)
                    {
                        //todo: 通知外部连上了
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

            if(innerRetry> MaxRetry)
            {
                //事件通知下主线程，重新拨号
                return;
            }
            if (outerRetry == MaxRetry + 1)//仅仅产生一次提示事件
            {
                //事件提示下外网断了
                //不要停止检测

            }

        }
        //这需要个事件
    }
}
