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
                if (info.date == "success")
                {
                    var temp = info.note;
                    Username = temp.welcome;
                    Money = temp.leftmoeny;
                    ////到期时间有 "；到期时间" 于是去掉";"
                    Overdata = ConvertOverdataStrToDate(temp.overdate); //temp.overdate.Replace("；","").Replace(";","");
                }
            }).Start();
        }

        private static string ConvertOverdataStrToDate(string overdata)
        {
            //overdata = "；到期时间 2017.6.20";
            int n = 20;//时间开头是  201*年x月
            n = overdata.IndexOf(n.ToString());
            //第一个 20 
            //最后一个可能 20日  2017.1.20 不能拿最后一个
            //Console.WriteLine(overdata.Substring(n));
            return overdata.Substring(n);
        }
    }
}