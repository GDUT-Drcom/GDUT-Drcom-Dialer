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
            //更新
            if (Update())
                return;

            //防止多启
            string mutexName = Properties.Resources.ProgramTitle + "Mutex";
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

        private static bool Update()
        {
            string name = AppDomain.CurrentDomain.FriendlyName;
            if (name.EndsWith(".exe"))
                name = name.Substring(0, name.Length - 4);
            int idx = name.LastIndexOf('.');
            if (idx == -1) // .exe
            {
                // Clean up
                if (File.Exists(Model.Utils.Updater.OldName + ".exe"))
                {
                    MessageBox.Show("Delete " + Model.Utils.Updater.OldName + ".exe");
                    File.Delete(Model.Utils.Updater.OldName + ".exe");
                }

                if (File.Exists(Model.Utils.Updater.NewName))
                {
                    MessageBox.Show("Rename .new to .new.exe");
                    File.Move(Model.Utils.Updater.NewName, Model.Utils.Updater.NewName + ".exe");
                    Process.Start(Model.Utils.Updater.NewName + ".exe");
                    return true;
                }
                else
                    return false; // 正常流程
            }
            else
            {
                string suff = name.Substring(idx);
                if (suff == Model.Utils.Updater.NewSuffix) // .new.exe
                {
                    if (File.Exists(Model.Utils.Updater.NoExtName + ".exe"))
                    {
                        MessageBox.Show("Rename .exe to .old.exe");
                        File.Move(Model.Utils.Updater.NoExtName + ".exe", Model.Utils.Updater.OldName + ".exe");
                        Process.Start(Model.Utils.Updater.OldName + ".exe");
                        return true;
                    }
                    else
                    {
                        // Exception
                        return false;
                    }
                }
                else if (suff == Model.Utils.Updater.OldSuffix) // .old.exe
                {
                    if (File.Exists(Model.Utils.Updater.NewName + ".exe"))
                    {
                        MessageBox.Show("Rename .new.exe to .exe");
                        File.Move(Model.Utils.Updater.NewName + ".exe", Model.Utils.Updater.NoExtName + ".exe");
                        Process.Start(Model.Utils.Updater.NoExtName + ".exe");
                        return true;
                    }
                    else
                    {
                        // Exception
                        return false;
                    }
                }
                else
                {
                    // Exception
                    return false;
                }
            }
        }
    }
}
