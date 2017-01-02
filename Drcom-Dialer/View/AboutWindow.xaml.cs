using System;
using System.Diagnostics;
using System.Windows.Navigation;
// To access MetroWindow, add the following reference
using MahApps.Metro.Controls;
namespace Drcom_Dialer.View
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : MetroWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
            Version.Content = "Dr.COM三方客户端广工大专版 " + Model.Utils.Version.GetVersion();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Hyperlink_File(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo("file:\\\\"+Environment.CurrentDirectory + "\\"+ e.Uri));
            e.Handled = true;
        }
    }
}
