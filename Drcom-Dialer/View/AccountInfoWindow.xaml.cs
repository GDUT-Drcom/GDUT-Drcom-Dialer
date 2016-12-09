using Drcom_Dialer.Model.Utils;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Drcom_Dialer.Model.Utils.AccountStatus;

namespace Drcom_Dialer.View
{
    /// <summary>
    /// Interaction logic for AccountInfoWindow.xaml
    /// </summary>
    public partial class AccountInfoWindow : MetroWindow
    {
        public AccountInfoWindow()
        {
            InitializeComponent();

            ReadInfo();
        }

        private void ReadInfo()
        {
            try
            {
                AccountInfomation info = GetAccountInfomation();
                if(info.date == "success")
                {
                    lbl_user.Content = info.note.welcome;
                    lbl_leftmoeny.Content = info.note.leftmoeny;
                    lbl_overdate.Content = info.note.overdate;
                }
                else
                {
                    lbl_user.Content = "N/A";
                    lbl_leftmoeny.Content = "N/A";
                    lbl_overdate.Content = "N/A";
                }
            }
            catch(Exception e)
            {
                Log4Net.WriteLog(e.Message, e);
            }
        }
    }
}
