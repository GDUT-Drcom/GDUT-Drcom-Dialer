using Drcom_Dialer.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    /// VPN 修复
    /// (测试可添加路由表)
    /// </summary>
    internal static class VPNFixer
    {
        /// <summary>
        /// 是否有管理员权限
        /// </summary>
        public static bool IsElevated
        {
            get
            {
                try
                {
                    WindowsIdentity user = WindowsIdentity.GetCurrent();
                    WindowsPrincipal principal = new WindowsPrincipal(user);
                    return principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 修复VPN
        /// </summary>
        public static void Fix()
        {
            if (!IsElevated)
            {
                Log4Net.WriteLog(nameof(Fix), new InvalidOperationException("没有足够权限"));
                MessageBox.Show(
                    "需要管理员权限以修复VPN(请重启并允许UAC)",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            string gateway = FindGateway();
            string IF = FindInterface();
            string args = $"add {DialerConfig.AuthIP} mask 255.255.255.255 0.0.0.0 metric 5 IF {IF}";
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "route",
                Arguments = args,
                CreateNoWindow = true
            };
            Process proc = Process.Start(psi);
            proc?.WaitForExit();
            int metric = CheckMetric();
            if (metric > 128)
            {
                MessageBox.Show(
                    $"VPN跃点数过大，可能修复失败，如失败请断线重试(metric={metric})",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 用route print找Gateway
        /// </summary>
        /// <returns>Gateway</returns>
        private static string FindGateway()
        {
            try
            {
                string rout = ExcRoutePrint();
                Regex reg = new Regex(@"(0\.0\.0\.0\s+){2}(\d+\.\d+\.\d+\.\d)+");
                var mc = reg.Matches(rout);
                return mc[0].Groups[2].Value;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static string FindInterface()
        {
            try
            {
                string rout = ExcRoutePrint();
                Regex reg = new Regex($@"(\d+)\.+{Properties.Resources.RasConnectionName}");
                var mc = reg.Matches(rout);
                return mc[0].Groups[1].Value;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private static int CheckMetric()
        {
            try
            {
                string rout = ExcRoutePrint();
                string re = DialerConfig.AuthIP.Replace(".", "\\.");
                re += @"\s+255\.255\.255\.255\s+[^\s]+\s+\d+\.\d+\.\d+\.\d+\s+(\d+)";
                Regex reg = new Regex(re);
                var mc = reg.Matches(rout);
                return int.Parse(mc[0].Groups[1].Value);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static string ExcRoutePrint()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "route",
                Arguments = "print",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            Process proc = Process.Start(psi);
            proc?.WaitForExit();
            return proc.StandardOutput.ReadToEnd();
        }

        public const string StartupArgs = "fixvpn";
    }
}
