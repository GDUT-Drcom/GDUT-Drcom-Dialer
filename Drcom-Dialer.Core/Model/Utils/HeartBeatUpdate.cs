using System;
using System.IO;
using RestSharp;
using System.Net;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    ///     心跳包升级类
    /// </summary>
    internal static class HeartBeatUpdate
    {
        private const string DllName = "gdut-drcom.dll";

        /// <summary>
        ///     检测DLL是否存在
        /// </summary>
        /// <returns></returns>
        public static bool CheckDLL()
        {
            return File.Exists(DllName);
        }

        /// <summary>
        ///     检测更新
        /// </summary>
        /// <returns></returns>
        public static Updater.UpdateState TryUpdate()
        {
            string localVersion = GDUT_Drcom.Version;
            Log4Net.WriteLog($"开始更新{DllName},当前心跳包版本({localVersion})");

            try
            {
                string RemoteFileUrl = Updater.CheckUpdate("https://tools.bigkeer.cn/", "drcom/heartbeat.json", "bigkeer", DllName, GDUT_Drcom.Version);

                if (RemoteFileUrl == "")
                {
                    Log4Net.WriteLog("无需更新");
                    return Updater.UpdateState.UpToDate;
                }

                if (RemoteFileUrl != null)
                {
                    GDUT_Drcom.Unload();
                    if (Updater.DownloadFile(RemoteFileUrl, DllName))
                    {
                        GDUT_Drcom.Load();
                        return Updater.UpdateState.Updated;
                    }
                    else
                    {
                        GDUT_Drcom.Load();
                    }
                }

                RemoteFileUrl = Updater.CheckUpdate("chenhaowen01/gdut-drcom", DllName, GDUT_Drcom.Version);

                if (RemoteFileUrl == "")
                {
                    Log4Net.WriteLog("无需更新");
                    return Updater.UpdateState.UpToDate;
                }

                if (RemoteFileUrl != null)
                {
                    GDUT_Drcom.Unload();
                    if (Updater.DownloadFile(RemoteFileUrl, DllName))
                    {
                        GDUT_Drcom.Load();
                        return Updater.UpdateState.Updated;
                    }
                    else
                    {
                        GDUT_Drcom.Load();
                        return Updater.UpdateState.Failed;
                    }
                }

                return Updater.UpdateState.Failed;
            }
            catch (Exception e)
            {
                Log4Net.WriteLog(e.Message, e);
                return Updater.UpdateState.Failed;
            }
        }
    }
}