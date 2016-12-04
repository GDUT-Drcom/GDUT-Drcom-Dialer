using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drcom_Dialer.ViewModel
{
    public class StatusPresenter : NotifyProperty
    {
        private string status = "点击关于可查看免责声明";
        public string Status
        {
            get { return status; }
            set
            {
                UpdateProper(ref status, value, nameof(Status));
            }
        }
    }
}
