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
    class VPNFixer
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
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 重启进程提升权限
        /// 这会导致自身退出
        /// </summary>
        public static void Elevate()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.WorkingDirectory = Environment.CurrentDirectory;
            psi.FileName = Application.ExecutablePath;
            psi.Verb = "runas";
            psi.Arguments = StartupArgs;
            try
            {
                Process.Start(psi);
            }
            catch // 用户拒绝了UAC
            {
                Log4Net.WriteLog("用户拒绝提升权限");
                // TODO : 提醒用户需要允许UAC
                return;
            }
            //还是不要退出比较好
            //Application.Exit();
        }

        /// <summary>
        /// VPN 修复
        /// 向路由表添加一条规则
        /// </summary>
        public static void AddRouteRule()
        {
            if (!IsElevated)
                throw new InvalidOperationException("没有足够权限");
            string gateway = FindGateway();

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "route";
            // TODO 认证地址需要获取
            psi.Arguments = $"add {DialerConfig.AuthIP} mask 255.255.255.255 {gateway} metric 5";
            psi.CreateNoWindow = true;
            Process proc = Process.Start(psi);
            proc.WaitForExit();
        }

        /// <summary>
        /// 用route print找Gateway
        /// </summary>
        /// <returns>Gateway</returns>
        private static string FindGateway()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "route";
            psi.Arguments = "print";
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            Process proc = Process.Start(psi);
            proc.WaitForExit();
            string rout = proc.StandardOutput.ReadToEnd();
            Regex reg = new Regex(@"(0\.0\.0\.0\s+){2}(\d+\.\d+\.\d+\.\d)+");
            var mc = reg.Matches(rout);
            return mc[0].Groups[2].Value;
        }

        public const string StartupArgs = "fixvpn";
    }
}
