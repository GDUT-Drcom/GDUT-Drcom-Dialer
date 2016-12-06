using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drcom_Dialer.Model;

namespace Drcom_Dialer.ViewModel
{
    public class ViewModel : NotifyProperty
    {
        public ViewModel()
        {
            View = this;

            //初始化

            NewStatusPresenterModel();

            if (!string.IsNullOrEmpty(DialerConfig.password))
            {
                Password = DialerConfig.password;
            }

            if (!string.IsNullOrEmpty(DialerConfig.username))
            {
                UserName = DialerConfig.username;
            }

            IsRememberPassword = DialerConfig.isRememberPassword;

            IsAutoLogin = DialerConfig.isAutoLogin;
        }

        public string Password
        {
            set
            {
                _password = value;
                OnPropertyChanged();
            }
            get
            {
                return _password;
            }
        }

        public string UserName
        {
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
            get
            {
                return _userName;
            }
        }

        public bool IsRememberPassword
        {
            set
            {
                _isRememberPassword = value;
                OnPropertyChanged();
            }
            get
            {
                return _isRememberPassword;
            }
        }

        public bool IsAutoLogin
        {
            set
            {
                _isAutoLogin = value;
                OnPropertyChanged();
            }
            get
            {
                return _isAutoLogin;
            }
        }

        public StatusPresenterModel StatusPresenterModel
        {
            set
            {
                _statusPresenterModel = value;
                OnPropertyChanged();
            }
            get
            {
                return _statusPresenterModel;
            }
        }

        /// <summary>
        ///     抽象
        /// </summary>
        public static ViewModel View
        {
            set;
            get;
        }

        public bool Enable
        {
            set
            {
                _enable = value;
                OnPropertyChanged();
            }
            get
            {
                return _enable;
            }
        }

        /// <summary>
        ///     拨号
        /// </summary>
        public void Dial()
        {
            // 不想写Command

            if (string.IsNullOrEmpty(UserName))
            {
                Notify("请输入账户");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                Notify("请输入密码");
                return;
            }

            DialerConfig.password = Password;
            DialerConfig.username = UserName;

            //开始拨号
            Notify("开始拨号");

            Enable = false;

            new Task(() =>
            {
                try
                {
                    //后台保存
                    DialerConfig.SaveConfig();
                    Model.Dial.Auth();
                }
                catch (Exception e)
                {
                    Notify(e.Message);
                    Model.Utils.Log4Net.WriteLog(e.Message, e);
                }
                Enable = true;
            }).Start();
        }
        
        private bool _enable;

        private string _userName;

        private string _password;

        private bool _isRememberPassword;

        private bool _isAutoLogin;
        
        private StatusPresenterModel _statusPresenterModel;

        /// <summary>
        ///     通知
        /// </summary>
        /// <param name="str"></param>
        private void Notify(string str)
        {
            StatusPresenterModel.Status = str;
        }

        private void NewStatusPresenterModel()
        {
            StatusPresenterModel = new StatusPresenterModel();
            PPPoE.PPPoEDialFailEvent += (s, e) =>
            {
                StatusPresenterModel.Status = e.Message;
            };
            PPPoE.PPPoEDialSuccessEvent += (s, e) =>
            {
                StatusPresenterModel.Status = "拨号成功，IP: " + e.Message;
            };
            PPPoE.PPPoEHangupSuccessEvent += (s, e) =>
            {
                StatusPresenterModel.Status = "拨号已断开";
            };
            HeartBeatProxy.HeartbeatExited += (s, code) =>
            {
                StatusPresenterModel.Status = $"心跳终止({code})";
            };
        }
    }
}