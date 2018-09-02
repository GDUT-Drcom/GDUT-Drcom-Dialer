using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace Drcom_Dialer
{
    public static class Initializer
    {
        public static bool Initialize(string[] args)
        {
            string exePath = Application.ExecutablePath;
            Environment.CurrentDirectory = Path.GetDirectoryName(exePath);

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //初始化Log
            Model.Utils.Log4Net.SetConfig();

            //防止多启
            Singleton();

            //更新
            if (Update())
            {
                return false;
            }

            //初始化配置
            Model.DialerConfig.Init();

            //VPN修复
            if (Model.DialerConfig.isFixVPN && !Model.Utils.VPNFixer.IsElevated)
            {
                try
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = Application.ExecutablePath,
                        WorkingDirectory = Environment.CurrentDirectory,
                        Verb = "runas"
                    });
                    return false;
                }
                catch (Exception e)
                {
                    Model.Utils.Log4Net.WriteLog(e.Message, e);
                    MessageBox.Show(
                        "需要管理员权限以修复VPN",
                        "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

            //初始化必要组件
            Model.PPPoE.Init();
            Model.Authenticator.Init();
            Model.Utils.GDUT_Drcom.Load();

            if (Model.DialerConfig.isAutoUpdate)
                Model.Utils.DialerUpdater.LaterCheckUpdate();

            Model.Utils.Log4Net.WriteLog("初始化程序成功");

            return true;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception ?? new Exception(nameof(e.ExceptionObject));
            Model.Utils.Log4Net.WriteLog(e.ExceptionObject.ToString(), ex);
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Model.Utils.Log4Net.WriteLog(e.Exception.Message, e.Exception);
            e.SetObserved();
        }

        private static void Singleton()
        {
            var ps = Process.GetProcessesByName(Model.Utils.DialerUpdater.NoExtName);
            int count = ps.Length;
            if (count > 1)
            {
                string msg = $"程序已经运行!\n{ps.Select(p => p.ProcessName).Aggregate((a, b) => $"{a}\n{b}")}";
                MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        private static void WaitProcess(string name)
        {
            foreach (var ps in Process.GetProcessesByName(name))
            {
                if (!ps.WaitForExit(3000))
                {
                    ps.Kill();
                }
            }
        }

        /// <summary>
        /// 更新程序文件
        /// </summary>
        /// <returns>是否需要重启</returns>
        private static bool Update()
        {
            string name = AppDomain.CurrentDomain.FriendlyName;
            if (name.EndsWith(".exe"))
                name = name.Substring(0, name.Length - 4);
            int idx = name.LastIndexOf('.');
            if (idx == -1) // .exe
            {
                // Clean up
                if (File.Exists(Model.Utils.DialerUpdater.OldName + ".exe"))
                {
                    //MessageBox.Show("Delete " + Model.Utils.Updater.OldName + ".exe");
                    WaitProcess(Model.Utils.DialerUpdater.OldName);
                    File.Delete(Model.Utils.DialerUpdater.OldName + ".exe");
                }

                if (File.Exists(Model.Utils.DialerUpdater.NewName))
                {
                    Model.Utils.Log4Net.WriteLog("重命名 .new 到 .new.exe");
                    File.Move(Model.Utils.DialerUpdater.NewName, Model.Utils.DialerUpdater.NewName + ".exe");
                    Process.Start(Model.Utils.DialerUpdater.NewName + ".exe"); //是否有可能导致一个没关一个又开了呢？
                    return true;
                }
                else
                {
                    return false; // 正常流程
                }
            }
            else
            {
                string suff = name.Substring(idx);
                if (suff == Model.Utils.DialerUpdater.NewSuffix) // .new.exe
                {
                    if (File.Exists(Model.Utils.DialerUpdater.NoExtName + ".exe"))
                    {
                        Model.Utils.Log4Net.WriteLog("重命名 .exe 到 .old.exe");
                        WaitProcess(Model.Utils.DialerUpdater.NoExtName);
                        File.Move(Model.Utils.DialerUpdater.NoExtName + ".exe", Model.Utils.DialerUpdater.OldName + ".exe");
                        Process.Start(Model.Utils.DialerUpdater.OldName + ".exe");
                        return true;
                    }
                    else
                    {
                        Model.Utils.Log4Net.WriteLog("未找到 .exe，尝试直接使用 .new.exe");
                        File.Copy(Model.Utils.DialerUpdater.NewName + ".exe", Model.Utils.DialerUpdater.NoExtName + ".exe");
                        Process.Start(Model.Utils.DialerUpdater.NoExtName + ".exe");
                        return true;
                    }
                }
                else if (suff == Model.Utils.DialerUpdater.OldSuffix) // .old.exe
                {
                    if (File.Exists(Model.Utils.DialerUpdater.NewName + ".exe"))
                    {
                        Model.Utils.Log4Net.WriteLog("重命名 .new.exe to .exe");
                        WaitProcess(Model.Utils.DialerUpdater.NewName);
                        File.Move(Model.Utils.DialerUpdater.NewName + ".exe", Model.Utils.DialerUpdater.NoExtName + ".exe");
                        Process.Start(Model.Utils.DialerUpdater.NoExtName + ".exe");
                        return true;
                    }
                    else
                    {
                        Model.Utils.Log4Net.WriteLog("未找到 .new.exe，尝试还原");
                        File.Copy(Model.Utils.DialerUpdater.OldName + ".exe", Model.Utils.DialerUpdater.NoExtName + ".exe");
                        Process.Start(Model.Utils.DialerUpdater.NoExtName + ".exe");
                        return true;
                    }
                }
                else
                {
                    Model.Utils.Log4Net.WriteLog("无法识别的文件后缀");
                    // 正常流程
                    return false;
                }
            }
        }
    }
}
