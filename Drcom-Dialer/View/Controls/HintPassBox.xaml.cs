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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Drcom_Dialer.View.Controls
{
    /// <summary>
    /// HintPassBox.xaml 的交互逻辑
    /// </summary>
    public partial class HintPassBox : UserControl
    {
        public HintPassBox()
        {
            InitializeComponent();
        }

        private string _watermark;

        public string Watermark
        {
            set
            {
                _watermark = value;
                MahApps.Metro.Controls.TextBoxHelper.SetWatermark(pb_password, _watermark);
            }
            get
            {
                return _watermark;
            }
        }
        public class password : DependencyObject
        {
            public static readonly DependencyProperty PasswordProperty =
    DependencyProperty.Register("Password", typeof(string), typeof(HintPassBox), new FrameworkPropertyMetadata(false));

            public string Password
            {
                get { return (String)GetValue(PasswordProperty); }
                set { SetValue(PasswordProperty, value); }
            }
        }

        private void btn_see_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //tb_text.Text = Password;
            btn_see.Source = new BitmapImage(new Uri("/Drcom-Dialer;component/Resource/ShowDark.ico", UriKind.Relative));
            tb_text.Visibility = Visibility.Visible;
        }

        private void btn_see_MouseUp(object sender, MouseButtonEventArgs e)
        {
            tb_text.Visibility = Visibility.Hidden;
            btn_see.Source = new BitmapImage(new Uri("/Drcom-Dialer;component/Resource/Show.ico", UriKind.Relative));
            tb_text.Text = "";
        }

        private void btn_see_MouseLeave(object sender, MouseEventArgs e)
        {
            btn_see_MouseUp(null, null);
        }
    }
}
