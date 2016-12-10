using System;
using Drcom_Dialer.Model.Utils;
using Drcom_Dialer.ViewModel;
using static Drcom_Dialer.Model.Utils.AccountStatus;
using MahApps.Metro.Controls;

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

            DataContext = new AccountInfoModel();
            // ReadInfo();
        }

        //private void ReadInfo()
        //{
        //    try
        //    {
        //        AccountInfomation info = GetAccountInfomation();
        //        if(info.date == "success")
        //        {
        //            lbl_user.Content = info.note.welcome;
        //            lbl_leftmoeny.Content = info.note.leftmoeny;
        //            lbl_overdate.Content = info.note.overdate;
        //        }
        //        else
        //        {
        //            lbl_user.Content = "N/A";
        //            lbl_leftmoeny.Content = "N/A";
        //            lbl_overdate.Content = "N/A";
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        Log4Net.WriteLog(e.Message, e);
        //    }
        //}
    }
}
