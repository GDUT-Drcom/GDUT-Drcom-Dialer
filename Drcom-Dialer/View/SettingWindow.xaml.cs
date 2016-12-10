using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace Drcom_Dialer.View
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : MetroWindow
    {
        public SettingWindow()
        {
            InitializeComponent();
            StartupCheckBox.IsChecked = Model.DialerConfig.isRunOnStartup;
            RedialCheckBox.IsChecked = Model.DialerConfig.isReDialOnFail;
            VpnFixCheckBox.IsChecked = Model.DialerConfig.isFixVPN;
            ExpireNotifyCheckBox.IsChecked = Model.DialerConfig.isNotifyWhenExpire;
            ReportCheckBox.IsChecked = Model.DialerConfig.isFeedback;
            CampusComboBox.SelectedIndex = (int)Model.DialerConfig.zone;

            CampusComboBox.SelectionChanged += CampusComboBox_SelectionChanged;
        }

        private void StartupCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (StartupCheckBox.IsChecked != null)
            {
                Model.DialerConfig.isRunOnStartup = (bool)StartupCheckBox.IsChecked;
                Model.Utils.RunOnStartup.SetStartup(Model.DialerConfig.isRunOnStartup);
            }
        }

        private void cb_redial_Checked(object sender, RoutedEventArgs e)
        {
            if (RedialCheckBox.IsChecked != null)
            {
                Model.DialerConfig.isReDialOnFail = (bool)RedialCheckBox.IsChecked;
            }
        }

        private void cb_vpnFix_Checked(object sender, RoutedEventArgs e)
        {
            if (VpnFixCheckBox.IsChecked != null)
            {
                Model.DialerConfig.isFixVPN = (bool)VpnFixCheckBox.IsChecked;
            }
        }

        private void CampusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.DialerConfig.zone = (Model.DialerConfig.Campus)CampusComboBox.SelectedIndex;
        }

        private void ExpireNotifyCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ExpireNotifyCheckBox.IsChecked != null)
            {
                Model.DialerConfig.isNotifyWhenExpire = (bool)ExpireNotifyCheckBox.IsChecked;
            }
        }

        private void ReportCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ReportCheckBox.IsChecked != null)
            {
                Model.DialerConfig.isFeedback = (bool)ReportCheckBox.IsChecked;
            }
        }
    }
}
