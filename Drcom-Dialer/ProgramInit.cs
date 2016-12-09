using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Drcom_Dialer
{
    internal static class ProgramInit
    {
        private static Mutex singleInstanceWatcher;
        private static bool createdNew;

        [STAThread]
        private static void Main(string[] args)
        {
            Update();

            string mutexName = Drcom_Dialer.Properties.Resources.ProgramTitle + "Mutex";
            singleInstanceWatcher = new Mutex(false, mutexName, out createdNew);
            if (!createdNew)
            {
                MessageBox.Show("程序已经运行!", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }

            //进行必要的初始化工作
            Model.Utils.GDUT_Drcom.Load();
            Model.Utils.Log4Net.SetConfig();
            Model.DialerConfig.Init();
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case Model.Utils.VPNFixer.StartupArgs:
                         Model.Utils.VPNFixer.AddRouteRule();
                         return;
                    default:
                        Model.Utils.Log4Net.WriteLog("未知的启动参数: " + args);
                        break;
                }
            }

            if (Model.DialerConfig.isFixVPN)
            {
                Model.Utils.VPNFixer.Elevate();
            }

            Model.PPPoE.Init();
            Model.Dial.Init();

            App app = new App();
            app.InitializeComponent();
            app.Run();
        }

        private static void Update()
        {
            if (AppDomain.CurrentDomain.FriendlyName.EndsWith(Model.Utils.Updater.UpdateSuffix))
            {
                string tar = Model.Utils.Updater.ExeName;
                tar = tar.Substring(0, tar.Length - 4);
                Process[] ps;
                do
                {
                    ps = Process.GetProcessesByName(tar);
                } while (ps.Length > 0);

                File.Delete(Model.Utils.Updater.ExeName);
                File.Copy(Model.Utils.Updater.UpdateName, Model.Utils.Updater.ExeName);
                Process proc = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = Model.Utils.Updater.ExeName
                    }
                };
                proc.Start();
                return;
            }

            if (File.Exists(Model.Utils.Updater.UpdateName))
                File.Delete(Model.Utils.Updater.UpdateName);
        }
    }
}
