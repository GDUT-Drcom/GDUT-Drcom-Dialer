using Drcom_Dialer.Model;
using Drcom_Dialer.Model.Utils;
using Drcom_Dialer.ViewModel;
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
            Binder.BaseBinder = new LiteBinder(this);
        }

        private void dialBtn_Click(object sender, EventArgs e)
        {
            Binder.BaseBinder.DialOrHangup();
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
        }

        private void autoLoginCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DialerConfig.isAutoLogin = autoLoginCheckBox.Checked;
        }

        private void userText_TextChanged(object sender, EventArgs e)
        {
            DialerConfig.username = userText.Text;
        }

        private void paswText_TextChanged(object sender, EventArgs e)
        {
            DialerConfig.password = paswText.Text;
        }

        private void viewPaswBtn_Click(object sender, EventArgs e)
        {
            if (paswText.PasswordChar == '*')
                paswText.PasswordChar = '\0';
            else
                paswText.PasswordChar = '*';
        }
    }
}
