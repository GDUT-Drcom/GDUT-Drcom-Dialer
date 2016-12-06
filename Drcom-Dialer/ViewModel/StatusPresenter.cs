using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drcom_Dialer.ViewModel
{
    public class StatusPresenterModel : NotifyProperty
    {
        public string Status
        {
            set
            {
                UpdateProper(ref _status, value);
            }
            get
            {
                return _status;
            }
        }

        private string _status = "点击关于可查看免责声明";
    }
}