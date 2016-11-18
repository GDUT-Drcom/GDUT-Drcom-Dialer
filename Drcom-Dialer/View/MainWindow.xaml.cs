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
// To access MetroWindow, add the following reference
using MahApps.Metro.Controls;

namespace Drcom_Dialer.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// 拨号器配置
        /// </summary>
        Model.DialerConfig DialerCfg = new Model.DialerConfig();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_dial_Click(object sender, RoutedEventArgs e)
        {
            //TODO:add code here
            DialerCfg.SaveConfig();
        }
        /// <summary>
        /// 自动登录点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_autoLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)cb_remember.IsChecked && (bool)cb_autoLogin.IsChecked)
            {
                cb_remember.IsChecked = true;
                cb_remember_Click(null, null);
            }
                
            DialerCfg.isAutoLogin = (bool)cb_autoLogin.IsChecked;
        }
        /// <summary>
        /// 记住密码点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_remember_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)cb_remember.IsChecked && (bool)cb_autoLogin.IsChecked)
            {
                cb_autoLogin.IsChecked = false;
                cb_autoLogin_Click(null,null);
            }
            DialerCfg.isRememberPassword = (bool)cb_remember.IsChecked;
        }

        /// <summary>
        /// 密码输入处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pb_password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            DialerCfg.password = pb_password.Password;
        }

        /// <summary>
        /// 账号输入处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_username_TextChanged(object sender, TextChangedEventArgs e)
        {
            DialerCfg.username = tb_username.Text;
        }

        /// <summary>
        /// 关于按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }
    }
}
