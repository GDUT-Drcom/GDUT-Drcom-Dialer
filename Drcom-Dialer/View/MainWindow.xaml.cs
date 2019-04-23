using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Drcom_Dialer.Model;
// To access MetroWindow, add the following reference
using MahApps.Metro.Controls;
using Application = System.Windows.Forms.Application;
using Drcom_Dialer.ViewModel;

namespace Drcom_Dialer.View
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            View = (ViewModel.ViewModel)DataContext;
            InitTrayIcon();

            // load password from VM
            Password = View.Password;
        }

        private ViewModel.ViewModel View
        {
            set;
            get;
        }

        /// <summary>
        ///     拨号按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_dial_Click(object sender, RoutedEventArgs e)
        {
            View.Password = Password;
            View.DialOrHangup();
        }

        /// <summary>
        ///     输入框中保留的密码
        /// </summary>
        public string Password
        {
            get
            {
                return pb_password.Password;
            }
            set
            {
                pb_password.Password = value;
            }
        }

        /// <summary>
        ///     自动登录点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_autoLogin_Click(object sender, RoutedEventArgs e)
        {
            if ((cb_autoLogin.IsChecked != null) && (cb_remember.IsChecked != null) && !(bool)cb_remember.IsChecked &&
                (bool)cb_autoLogin.IsChecked)
            {
                cb_remember.IsChecked = true;
                cb_remember_Click(null, null);
            }

            if (cb_autoLogin.IsChecked != null)
            {
                DialerConfig.isAutoLogin = (bool)cb_autoLogin.IsChecked;
            }
        }

        /// <summary>
        ///     记住密码点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_remember_Click(object sender, RoutedEventArgs e)
        {
            if ((cb_autoLogin.IsChecked != null) &&
                (cb_remember.IsChecked != null) &&
                !(bool)cb_remember.IsChecked &&
                (bool)cb_autoLogin.IsChecked)
            {
                cb_autoLogin.IsChecked = false;
                cb_autoLogin_Click(null, null);
            }

            if (cb_remember.IsChecked != null)
            {
                DialerConfig.isRememberPassword = (bool)cb_remember.IsChecked;
            }
        }

        /// <summary>
        ///     关于按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_Button_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }

        /// <summary>
        ///     将要关闭的事件
        ///     处理关闭前的工作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Closing(object sender, CancelEventArgs e)
        {
            View.Hangup();
            DialerConfig.SaveConfig();
            Model.Utils.DialerUpdater.StopCheckUpdateTimer();
        }

        /// <summary>
        ///     关闭事件
        ///     处理最后一步的工作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            View.TrayIcon.Visible = false;
        }

        /// <summary>
        ///     窗口位置改变事件
        ///     处理最小化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_StateChanged(object sender, EventArgs e)
        {
            //最小化到托盘
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
        }

        /// <summary>
        ///     初始化托盘图标
        /// </summary>
        private void InitTrayIcon()
        {
            View.TrayIcon.MouseClick += (obj, e) =>
            {
                if ((e.Button == MouseButtons.Left) &&
                (WindowState == WindowState.Minimized))
                {
                    Show();
                    Activate();
                    WindowState = WindowState.Normal;
                }
            };
        }

        /// <summary>
        ///     设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Setting_Button_Click(object sender, RoutedEventArgs e)
        {
            //todo:建议为窗体级
            SettingWindow setting = new SettingWindow();
            setting.ShowDialog();
        }

        private void AccountInfo_Button_Click(object sender, RoutedEventArgs e)
        {
            AccountInfoWindow accountInfo = new AccountInfoWindow();
            accountInfo.ShowDialog();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DialerConfig.isAutoLogin)
                View.DialOrHangup();
        }

        /// <summary>
        ///     显示密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void showPassword(object sender, RoutedEventArgs e)
        {
            View.Password = Password;
            pb_passhint.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 隐藏密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void hidePassword(object sender, RoutedEventArgs e)
        {
            Password = View.Password;
            pb_passhint.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// listen change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pb_password_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            View.Password = Password;
        }
    }
}