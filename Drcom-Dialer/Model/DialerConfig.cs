using System;
using System.Configuration;

namespace Drcom_Dialer.Model
{
    /// <summary>
    /// 拨号器配置类
    /// </summary>
    internal static class DialerConfig
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public static string username;
        /// <summary>
        /// 密码
        /// </summary>
        public static string password;

        /// <summary>
        /// 是否记住密码
        /// </summary>
        public static bool isRememberPassword;

        /// <summary>
        /// 是否自动登录
        /// </summary>
        public static bool isAutoLogin;

        /// <summary>
        /// 是否开机启动
        /// </summary>
        public static bool isRunOnStartup;

        /// <summary>
        /// 是否断线重连
        /// </summary>
        public static bool isReDialOnFail;

        /// <summary>
        /// VPN修复
        /// </summary>
        public static bool isFixVPN;

        /// <summary>
        /// 校区枚举项
        /// </summary>
        public enum Campus
        {
            HEMC = 0,
            LongDong = 1,
            DongfengRd = 2,
            Panyu = 3
        }
        /// <summary>
        /// 校区选择
        /// </summary>
        public static Campus zone;

        /// <summary>
        /// 配置类引用
        /// </summary>
        private static Configuration cfa;

        /// <summary>
        /// 拨号器配置类
        /// </summary>
        public static void Init()
        {
            cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            readConfig();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public static void SaveConfig()
        {
            if (cfa.AppSettings.Settings.Count > 0)
                saveConfig();
            else
                createConfig();
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        private static void saveConfig()
        {
            try
            {
                //bug:如果写的和Read不同就炸了
                cfa.AppSettings.Settings["username"].Value = username;
                cfa.AppSettings.Settings["password"].Value = isRememberPassword ? password : "";
                cfa.AppSettings.Settings["autoLogin"].Value = isAutoLogin ? "Y" : "N";
                cfa.AppSettings.Settings["rememberPassword"].Value = isRememberPassword ? "Y" : "N";
                cfa.AppSettings.Settings["runOnStartup"].Value = isRunOnStartup ? "Y" : "N";
                cfa.AppSettings.Settings["reDialOnFail"].Value = isReDialOnFail ? "Y" : "N";
                cfa.AppSettings.Settings["fixVPN"].Value = isFixVPN ? "Y" : "N";
                cfa.AppSettings.Settings["campusZone"].Value = zone.ToString();
                cfa.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
            }
        }

        /// <summary>
        /// 创建配置文件
        /// </summary>
        private static void createConfig()
        {
            try
            {
                //bug:写的和上面不一样就炸了
                cfa.AppSettings.Settings.Add("username", username);
                cfa.AppSettings.Settings.Add("password", isRememberPassword ? password : "");
                cfa.AppSettings.Settings.Add("autoLogin", isAutoLogin ? "Y" : "N");
                cfa.AppSettings.Settings.Add("rememberPassword", isRememberPassword ? "Y" : "N");
                cfa.AppSettings.Settings.Add("runOnStartup", isRunOnStartup ? "Y" : "N");
                cfa.AppSettings.Settings.Add("reDialOnFail", isReDialOnFail ? "Y" : "N");
                cfa.AppSettings.Settings.Add("fixVPN", isFixVPN ? "Y" : "N");
                cfa.AppSettings.Settings.Add("campusZone", ((int)zone).ToString());
                cfa.Save();
            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
            }
        }
        
        /// <summary>
        /// 读配置文件
        /// </summary>
        private static void readConfig()
        {
            try
            {
                //建议：不要写字符串作为setting
                //todo：这样写容易炸，建议nameof
                username = cfa.AppSettings.Settings["username"].Value;
                password = cfa.AppSettings.Settings["password"].Value;
                isAutoLogin = cfa.AppSettings.Settings["autoLogin"].Value == "Y";
                isRememberPassword = cfa.AppSettings.Settings["rememberPassword"].Value == "Y";
                isRunOnStartup = cfa.AppSettings.Settings["runOnStartup"].Value == "Y";
                isReDialOnFail = cfa.AppSettings.Settings["reDialOnFail"].Value == "Y";
                isFixVPN = cfa.AppSettings.Settings["fixVPN"].Value == "Y";
                zone = (Campus)int.Parse(cfa.AppSettings.Settings["campusZone"].Value);
            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
            }
        }
    }
}
