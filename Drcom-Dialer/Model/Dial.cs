using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drcom_Dialer.Model
{
    static class Dial
    {
        public static void Auth()
        {
            string username = "/r/n" + DialerConfig.username;
            string password = DialerConfig.password;

            bool status = PPPoE.Dial(username, password);
            
            if (status)
            {
                //通常来说，他会卡在这
                //如果他返回了什么，那么一定是拨号失败了
                HeartBeatProxy.status hbStatus = HeartBeatProxy.heartbeat();
                switch (hbStatus)
                {
                    case HeartBeatProxy.status.BindPortFail:
                        Utils.Log4Net.WriteLog("绑定端口失败");
                        break;
                    case HeartBeatProxy.status.RecvTimedOut:
                        //心跳超时又有几种可能
                        //1.Heartbeat格式/校验出现的错误
                        //2.Keepalive2格式/校验出现的错误
                        //3.单纯的网络超时问题
                        //
                        //有的超时意味着PPPoE已经断开，有的却不是，这需要区分然后处理。

                        Utils.Log4Net.WriteLog("心跳超时");
                        break;
                    default:
                        break;
                }
            }
                
        }
    }
}
