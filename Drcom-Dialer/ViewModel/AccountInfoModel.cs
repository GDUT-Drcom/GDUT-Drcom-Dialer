using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drcom_Dialer.Model.Utils;

namespace Drcom_Dialer.ViewModel
{
    public class AccountInfoModel : NotifyProperty
    {
        public AccountInfoModel()
        {
            const string na = "N/A";
            Username = na;
            Money = na;
            Overdata = na;
            GetAccountInfomation();
        }

        public string Username
        {
            set
            {
                _username = value;
                OnPropertyChanged();
            }
            get
            {
                return _username;
            }
        }

        public string Money
        {
            set
            {
                _money = value;
                OnPropertyChanged();
            }
            get
            {
                return _money;
            }
        }

        public string Overdata
        {
            set
            {
                _overdata = value;
                OnPropertyChanged();
            }
            get
            {
                return _overdata;
            }
        }

        private string _money;

        private string _overdata;

        private string _username;

        private void GetAccountInfomation()
        {
            new Task(() =>
            {
                if (AccountStatus.AccInfo == null)
                    AccountStatus.AccInfo = AccountStatus.GetAccountInfomation();
                if (AccountStatus.AccInfo.Status == "success")
                {
                    Username = AccountStatus.AccInfo.Username;
                    Money = AccountStatus.AccInfo.LeftMoney;
                    Overdata = AccountStatus.AccInfo.OverDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    Username = AccountStatus.AccInfo.Status;
                }
            }).Start();
        }
    }
}