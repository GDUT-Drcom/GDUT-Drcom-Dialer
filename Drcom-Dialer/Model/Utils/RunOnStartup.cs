using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    /// 自启动类
    /// </summary>
    static class RunOnStartup
    {
        /// <summary>
        /// 设置自启动
        /// </summary>
        /// <returns></returns>
        public static bool SetStartup()
        {
            return SetStartup(true);
        }
        /// <summary>
        /// 取消自启动
        /// </summary>
        /// <returns></returns>
        public static bool UnsetStartup()
        {
            return SetStartup(false);
        }
        /// <summary>
        /// 改变自启动状态
        /// </summary>
        /// <param name="setOrUnset">设置/取消</param>
        /// <returns></returns>
        public static bool SetStartup(bool setOrUnset)
        {
            RegistryKey _regPath = Registry.CurrentUser;
            string path = Application.ExecutablePath;
            RegistryKey _regKey = _regPath.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            try
            {
                if (setOrUnset)
                    _regKey.SetValue(Application.ProductName, path);
                else
                    _regKey.DeleteValue(Application.ProductName);
                return true;
            }
            catch (Exception e)
            {
                Log4Net.WriteLog(e.Message, e);
                return false;
            }
        }
    }
}
