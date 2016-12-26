using Drcom_Dialer.Lite;
using Drcom_Dialer.Model;
using Drcom_Dialer.Model.Utils;
using Drcom_Dialer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drcom_Dialer
{
    internal class LiteBinder : IBaseBinder
    {
        private LiteWindow window;

        public LiteBinder(LiteWindow win)
        {
            window = win;
            IsConnected = false;
            RegisterEventHandler();
            InitializeFieldFormDialerConfig();
        }

        private void RegisterEventHandler()
        {
            PPPoE.PPPoEDialSuccessEvent += (s, e) =>
            {
                Notify("拨号成功，IP: " + e.Message);
                window.dialBtn.Invoke((MethodInvoker)(() =>
                {
                    window.dialBtn.Text = "断开";
                }));
                IsConnected = true;
                DialerConfig.SaveConfig();
            };
            PPPoE.PPPoEHangupSuccessEvent += (s, e) =>
            {
                Notify("断开成功");
                window.dialBtn.Invoke((MethodInvoker)(() =>
                {
                    window.dialBtn.Text = "拨号";
                }));
                IsConnected = false;
            };
            PPPoE.PPPoEDialFailEvent += (s, e) => Notify(e.Message);
            PPPoE.PPPoEHangupFailEvent += (s, e) => Notify("断开失败");
            HeartBeatProxy.HeartbeatExited += (s, code) => Notify($"心跳停止({code})");
            NetworkCheck.InnerNetworkCheckFailed += (s, e) => Notify("InnerNetworkCheckFailed");
            NetworkCheck.OuterNetworkCheckFailed += (s, e) => Notify("OuterNetworkCheckFailed");
            NetworkCheck.OuterNetworkCheckSuccessed += (s, e) => { };
        }

        private void InitializeFieldFormDialerConfig()
        {
            if (!string.IsNullOrEmpty(DialerConfig.password))
            {
                window.paswText.Text = DialerConfig.password;
            }

            if (!string.IsNullOrEmpty(DialerConfig.username))
            {
                window.userText.Text = DialerConfig.username;
            }

            window.remPaswCheckBox.Checked = DialerConfig.isRememberPassword;

            window.autoLoginCheckBox.Checked = DialerConfig.isAutoLogin;
        }

        public bool IsConnected { private set; get; }

        public string Password
        {
            get
            {
                return DialerConfig.password;
            }

            set
            {
                DialerConfig.password = value;
            }
        }

        public string UserName
        {
            get
            {
                return DialerConfig.username;
            }

            set
            {
                DialerConfig.username = value;
            }
        }

        public bool IsRememberPassword
        {
            get
            {
                return DialerConfig.isRememberPassword;
            }

            set
            {
                DialerConfig.isRememberPassword = value;
            }
        }

        public bool IsAutoLogin
        {
            get
            {
                return DialerConfig.isAutoLogin;
            }

            set
            {
                DialerConfig.isAutoLogin = value;
            }
        }

        public void ShowBalloonTip(int timeout, string title, string text, ToolTipIcon icon)
        {
            window.notifyIcon.ShowBalloonTip(timeout, title, text, icon);
        }

        public async void DialOrHangup()
        {
            await Task.Factory.StartNew(() =>
            {
                if (IsConnected)
                {
                    Notify("正在断开");
                    Hangup();
                }
                else
                {
                    Notify("正在拨号");
                    Dial();
                }
            });
        }

        private void Hangup()
        {
            Authenticator.Deauthenticate();
        }

        private void Dial()
        {
            Authenticator.Authenticate();
        }

        private void Notify(string str)
        {
            window.statusLabel.Text = str;
        }

        public void ShowStatus(string status)
        {
            Notify(status);
        }
    }
}
