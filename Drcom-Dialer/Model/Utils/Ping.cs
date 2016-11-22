using System;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    /// 简单的PING类
    /// </summary>
    static class SimplePing
    {
        /// <summary>
        /// Ping返回状态
        /// </summary>
        public enum status
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
        public static status ping(String addr)
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(addr);

                switch (reply.Status)
                {
                    case IPStatus.Success:
                        return status.Success;
                    case IPStatus.TimedOut:
                        return status.Timeout;
                    default:
                        return status.Fail;
                }
            }
            catch(Exception e)
            {
                Log4Net.WriteLog(e.Message, e);
                return status.Expection;
            }

        }
    }

    /// <summary>
    /// 检测是否断网
    /// </summary>
    static class NetworkCheck
    {
        /// <summary>
        /// 最大重试次数
        /// </summary>
        public static int MaxRetry = 3;

        /// <summary>
        /// 检测
        /// </summary>
        public static void Check()
        {
            //校内内网网址还有114,114,114,114
            Check("10.0.3.2", "119.29.29.29");
        }
        /// <summary>
        /// 检测
        /// </summary>
        /// <param name="InnerIPAddr">内网IP</param>
        /// <param name="OuterIPAddr">外网IP</param>
        public static void Check(string InnerIPAddr,string OuterIPAddr)
        {
            int _innerRetry = 0,_outerRetry = 0;

            switch (SimplePing.ping(InnerIPAddr))
            {
                case SimplePing.status.Success:
                    _innerRetry = 0;
                    break;
                case SimplePing.status.Timeout:
                case SimplePing.status.Fail:
                    _innerRetry++;
                    break;
                case SimplePing.status.Expection:
                    //这就很尴尬了
                    break;
            }

            switch (SimplePing.ping(OuterIPAddr))
            {
                case SimplePing.status.Success:
                    if(_outerRetry > MaxRetry)
                        //同志们我又连上了
                        //下面这句话是占位符
                        _outerRetry = 0;
                    _outerRetry = 0;
                    break;
                case SimplePing.status.Timeout:
                case SimplePing.status.Fail:
                    _outerRetry++;
                    break;
                case SimplePing.status.Expection:
                    //这就很尴尬了
                    break;
            }

            if(_innerRetry> MaxRetry)
            {
                //事件通知下主线程，重新拨号
                return;
            }
            if (_outerRetry == MaxRetry + 1)//仅仅产生一次提示事件
            {
                //事件提示下外网断了
                //不要停止检测

            }

        }
        //这需要个事件
    }
}
