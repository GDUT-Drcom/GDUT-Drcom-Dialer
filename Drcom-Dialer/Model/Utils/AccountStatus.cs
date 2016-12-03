using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using RestSharp;

namespace Drcom_Dialer.Model.Utils
{
    /// <summary>
    ///     账户信息查询
    /// </summary>
    internal static class AccountStatus
    {
        public static AccountInfomation GetAccountInfomation()
        {
            RestClient client = new RestClient("http://222.200.98.8:1800");
            client.CookieContainer = new System.Net.CookieContainer();
            client.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:50.0) Gecko/20100101 Firefox/50.0";

            RestRequest indexRequest = new RestRequest("/Self/nav_login");
            IRestResponse indexResponse = client.Execute(indexRequest);

            Match match = Regex.Match(indexResponse.Content, @"var checkcode=""([0-9]*)""");

            string code = match.Groups[1].Value;

            RestRequest authRequest = new RestRequest("/Self/LoginAction.action", Method.POST);
            authRequest.AddParameter("account", DialerConfig.username);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] mdResult = md5.ComputeHash(Encoding.Default.GetBytes(DialerConfig.password));
            authRequest.AddParameter("password", BmobAnalyze.ToHexString(mdResult, mdResult.Length));
            authRequest.AddParameter("code", "");
            authRequest.AddParameter("checkcode", code);
            authRequest.AddParameter("Submit", "登录");

            client.Execute(new RestRequest("Self/RandomCodeAction.action"));

            IRestResponse authResponse = client.Execute(authRequest);

            RestRequest infoRequest = new RestRequest("Self/refreshaccount");
            IRestResponse<AccountInfomation> infoResponse = client.Execute<AccountInfomation>(infoRequest);

            return infoResponse.Data;
        }


        public static void AccountInfo()
        {
            AccountInfomation accInfo = GetAccountInfomation();
            Log4Net.WriteLog(accInfo.note.overdate);
        }

        /// <summary>
        /// 账户信息
        /// </summary>
        public class AccountInfomation
        {
            /// <summary>
            ///     神tm date，此处应为状态
            /// </summary>
            public string date;

            /// <summary>
            ///     应该是通知
            /// </summary>
            public string outmessage;

            /// <summary>
            ///     服务器日期
            /// </summary>
            public string serverDate;

            /// <summary>
            ///     详细信息
            /// </summary>
            public AccountNote note;

            public class AccountNote
            {
                /// <summary>
                ///     未知
                /// </summary>
                public string leftFlow;
                /// <summary>
                ///     剩余上网时间（此处为空）
                /// </summary>
                public string leftTime;
                /// <summary>
                ///     余额
                /// </summary>
                public string leftmoeny;
                /// <summary>
                ///     在线状态
                /// </summary>
                public int onlinestate;
                /// <summary>
                ///     到期时间
                /// </summary>
                public string overdate;
                /// <summary>
                ///     校园网服务内容
                /// </summary>
                public string service;
                /// <summary>
                ///     当前状态
                /// </summary>
                public string status;
                /// <summary>
                ///     用户账户
                /// </summary>
                public string welcome;

            }
        }
    }
}
