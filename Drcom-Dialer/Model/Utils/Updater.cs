using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    /// 升级器
    /// </summary>
    internal static class Updater
    {
        public static readonly string UpdateSuffix = ".update.exe";
        public static readonly string ExeName;
        public static readonly string UpdateName;

        static Updater()
        {
            ExeName = AppDomain.CurrentDomain.FriendlyName;
            if (ExeName.Contains("."))
            {
                int idx = ExeName.IndexOf('.');
                ExeName = ExeName.Substring(0, idx) + ".exe";
            }
            UpdateName = ExeName.Substring(0, ExeName.Length - 4) + UpdateSuffix;
        }

        public static void TryUpdate()
        {
            string MirrorHost = "https://api.github.com";
            string MirrorUrl = "/repos/GDUT-Drcom/Drcom-Dialer/releases/latest";
            string MirrorName = "Github";

            RestClient client = new RestClient(MirrorHost);
            RestRequest request = new RestRequest(MirrorUrl);
            IRestResponse response = client.Execute(request);

            Log4Net.WriteLog($"正在从{MirrorName}检测EXE更新");
            if (response != null &&
                response.Content != "" &&
                response.StatusCode == HttpStatusCode.OK)
            {
                JsonObject json = SimpleJson.DeserializeObject(response.Content) as JsonObject;
                string remoteVersion = json["tag_name"] as string;
                Log4Net.WriteLog($"远端EXE版本:{remoteVersion}");

                // 无需更新
                if (remoteVersion == Version.GetVersion())
                    return;

                // 需要更新
                foreach (JsonObject asset in json["assets"] as JsonArray)
                    if (asset["name"] as string == ExeName)
                        if (DownloadFile(asset["browser_download_url"] as string, UpdateName))
                            if (MessageBox.Show(
                                "程序更新完成,是否立即重启?",
                                "更新",
                                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                Log4Net.WriteLog($"Exe Updated");
                                RebootForUpdate();
                            }
            }
            else
            {
                Log4Net.WriteLog($"{MirrorName}源获取失败");
            }
        }

        /// <summary>
        /// 重启更新
        /// </summary>
        private static void RebootForUpdate()
        {
            Process proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = UpdateName
                }
            };
            proc.Start();

            // 搞事搞事
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        // ctrl-c ctrl-v 了，要重构，提取函数了
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="path">本地文件</param>
        /// <returns></returns>
        private static bool DownloadFile(string url, string path)
        {
            int index = url.IndexOf("/", 10); //懒的用其他的了，这是第三个/的出现的位置

            RestClient client = new RestClient(url.Substring(0, index));
            RestRequest request = new RestRequest(url.Substring(index, url.Length - index));

            byte[] result = client.DownloadData(request);
            if (result.Length < 1024)
            {
                Log4Net.WriteLog($"下载心跳包失败: 太少数据({result.Length}b)");
                return false;
            }
            try
            {
                GDUT_Drcom.Unload();
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    stream.Write(result, 0, result.Length);
                }
                GDUT_Drcom.Load();
                Log4Net.WriteLog($"心跳更新成功({GDUT_Drcom.Version})");
            }
            catch (Exception e)
            {
                Log4Net.WriteLog("下载心跳包失败: " + e.Message, e);
                return false;
            }

            return true;
        }
    }
}
