using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drcom_Dialer.Model
{
    static class Dial
    {
        public static void Init()
        {
            PPPoE.PPPoEDialSuccessEvent += new EventHandler<Msg>(PPPoESuccessEventHandler);
            PPPoE.PPPoEDialFailEvent += new EventHandler<Msg>(PPPoEFailEventHandler);
            PPPoE.PPPoEHangupSuccessEvent += new EventHandler(PPPoEHangupEventHandler);
        }

        public static void Auth()
        {
            string username = "\r\n" + DialerConfig.username;
            string password = DialerConfig.password;
            PPPoE.Dial(username, password);
  
        }

        private static void PPPoESuccessEventHandler(object obj,Msg e)
        {
            //TODO:IP地址的显示
            if (HeartBeatProxy.init() != HeartBeatProxy.status.Success)
                Utils.Log4Net.WriteLog("初始化心跳失败");
            HeartBeatProxy.status stat = HeartBeatProxy.heartbeat();

            switch (stat)
            {
                case HeartBeatProxy.status.BindPortFail:
                    Utils.Log4Net.WriteLog("绑定端口失败");
                    break;
                default:
                    break;
            }
        }

        private static void PPPoEFailEventHandler(object obj,Msg e)
        {
            //PPPoESuccessEventHandler(obj, e);
        }

        private static void PPPoEHangupEventHandler(object obj,EventArgs e)
        {

        }
    }
}
