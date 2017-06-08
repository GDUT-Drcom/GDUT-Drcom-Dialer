using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Drcom_Dialer.Model
{
    /// <summary>
    /// 拨号器配置类
    /// </summary>
    internal static class DialerConfig
    {
        //todo:命名！！
        //看到这个命名……

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
        public static bool isFixVPN = false;

        /// <summary>
        /// 发送反馈
        /// </summary>
        public static bool isFeedback = true;

        /// <summary>
        /// 到期前提醒
        /// </summary>
        public static bool isNotifyWhenExpire = true;

        /// <summary>
        /// 自动更新
        /// </summary>
        public static bool isAutoUpdate = true;

        /// <summary>
        /// GUID
        /// 用于发送反馈
        /// </summary>
        public static string guid = "";

        /// <summary>
        /// 配置文件版本
        /// </summary>
        public static int configVer = 2;

        /// <summary>
        /// 校区枚举项
        /// </summary>
        public enum Campus
        {
            /// <summary>
            ///     大学城
            /// </summary>
            HEMC = 0,
            /// <summary>
            ///     龙洞
            /// </summary>
            LongDong = 1,
            /// <summary>
            ///     东风路
            /// </summary>
            DongfengRd = 2,
            /// <summary>
            ///     番禺
            /// </summary>
            Panyu = 3,
            /// <summary>
            ///     未知
            /// </summary>
            Unknown = -1
        }
        /// <summary>
        /// 校区选择
        /// </summary>
        public static Campus zone = Campus.Unknown;

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
                        return "10.0.3.6";
                    case Campus.Panyu:
                        return "0.0.0.0";
                    default:
                        return "10.0.3.2";
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
            ReadConfig();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public static void SaveConfig()
        {
            if (cfa.AppSettings.Settings.Count > 0)
            {
                saveConfig();
            }
            else
            {
                CreateConfig(0);
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        private static void saveConfig()
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                Utils.Log4Net.WriteLog($"'{nameof(saveConfig)}' is ignored because stratup from cmd");
                return;
            }
            try
            {
                cfa.AppSettings.Settings[nameof(username)].Value = username;
                cfa.AppSettings.Settings[nameof(password)].Value = isRememberPassword ? password : "";
                cfa.AppSettings.Settings[nameof(isAutoLogin)].Value = isAutoLogin ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(isRememberPassword)].Value = isRememberPassword ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(isRunOnStartup)].Value = isRunOnStartup ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(isReDialOnFail)].Value = isReDialOnFail ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(isFixVPN)].Value = isFixVPN ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(isFeedback)].Value = isFeedback ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(isNotifyWhenExpire)].Value = isNotifyWhenExpire ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(isAutoUpdate)].Value = isAutoUpdate ? "Y" : "N";
                cfa.AppSettings.Settings[nameof(guid)].Value = guid;
                cfa.AppSettings.Settings[nameof(zone)].Value = ((int)zone).ToString();
                cfa.AppSettings.Settings[nameof(configVer)].Value = configVer.ToString();
                cfa.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
            }
        }
        /// <summary>
        /// 我觉得做这个没用
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static string ConvertBoolYN(bool parameter)
        {
            const string True = "Y";
            const string False = "N";

            return parameter ? True : False;
        }

        /// <summary>
        /// 创建配置文件
        /// </summary>
        private static void CreateConfig(int configVer)
        {
            //神，居然这样写
            try
            {
                switch (configVer)
                {
                    case 0:
                        cfa.AppSettings.Settings.Add(nameof(username), username);
                        cfa.AppSettings.Settings.Add(nameof(password), isRememberPassword ? password : "");
                        cfa.AppSettings.Settings.Add(nameof(isAutoLogin), ConvertBoolYN(isAutoLogin));
                        cfa.AppSettings.Settings.Add(nameof(isRememberPassword), ConvertBoolYN(isRememberPassword));
                        cfa.AppSettings.Settings.Add(nameof(isRunOnStartup), ConvertBoolYN(isRunOnStartup));
                        cfa.AppSettings.Settings.Add(nameof(isReDialOnFail), ConvertBoolYN(isReDialOnFail));
                        cfa.AppSettings.Settings.Add(nameof(isFixVPN), ConvertBoolYN(isFixVPN));
                        cfa.AppSettings.Settings.Add(nameof(isFeedback), ConvertBoolYN(isFeedback));
                        cfa.AppSettings.Settings.Add(nameof(isNotifyWhenExpire), ConvertBoolYN(isNotifyWhenExpire));
                        cfa.AppSettings.Settings.Add(nameof(zone), ((int)zone).ToString());
                        cfa.AppSettings.Settings.Add(nameof(guid), guid);
                        cfa.AppSettings.Settings.Add(nameof(configVer), configVer.ToString());
                        goto case 1;
                    case 1:
                        cfa.AppSettings.Settings.Add(nameof(isAutoUpdate), ConvertBoolYN(isAutoUpdate));
                        goto case 2;
                    case 2:
                        break;
                    default:
                        goto case 0;
                }

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
        private static void ReadConfig()
        {
            try
            {
                if (cfa.AppSettings.Settings.Count > 0)
                {
                    //升级配置文件
                    CreateConfig(int.Parse(cfa.AppSettings.Settings[nameof(configVer)].Value));

                    username = cfa.AppSettings.Settings[nameof(username)].Value;
                    password = cfa.AppSettings.Settings[nameof(password)].Value;
                    isAutoLogin = cfa.AppSettings.Settings[nameof(isAutoLogin)].Value == "Y";
                    isRememberPassword = cfa.AppSettings.Settings[nameof(isRememberPassword)].Value == "Y";
                    isRunOnStartup = cfa.AppSettings.Settings[nameof(isRunOnStartup)].Value == "Y";
                    isReDialOnFail = cfa.AppSettings.Settings[nameof(isReDialOnFail)].Value == "Y";
                    isFixVPN = cfa.AppSettings.Settings[nameof(isFixVPN)].Value == "Y";
                    isFeedback = cfa.AppSettings.Settings[nameof(isFeedback)].Value == "Y";
                    isNotifyWhenExpire = cfa.AppSettings.Settings[nameof(isNotifyWhenExpire)].Value == "Y";
                    isAutoUpdate = cfa.AppSettings.Settings[nameof(isAutoUpdate)].Value == "Y";
                    guid = cfa.AppSettings.Settings[nameof(guid)].Value;
                    zone = (Campus)int.Parse(cfa.AppSettings.Settings[nameof(zone)].Value);
                }
                else
                {
                    //创建一个配置文件
                    CreateConfig(0);
                }

                // cmd-arguments overwrite config
                Regex userpat = new Regex(@"^u=(\d+)");
                Regex pswpat = new Regex(@"^p=(\w+)");
                foreach(var opt in Environment.GetCommandLineArgs())
                {
                    switch (opt)
                    {
                        case "con": // connect
                            isAutoLogin = true;
                            break;
                        case "vpn": // fix vpn
                            isFixVPN = true;
                            break;
                        default: // user | psw
                            var m0 = userpat.Match(opt);
                            var m1 = pswpat.Match(opt);
                            if (m0.Success)
                                username = m0.Groups[1].Value;
                            else if (m1.Success)
                                password = m1.Groups[1].Value;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
            }
        }
    }
}
