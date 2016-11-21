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
            cb_startup.IsChecked = Model.DialerConfig.isRunOnStartup;
            cb_redial.IsChecked = Model.DialerConfig.isReDialOnFail;
            cb_vpnFix.IsChecked = Model.DialerConfig.isFixVPN;
            comboBox.SelectedIndex = (int)Model.DialerConfig.zone;

            comboBox.SelectionChanged += new SelectionChangedEventHandler(comboBox_SelectionChanged);
        }

        private void cb_startup_Checked(object sender, RoutedEventArgs e)
        {
            Model.DialerConfig.isRunOnStartup = (bool)cb_startup.IsChecked;
        }

        private void cb_redial_Checked(object sender, RoutedEventArgs e)
        {
            Model.DialerConfig.isReDialOnFail = (bool)cb_redial.IsChecked;
        }

        private void cb_vpnFix_Checked(object sender, RoutedEventArgs e)
        {
            Model.DialerConfig.isFixVPN = (bool)cb_vpnFix.IsChecked;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.DialerConfig.zone = (Model.DialerConfig.Campus)comboBox.SelectedIndex;
        }
    }
}
