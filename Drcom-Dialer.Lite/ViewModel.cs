using Drcom_Dialer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drcom_Dialer
{
    internal class LiteBinder : IModelBinder
    {
        public bool IsConnected
        {
            get
            {
                return false;
            }
        }

        public void ShowBalloonTip(int timeout, string title, string text, ToolTipIcon icon)
        {

        }
    }
}
