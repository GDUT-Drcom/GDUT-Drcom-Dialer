using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Drcom_Dialer.ViewModel
{
    interface IBaseBinder
    {
        void ShowBalloonTip(int timeout, string title, string text, ToolTipIcon icon);
        bool IsConnected { get; }

        void DialOrHangup();

        string Password { set; get; }
        string UserName { set; get; }
        bool IsRememberPassword { set; get; }
        bool IsAutoLogin { set; get; }
    }

    internal class Binder
    {
        public static IBaseBinder BaseBinder { set; get; }
    }
}
