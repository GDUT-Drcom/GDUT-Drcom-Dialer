using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    /// 主程序升级器
    /// </summary>
    internal static class DialerUpdater
    {
        public static readonly string NoExtName;
        public static readonly string NewSuffix = ".new";
        public static readonly string OldSuffix = ".old";
        public static string NewName => NoExtName + NewSuffix;
        public static string OldName => NoExtName + OldSuffix;

        public static Timer UpdateTimer;

        static DialerUpdater()
        {
            NoExtName = AppDomain.CurrentDomain.FriendlyName;
            int idx = NoExtName.IndexOf('.');
            if (idx != -1)
                NoExtName = NoExtName.Substring(0, idx);
        }

        public static void TryUpdate()
        {
            string RemoteFileUrl = Updater.CheckUpdate("GDUT-Drcom/Drcom-Dialer", NoExtName, Version.GetVersion());

            if (RemoteFileUrl != null)
            {
                if (Updater.DownloadFile(RemoteFileUrl, NewName))
                {
                    Log4Net.WriteLog($"成功更新主程序");
                    ViewModel.ViewModel.View.ShowBalloonTip(
                        3000,
                        "应用更新",
                        "程序更新成功，将在下次启动生效");
                    return;
                }
            }
        }
        /// <summary>
        /// 等会再检测
        /// </summary>
        public static void LaterCheckUpdate()
        {
            try
            {
                if (UpdateTimer == null)
                {
                    UpdateTimer = new Timer(new TimerCallback((state) =>
                    {
                        if (ViewModel.ViewModel.View.DialStatus == ViewModel.ViewModel.DialHangupStatus.Connect)
                            TryUpdate();
                        StopCheckUpdateTimer();
                    }), null, 1000 * 60 * 10, -1);
                }
            }
            catch(Exception e)
            {
                Log4Net.WriteLog(e.Message, e);
            }
        }

        /// <summary>
        /// 停止检测
        /// </summary>
        public static void StopCheckUpdateTimer()
        {
            try
            {
                if (UpdateTimer != null)
                {
                    UpdateTimer.Change(-1, -1);
                    UpdateTimer = null;
                }
            }
            catch (Exception e)
            {
                Log4Net.WriteLog(e.Message, e);
            }
        }
    }

    /// <summary>
    /// 升级公共类
    /// </summary>
    internal static class Updater
    {
        public enum UpdateState
        {
            UpToDate,
            Updated,
            Failed
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="path">本地文件</param>
        /// <returns></returns>
        public static bool DownloadFile(string url, string path)
        {
            int index = url.IndexOf("/", 10); //懒的用其他的了，这是第三个/的出现的位置

            RestClient client = new RestClient(url.Substring(0, index));
            RestRequest request = new RestRequest(url.Substring(index, url.Length - index));

            byte[] result = client.DownloadData(request);
            if (result.Length < 1024)
            {
                Log4Net.WriteLog($"下载失败: 太少数据({result.Length}b)");
                return false;
            }
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    stream.Write(result, 0, result.Length);
                }
                Log4Net.WriteLog($"下载成功({path})");
            }
            catch (Exception e)
            {
                Log4Net.WriteLog("下载失败: " + e.Message, e);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     使用指定的Mirror检测更新
        /// </summary>
        /// <param name="mirrorHost">Mirror主机</param>
        /// <param name="mirrorUrl">RestfulAPI的URL</param>
        /// <param name="mirrorName">Mirror名称</param>
        /// <param name="fileName">文件名</param>
        /// <param name="currentVersion">当前版本</param>
        /// <returns></returns>
        public static String CheckUpdate(string mirrorHost, string mirrorUrl, string mirrorName, string fileName, string currentVersion)
        {
            var client = new RestClient(mirrorHost);
            var request = new RestRequest(mirrorUrl);
            var response = client.Execute(request);


            Log4Net.WriteLog($"[{fileName}]正在从{mirrorName}检测更新");
            try
            {
                if (!string.IsNullOrEmpty(response?.Content) &&
                    (response.StatusCode == HttpStatusCode.OK))
                {
                    JsonObject json = SimpleJson.DeserializeObject(response.Content) as JsonObject;
                    string remoteVersion = json["tag_name"] as string;
                    Log4Net.WriteLog($"远端版本:{remoteVersion}");

                    // 无需更新
                    if (remoteVersion == currentVersion)
                    {
                        return null;
                    }

                    // 需要更新
                    foreach (JsonObject asset in json["assets"] as JsonArray)
                    {
                        if (asset["name"] as string == fileName)
                        {
                            return asset["browser_download_url"] as string;
                        }
                    }
                }
                else
                {
                    Log4Net.WriteLog($"{mirrorName}源获取失败");
                }
            }
            catch (Exception e)//更新网络数据不是想要的
            {
                Log4Net.WriteLog($"{mirrorName}源获取失败", e);
            }
            return null;
        }
        /// <summary>
        ///     从Github获取更新
        /// </summary>
        /// <param name="githubRepoPath">库路径(e.g. GDUT-Drcom/Drcom-Dialer)</param>
        /// <param name="fileName">文件名</param>
        /// <param name="currentVersion">当前版本</param>
        /// <returns>url</returns>
        public static string CheckUpdate(string githubRepoPath, string fileName, string currentVersion)
        {
            return CheckUpdate("https://api.github.com",
                $"/repos/{githubRepoPath}/releases/latest",
                "Github",
                fileName,
                currentVersion);
        }
    }
}
