using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Drcom_Dialer.ViewModel
{
    interface IModelBinder
    {
        void ShowBalloonTip(int timeout, string title, string text, ToolTipIcon icon);
        bool IsConnected { get; }
    }

    internal class Binder
    {
        public static IModelBinder ModelBinder { set; get; }
    }
}
