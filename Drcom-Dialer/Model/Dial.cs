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
            PPPoE.PPPoEDialSuccessEvent += OnPPPoESuccess;
            PPPoE.PPPoEDialFailEvent += OnPPPoEFail;
            PPPoE.PPPoEHangupSuccessEvent += OnPPPoEHangup;
        }
        /// <summary>
        /// 自动拨号
        /// </summary>
        public static void Auth()
        {
            string username = "\r\n" + DialerConfig.username;
            string password = DialerConfig.password;
            PPPoE.Dial(username, password);
        }
        /// <summary>
        /// 拨号成功
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void OnPPPoESuccess(object obj,Msg e)
        {
            //检测DLL更新
            if (!Utils.HeartBeatUpdate.CheckDLL() || Utils.HeartBeatUpdate.CheckUpdate())
                Utils.HeartBeatUpdate.Update();

            if (Utils.HeartBeatUpdate.CheckDLL())
            {
                //获取账户信息
                Utils.AccountStatus.AccountInfo();

                if (HeartBeatProxy.Init() != HeartBeatProxy.HeadBeatStatus.Success)
                    Utils.Log4Net.WriteLog("初始化心跳失败");
                HeartBeatProxy.HeadBeatStatus stat = HeartBeatProxy.Heartbeat();

                //发送反馈
                Utils.BmobAnalyze.SendAnalyze();
            }
            else
            {
                //在此报错！
            }

        }
        /// <summary>
        /// 拨号错误
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void OnPPPoEFail(object obj,Msg e)
        {
            //PPPoESuccessEventHandler(obj, e);
        }
        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void OnPPPoEHangup(object obj,EventArgs e)
        {
            HeartBeatProxy.Kill();
        }
    }
}
