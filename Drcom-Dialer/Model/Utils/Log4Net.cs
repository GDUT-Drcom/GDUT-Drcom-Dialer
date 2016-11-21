using System;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    /// Log4Net日志记录类
    /// </summary>
    class Log4Net
    {
        //log4net日志专用
        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        private static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void SetConfig()
        {
            //log4net.Config.XmlConfigurator.Configure();
            log4net.Appender.FileAppender fAppender = new log4net.Appender.FileAppender();
            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout();

            layout.ConversionPattern = "[%date] %thread -- %-5level -- %logger [%M] -- %message%newline";
            layout.ActivateOptions();

            fAppender.File = "DogCom.log";
            fAppender.Layout = layout;
            fAppender.AppendToFile = true;
            fAppender.ActivateOptions();
            log4net.Config.BasicConfigurator.Configure(fAppender);
            
        }

        /// <summary>
        /// 记录普通日志
        /// </summary>
        /// <param name="info">日志内容</param>
        public static void WriteLog(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                try
                {
                    loginfo.Info(info);
                    Console.WriteLine(info);
                }
                catch
                {

                }

            }
        }
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="info">日志内容</param>
        /// <param name="se">异常</param>
        public static void WriteLog(string info, Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                try
                {
                    logerror.Error(info, se);
                }
                catch
                {

                }

            }
        }
    }
}
