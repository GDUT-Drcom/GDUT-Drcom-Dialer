using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Security.Cryptography;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    /// 运行统计类
    /// </summary>
    internal static class BmobAnalyze
    {
        /// <summary>
        /// 发送运行统计
        /// </summary>
        public static void SendAnalyze()
        {
            RestClient client = new RestClient("https://api.bmob.cn");

            AnalyzeData data = new AnalyzeData();
            if (!DialerConfig.isFeedback)
            {
                data.Username = "Null";
            }
            else
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] mdResult = md5.ComputeHash(Encoding.Default.GetBytes(DialerConfig.username));
                data.Username = ToHexString(mdResult, mdResult.Length);
            }
            
            if (DialerConfig.guid == "")
            {
                RestRequest request = new RestRequest("/1/classes/analyze",Method.POST); //post
                request.AddHeader("X-Bmob-Application-Id", "6402f2d047a6728395bc8df1f918b340");
                request.AddHeader("X-Bmob-REST-API-Key", "a0069d330057d662b7a1af4245e8f882");
                request.AddJsonBody(data);
                IRestResponse<ObjId> objId = client.Execute<ObjId>(request);
                if(objId.ResponseStatus == ResponseStatus.Completed)
                {
                    DialerConfig.guid = objId.Data.objectId;
                    DialerConfig.SaveConfig();
                }
            }
            else
            {
                //update
                RestRequest request = new RestRequest("/1/classes/analyze/{id}", Method.PUT);
                request.AddHeader("X-Bmob-Application-Id", "6402f2d047a6728395bc8df1f918b340");
                request.AddHeader("X-Bmob-REST-API-Key", "a0069d330057d662b7a1af4245e8f882");
                request.AddParameter("id", DialerConfig.guid);
                request.AddJsonBody(data);
                client.Execute(request);
            }

        }
        /// <summary>
        /// HEX转字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes, int count)
        {
            string byteStr = string.Empty;
            if (bytes != null )
            {
                //int i = 0;
                for (int i = 0; i < bytes.Length; i++)
                {
                    var item = bytes[i];
                    //i++;
                    byteStr += $"{item:x2}";
                    if (i >= count)
                    {
                        break;
                    }
                }
            }
            return byteStr;
        }
    }


    internal class AnalyzeData
    {
        public string Username;
    }

    internal class ObjId
    {
        public string objectId;
    }
}
