using System;
using System.IO;
using RestSharp;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    /// 心跳包升级类
    /// </summary>
    static class HeartBeatUpdate
    {
        /// <summary>
        /// 检测DLL是否存在
        /// </summary>
        /// <returns></returns>
        public static bool CheckDLL()
        {
            return File.Exists("gdut-drcom.dll");
        }
        /// <summary>
        /// 检测更新
        /// </summary>
        /// <returns></returns>
        public static bool CheckUpdate()
        {
            RestClient client = new RestClient("https://api.github.com");
            RestRequest request = new RestRequest("/repos/chenhaowen01/gdut-drcom/releases/latest");
            IRestResponse<GithubReleaseResponse> response = client.Execute<GithubReleaseResponse>(request);

            if (response != null &&
                response.Content != "" &&
                response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data.name != GDUT_Drcom.Version;
            }

            //另一个mirror
            client = new RestClient("https://api.github.com");
            request = new RestRequest("/repos/chenhaowen01/gdut-drcom/releases/latest");
            response = client.Execute<GithubReleaseResponse>(request);

            if (response != null &&
                response.Content != "" &&
                response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data.name != GDUT_Drcom.Version;
            }

            return false;
        }

        /// <summary>
        /// 升级DLL
        /// </summary>
        /// <returns></returns>
        public static bool Update()
        {
            Log4Net.WriteLog("开始更新gdut-drcom.dll");

            RestClient client = new RestClient("https://api.github.com");
            client.Timeout = 2000;
            RestRequest request = new RestRequest("/repos/chenhaowen01/gdut-drcom/releases/latest");
            IRestResponse<GithubReleaseResponse> response = client.Execute<GithubReleaseResponse>(request);
            
            if (response != null &&
                response.Content != "" &&
                response.ResponseStatus == ResponseStatus.Completed)
            {
                if (response.Data.assets != null)
                {
                    foreach (GithubReleaseAssetItem fileName in response.Data.assets)
                    {
                        if (fileName.name == "gdut-drcom.dll")
                        {
                            if (DownloadFile(fileName.browser_download_url, "gdut-drcom.dll"))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            //mirror
            client = new RestClient("https://api.github.com");
            // todo:
            // 临时测试,注意在发行的时候改正
            request = new RestRequest("/repos/chenhaowen01/gdut-drcom/git/trees/abd65a02a4cb2cabcb40faff635744647daf57a7");
            response = client.Execute<GithubReleaseResponse>(request);

            if (response != null &&
                response.Content != "" &&
                response.ResponseStatus == ResponseStatus.Completed)
            {
                if (response.Data.assets != null)
                    foreach (GithubReleaseAssetItem fileName in response.Data.assets)
                    {
                        // todo:
                        // 临时测试,注意在发行的时候改正
                        if (fileName.name == "gdut-drcom.dll.1.6.3")
                        {
                            if (DownloadFile(fileName.browser_download_url, "gdut-drcom.dll"))
                            {
                                return true;
                            }
                        }
                    }
            }

            return false;
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
                //换用using
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    stream.Write(result, 0, result.Length);
                }
            }
            catch (Exception e)
            {
                Log4Net.WriteLog("下载心跳包失败: " + e.Message, e);
                return false;
            }

            return true;
        }


    }

    public class GithubReleaseResponse
    {
        //todo: 有点想改命名
        //改完记得用DeserializeAs修正序列化问题
        public string tag_name { get; set; }
        public string name { get; set; }
        public GithubReleaseAssetItem[] assets { get; set; }
    }

    public class GithubReleaseAssetItem
    {
        public string name { get; set; }
        public string browser_download_url { get; set; }
    }
}
