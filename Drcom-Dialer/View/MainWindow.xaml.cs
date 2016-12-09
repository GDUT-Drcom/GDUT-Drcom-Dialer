using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            View = (ViewModel.ViewModel) DataContext;
            InitTrayIcon();
        }

        /// <summary>
        ///     显示气泡
        ///     需要弄成MVVM
        /// </summary>
        /// <param name="timeout">消失时间（毫秒）</param>
        /// <param name="title">标题</param>
        /// <param name="text">内容</param>
        /// <param name="icon">图标</param>
        public static void ShowBalloonTip(int timeout, string title, string text, ToolTipIcon icon = ToolTipIcon.Info)
        {
            _trayIcon.ShowBalloonTip(timeout, title, text, icon);
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
            if (View.DialOrHangup)
                View.Dial();
            else
                View.Hangup();
        }

        /// <summary>
        ///     自动登录点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_autoLogin_Click(object sender, RoutedEventArgs e)
        {
            if ((cb_autoLogin.IsChecked != null) && (cb_remember.IsChecked != null) && !(bool) cb_remember.IsChecked &&
                (bool) cb_autoLogin.IsChecked)
            {
                cb_remember.IsChecked = true;
                cb_remember_Click(null, null);
            }

            if (cb_autoLogin.IsChecked != null)
            {
                DialerConfig.isAutoLogin = (bool) cb_autoLogin.IsChecked;
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
                !(bool) cb_remember.IsChecked &&
                (bool) cb_autoLogin.IsChecked)
            {
                cb_autoLogin.IsChecked = false;
                cb_autoLogin_Click(null, null);
            }

            if (cb_remember.IsChecked != null)
            {
                DialerConfig.isRememberPassword = (bool) cb_remember.IsChecked;
            }
        }

        ///// <summary>
        /////     密码输入处理 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void pb_password_PasswordChanged(object sender, RoutedEventArgs e)
        //{
        //    DialerConfig.password = pb_password.Password;
        //}

        ///// <summary>
        /////     账号输入处理
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void tb_username_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    DialerConfig.username = tb_username.Text;
        //}

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
            //TODO:add code here 注销
            PPPoE.Hangup();
            //hangup
        }

        /// <summary>
        ///     关闭事件
        ///     处理最后一步的工作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            _trayIcon.Visible = false;
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
            _trayIcon = new NotifyIcon
            {
                Text = Properties.Resources.ProgramTitle,
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath)
            };

            _trayIcon.MouseClick += (obj, e) =>
            {
                if ((e.Button == MouseButtons.Left) && (WindowState == WindowState.Minimized))
                {
                    Show();
                    Activate();
                    WindowState = WindowState.Normal;
                }
                //todo:关闭显示
                // trayIcon.Visible = false;
            };
            _trayIcon.Visible = true;
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

        private static NotifyIcon _trayIcon;
    }
}