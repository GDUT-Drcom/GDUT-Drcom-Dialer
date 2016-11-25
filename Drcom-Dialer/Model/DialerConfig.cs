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
        public static string username = "";
        /// <summary>
        /// 密码
        /// </summary>
        public static string password = "";

        /// <summary>
        /// 是否记住密码
        /// </summary>
        public static bool isRememberPassword = false;

        /// <summary>
        /// 是否自动登录
        /// </summary>
        public static bool isAutoLogin = false;

        /// <summary>
        /// 是否开机启动
        /// </summary>
        public static bool isRunOnStartup = false;

        /// <summary>
        /// 是否断线重连
        /// </summary>
        public static bool isReDialOnFail = false;

        /// <summary>
        /// VPN修复
        /// </summary>
        public static bool isFixVPN = false ;

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
        public static Campus zone = 0;

        /// <summary>
        /// 认证地址
        /// </summary>
        public static string AuthIP
        {
            get
            {
                switch (zone)
                {
                    case Campus.HEMC:
                        return "10.0.3.2";
                    case Campus.LongDong:
                        return "10.0.3.6";
                    case Campus.DongfengRd:
                        return "0.0.0.0";
                    case Campus.Panyu:
                        return "0.0.0.0";
                    default:
                        return "0.0.0.0";
                }
            }
        }

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
                cfa.AppSettings.Settings[nameof(username)].Value = username;
                cfa.AppSettings.Settings[nameof(password)].Value = isRememberPassword ? password : "";
                cfa.AppSettings.Settings[nameof(isAutoLogin)].Value = isAutoLogin ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(isRememberPassword)].Value = isRememberPassword ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(isRunOnStartup)].Value = isRunOnStartup ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(isReDialOnFail)].Value = isReDialOnFail ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(isFixVPN)].Value = isFixVPN ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(zone)].Value = zone.ToString();
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
                cfa.AppSettings.Settings.Add(nameof(username), username);
                cfa.AppSettings.Settings.Add(nameof(password), isRememberPassword ? password : "");
                cfa.AppSettings.Settings.Add(nameof(isAutoLogin), isAutoLogin ? "Y" : "N");
                cfa.AppSettings.Settings.Add(nameof(isRememberPassword), isRememberPassword ? "Y" : "N");
                cfa.AppSettings.Settings.Add(nameof(isRunOnStartup), isRunOnStartup ? "Y" : "N");
                cfa.AppSettings.Settings.Add(nameof(isReDialOnFail), isReDialOnFail ? "Y" : "N");
                cfa.AppSettings.Settings.Add(nameof(isFixVPN), isFixVPN ? "Y" : "N");
                cfa.AppSettings.Settings.Add(nameof(zone), ((int)zone).ToString());
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
                username = cfa.AppSettings.Settings[nameof(username)].Value;
                password = cfa.AppSettings.Settings[nameof(password)].Value;
                isAutoLogin = cfa.AppSettings.Settings[nameof(isAutoLogin)].Value == "Y";
                isRememberPassword = cfa.AppSettings.Settings[nameof(isRememberPassword)].Value == "Y";
                isRunOnStartup = cfa.AppSettings.Settings[nameof(isRunOnStartup)].Value == "Y";
                isReDialOnFail = cfa.AppSettings.Settings[nameof(isReDialOnFail)].Value == "Y";
                isFixVPN = cfa.AppSettings.Settings[nameof(isFixVPN)].Value == "Y";
                zone = (Campus)int.Parse(cfa.AppSettings.Settings[nameof(zone)].Value);
            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
            }
        }
    }
}
