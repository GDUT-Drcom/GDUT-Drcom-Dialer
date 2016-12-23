using Drcom_Dialer.Model;
using Drcom_Dialer.Model.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drcom_Dialer.Lite
{
    public partial class LiteWindow : Form
    {
        public LiteWindow()
        {
            InitializeComponent();

            RegisterEventHandler();
            InitializeFieldFormDialerConfig();
        }

        private void RegisterEventHandler()
        {
            PPPoE.PPPoEDialSuccessEvent += (s, e) =>
            {
                statusLabel.Text = "拨号成功";
                dialBtn.Invoke((MethodInvoker)(() => { Text = "断开"; }));
                DialerConfig.SaveConfig();
            };
            PPPoE.PPPoEHangupSuccessEvent += (s, e) =>
            {
                statusLabel.Text = "断开成功";
                dialBtn.Invoke((MethodInvoker)(() => { Text = "拨号"; }));
            };
            PPPoE.PPPoEDialFailEvent += (s, e) =>
            {
                statusLabel.Text = "拨号失败";
            };
            PPPoE.PPPoEHangupFailEvent += (s, e) =>
            {
                statusLabel.Text = "断开失败";
            };
            HeartBeatProxy.HeartbeatExited += (s, code) =>
            {
                statusLabel.Text = $"心跳停止({code})";
            };
            NetworkCheck.InnerNetworkCheckFailed += (s, e) => { };
            NetworkCheck.OuterNetworkCheckFailed += (s, e) => { };
            NetworkCheck.OuterNetworkCheckSuccessed += (s, e) => { };
        }

        private void InitializeFieldFormDialerConfig()
        {
            if (!string.IsNullOrEmpty(DialerConfig.password))
            {
                paswText.Text = DialerConfig.password;
            }

            if (!string.IsNullOrEmpty(DialerConfig.username))
            {
                userText.Text = DialerConfig.username;
            }

            remPaswCheckBox.Checked = DialerConfig.isRememberPassword;

            autoLoginCheckBox.Checked = DialerConfig.isAutoLogin;
        }

        private async void dialBtn_Click(object sender, EventArgs e)
        {
            await Task.Factory.StartNew(() =>
            {
                PPPoE.Dial("\r\n" + userText.Text, paswText.Text);
            });
        }

        private void settingBtn_Click(object sender, EventArgs e)
        {
            new SettingWindow().ShowDialog();
        }

        private void aboutBtn_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void remPaswCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DialerConfig.isRememberPassword = remPaswCheckBox.Checked;
            DialerConfig.SaveConfig();
        }

        private void autoLoginCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DialerConfig.isAutoLogin = autoLoginCheckBox.Checked;
            DialerConfig.SaveConfig();
        }

        private void viewPaswBtn_Click(object sender, EventArgs e)
        {
            if (paswText.PasswordChar == '*')
                paswText.PasswordChar = '\0';
            else
                paswText.PasswordChar = '*';
        }

        private void userText_TextChanged(object sender, EventArgs e)
        {
            DialerConfig.username = userText.Text;
        }

        private void paswText_TextChanged(object sender, EventArgs e)
        {
            DialerConfig.password = paswText.Text;
        }
    }
}
