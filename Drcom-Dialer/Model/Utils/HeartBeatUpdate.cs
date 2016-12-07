using System;
using System.IO;
using RestSharp;
using System.Net;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    /// 心跳包升级类
    /// </summary>
    internal static class HeartBeatUpdate
    {
        private const string DllName = "gdut-drcom.dll";

        /// <summary>
        /// 检测DLL是否存在
        /// </summary>
        /// <returns></returns>
        public static bool CheckDLL()
        {
            return File.Exists(DllName);
        }
        
        public enum UpdateState { UpToDate, Updated, Failed }
        /// <summary>
        /// 检测更新
        /// </summary>
        /// <returns></returns>
        public static UpdateState TryUpdate()
        {
            string localVersion = GDUT_Drcom.Version;
            Log4Net.WriteLog($"开始更新{DllName},当前心跳包版本({localVersion})");
            UpdateState result;

            result = TryUpdate("https://api.github.com", "/repos/chenhaowen01/gdut-drcom/releases/latest", "Github");

            if (result != UpdateState.Failed)
                return result;

            //Mirror
            //result = TryUpdate("https://api.github.com", "/repos/chenhaowen01/gdut-drcom/releases/latest", "Github");
            return result;

        }
        /// <summary>
        /// 使用指定的Mirror检测更新
        /// </summary>
        /// <param name="MirrorHost">Mirror主机</param>
        /// <param name="MirrorUrl">RestfulAPI的URL</param>
        /// <param name="MirrorName">Mirror名称</param>
        /// <returns></returns>
        public static UpdateState TryUpdate(string MirrorHost, string MirrorUrl, string MirrorName)
        {
            RestClient client;
            RestRequest request;
            IRestResponse response;

            client = new RestClient(MirrorHost);
            request = new RestRequest(MirrorUrl);
            response = client.Execute(request);


            Log4Net.WriteLog($"正在从{MirrorName}检测DLL更新");
            if (response != null &&
                response.Content != "" &&
                response.StatusCode == HttpStatusCode.OK)
            {
                JsonObject json = SimpleJson.DeserializeObject(response.Content) as JsonObject;
                string remoteVersion = json["tag_name"] as string;
                Log4Net.WriteLog($"远端版本:{remoteVersion}");

                // 无需更新
                if (remoteVersion == GDUT_Drcom.Version)
                    return UpdateState.UpToDate;

                // 需要更新
                foreach (JsonObject asset in json["assets"] as JsonArray)
                    if (asset["name"] as string == DllName)
                        if (DownloadFile(asset["browser_download_url"] as string, DllName))
                            return UpdateState.Updated;
            }
            else
            {
                Log4Net.WriteLog($"{MirrorName}源获取失败");
            }
            return UpdateState.Failed;
        }


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
