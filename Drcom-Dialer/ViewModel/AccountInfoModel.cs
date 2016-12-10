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
                var info = AccountStatus.GetAccountInfomation();
                if (info.Status == "success")
                {
                    Username = info.Username;
                    Money = info.LeftMoney;
                    Overdata = info.OverDate.ToString("yyyy-MM-dd");
                }
            }).Start();
        }
    }
}