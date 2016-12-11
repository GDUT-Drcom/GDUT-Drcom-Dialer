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
        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <returns></returns>
        public static AccountInfomation GetAccountInfomation()
        {
            try
            {
                //构造Cookie
                RestClient client = new RestClient("http://222.200.98.8:1800");
                client.CookieContainer = new System.Net.CookieContainer();
                client.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:50.0) Gecko/20100101 Firefox/50.0";

                //获取认证码
                RestRequest indexRequest = new RestRequest("/Self/nav_login");
                IRestResponse indexResponse = client.Execute(indexRequest);

                Match match = Regex.Match(indexResponse.Content, @"var checkcode=""([0-9]*)""");

                string code = match.Groups[1].Value;

                //拉取验证码
                client.Execute(new RestRequest("Self/RandomCodeAction.action"));

                //构造认证包
                RestRequest authRequest = new RestRequest("/Self/LoginAction.action", Method.POST);
                authRequest.AddParameter("account", DialerConfig.username);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] mdResult = md5.ComputeHash(Encoding.Default.GetBytes(DialerConfig.password));
                authRequest.AddParameter("password", BmobAnalyze.ToHexString(mdResult, mdResult.Length));
                authRequest.AddParameter("code", "");
                authRequest.AddParameter("checkcode", code);
                authRequest.AddParameter("Submit", "登录");

                //认证
                IRestResponse authResponse = client.Execute(authRequest);

                //获取账户信息
                RestRequest infoRequest = new RestRequest("Self/refreshaccount");
                IRestResponse<accountInfomation> infoResponse = client.Execute<accountInfomation>(infoRequest);

                //转换数据格式
                AccountInfomation acci = new AccountInfomation();

                if (infoResponse.Data == null)
                {
                    acci.Status = "fail";
                    return acci;
                }
                acci.Status = infoResponse.Data.date;
                acci.Username = infoResponse.Data.note.welcome;
                acci.Service = infoResponse.Data.note.service;
                acci.LeftMoney = infoResponse.Data.note.leftmoeny;

                match = Regex.Match(infoResponse.Data.note.overdate, @"([0-9]*)-([0-9]*)-([0-9]*)");
                DateTime overDate = new DateTime(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value));

                acci.OverDate = overDate;
                return acci;

            }
            catch(Exception e)
            {
                Log4Net.WriteLog(e.Message, e);
                AccountInfomation acci = new AccountInfomation();
                acci.Status = "fail";
                return acci;
            }
        }

        /// <summary>
        /// 账户信息相关功能
        /// </summary>
        public static void AccountInfo()
        {
            AccountInfomation accInfo = GetAccountInfomation();
            if(accInfo != null && accInfo.Status == "success")
            {
                if (DialerConfig.isNotifyWhenExpire)
                {
                    DateTime overDate = accInfo.OverDate;
                    TimeSpan left = overDate.Subtract(DateTime.Today);
                    if (left.TotalDays <= 7)
                    {
                        ViewModel.ViewModel.View.ShowBalloonTip(5000, "提示", "校园网账户将于" +
                            overDate.ToString("yyyy-MM-dd") + "过期，请尽快充值");
                    }
                }

                if(DialerConfig.zone == DialerConfig.Campus.Unknown)
                {
                    if (accInfo.Service.Contains("大学城"))
                    {
                        DialerConfig.zone = DialerConfig.Campus.HEMC;
                    }
                    else if (accInfo.Service.Contains("东风路"))
                    {
                        DialerConfig.zone = DialerConfig.Campus.DongfengRd;
                    }
                    else if (accInfo.Service.Contains("龙洞"))
                    {
                        DialerConfig.zone = DialerConfig.Campus.LongDong;
                    }
                    else if (accInfo.Service.Contains("番禺"))
                    {
                        DialerConfig.zone = DialerConfig.Campus.Panyu;
                    }
                    else
                    {
                        Log4Net.WriteLog("无法匹配的校区字符串：" + accInfo.Service);
                    }
                }
            }
        }

        /// <summary>
        /// 账户信息
        /// </summary>
        private class accountInfomation
        {
            /// <summary>
            ///     神tm date，此处应为状态
            /// </summary>
            public string date { get; set; }

            /// <summary>
            ///     详细信息
            /// </summary>
            public accountNote note { get; set; }

            /// <summary>
            ///     应该是通知
            /// </summary>
            public string outmessage { get; set; }

            /// <summary>
            ///     服务器日期
            /// </summary>
            public string serverDate { get; set; }

        }
        private class accountNote
        {
            /// <summary>
            ///     未知
            /// </summary>
            public string leftFlow { get; set; }
            /// <summary>
            ///     剩余上网时间（此处为空）
            /// </summary>
            public string leftTime { get; set; }
            /// <summary>
            ///     余额
            /// </summary>
            public string leftmoeny { get; set; }
            /// <summary>
            ///     在线状态
            /// </summary>
            public int onlinestate { get; set; }
            /// <summary>
            ///     到期时间
            /// </summary>
            public string overdate { get; set; }
            /// <summary>
            ///     校园网服务内容
            /// </summary>
            public string service { get; set; }
            /// <summary>
            ///     当前状态
            /// </summary>
            public string status { get; set; }
            /// <summary>
            ///     用户账户
            /// </summary>
            public string welcome { get; set; }

        }

        /// <summary>
        ///     账户信息
        /// </summary>
        public class AccountInfomation
        {
            /// <summary>
            /// 返回状态
            /// </summary>
            public string Status;
            /// <summary>
            /// 用户名
            /// </summary>
            public string Username;
            /// <summary>
            /// 剩余金额
            /// </summary>
            public string LeftMoney;
            /// <summary>
            /// 到期日期
            /// </summary>
            public DateTime OverDate;
            /// <summary>
            /// 校园网服务内容
            /// </summary>
            public string Service;

        }
    }
}
