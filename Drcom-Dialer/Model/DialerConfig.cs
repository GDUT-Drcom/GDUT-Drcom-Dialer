using System;
using System.Configuration;

namespace Drcom_Dialer.Model
{
    /// <summary>
    /// 拨号器配置类
    /// </summary>
    class DialerConfig
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username;
        /// <summary>
        /// 密码
        /// </summary>
        public string password;

        /// <summary>
        /// 是否记住密码
        /// </summary>
        public bool isRememberPassword;
        /// <summary>
        /// 是否自动登录
        /// </summary>
        public bool isAutoLogin;
        
        /*
        /// <summary>
        /// 是否开机启动
        /// </summary>
        public bool isRunOnStartup;
        /// <summary>
        /// 是否断线重连
        /// </summary>
        public bool isReDialOnFail;
        */
        /// <summary>
        /// 配置类引用
        /// </summary>
        private Configuration cfa;

        /// <summary>
        /// 拨号器配置类
        /// </summary>
        public DialerConfig()
        {
            cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            readConfig();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveConfig()
        {
            if (cfa.AppSettings.Settings.Count > 0)
                saveConfig();
            else
                createConfig();
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        private void saveConfig()
        {
            try
            {
                cfa.AppSettings.Settings["username"].Value = username;
                cfa.AppSettings.Settings["password"].Value = isRememberPassword ? password : "";
                cfa.AppSettings.Settings["autoLogin"].Value = isAutoLogin ? "Y" : "N";
                cfa.AppSettings.Settings["rememberPassword"].Value = isRememberPassword ? "Y" : "N";
                /**
                cfa.AppSettings.Settings["runOnStartup"].Value = isRunOnStartup ? "Y" : "N";
                cfa.AppSettings.Settings["reDialOnFail"].Value = isReDialOnFail ? "Y" : "N";
                **/
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
        private void createConfig()
        {
            try
            {
                cfa.AppSettings.Settings.Add("username", username);
                cfa.AppSettings.Settings.Add("password", isRememberPassword ? password : "");
                cfa.AppSettings.Settings.Add("autoLogin", isAutoLogin ? "Y" : "N");
                cfa.AppSettings.Settings.Add("rememberPassword", isRememberPassword ? "Y" : "N");
                /**
                cfa.AppSettings.Settings.Add("runOnStartup", isRunOnStartup ? "Y" : "N");
                cfa.AppSettings.Settings.Add("reDialOnFail", isReDialOnFail ? "Y" : "N");
                **/
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
        private void readConfig()
        {
            try
            {
                username = cfa.AppSettings.Settings["username"].Value;
                password = cfa.AppSettings.Settings["password"].Value;
                isAutoLogin = cfa.AppSettings.Settings["autoLogin"].Value == "Y";
                isRememberPassword = cfa.AppSettings.Settings["rememberPassword"].Value == "Y";
                /**
                isRunOnStartup = cfa.AppSettings.Settings["runOnStartup"].Value == "Y";
                isReDialOnFail = cfa.AppSettings.Settings["reDialOnFail"].Value == "Y";
                **/
            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
            }
        }
    }
}
