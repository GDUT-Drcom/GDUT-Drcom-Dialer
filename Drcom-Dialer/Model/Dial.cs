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
        private static void OnPPPoESuccess(object obj, Msg e)
        {
            switch (Utils.HeartBeatUpdate.TryUpdate())
            {
                case Utils.Updater.UpdateState.Failed:
                    Utils.Log4Net.WriteLog("更新失败");
                    break;
                default:
                    Utils.Log4Net.WriteLog("更新成功");
                    break;
            }

            if (Utils.HeartBeatUpdate.CheckDLL())
            {
                //获取账户信息
                Utils.AccountStatus.AccountInfo();

                if (HeartBeatProxy.Init() != HeartBeatProxy.HeadBeatStatus.Success)
                {
                    Utils.Log4Net.WriteLog("初始化心跳失败");
                }
                else
                {
                    HeartBeatProxy.HeadBeatStatus stat = HeartBeatProxy.Heartbeat();
                }

                //发送反馈
                Utils.BmobAnalyze.SendAnalyze();
            }
            else
            {
                Utils.Log4Net.WriteLog("心跳DLL缺失且更新失败");
                //ViewModel.ViewModel.View.StatusPresenterModel.Status = "心跳DLL缺失/更新失败";
            }
        }

        /// <summary>
        /// 拨号错误
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void OnPPPoEFail(object obj, Msg e)
        {
            //OnPPPoESuccess(obj, e);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private static void OnPPPoEHangup(object obj, EventArgs e)
        {
            //HeartBeatProxy.Kill();
        }
    }
}
