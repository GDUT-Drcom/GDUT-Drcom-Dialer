using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotRas;

namespace Drcom_Dialer.Model
{
    /// <summary>
    /// PPPoE拨号器
    /// </summary>
    internal static class PPPoE
    {
        public static void Init()
        {
            Init(Properties.Resources.RasConnectionName);
        }

        /// <summary>
        /// 初始化PPPoE拨号器
        /// </summary>
        /// <param name="connectName">连接名称</param>
        public static void Init(string connectName)
        {
            try
            {
                RasDialer dialer = new RasDialer();
                RasPhoneBook phoneBook = new RasPhoneBook();
                string path = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User);
                phoneBook.Open(path);

                if (phoneBook.Entries.Contains(connectName))
                {
                    phoneBook.Entries[connectName].PhoneNumber = " ";
                    phoneBook.Entries[connectName].Update();
                }
                else
                {
                    string adds = string.Empty;
                    System.Collections.ObjectModel.ReadOnlyCollection<RasDevice> readOnlyCollection = RasDevice.GetDevices();
                    RasDevice device = RasDevice.GetDevices().First(o => o.DeviceType == RasDeviceType.PPPoE);
                    RasEntry entry = RasEntry.CreateBroadbandEntry(connectName, device);
                    entry.PhoneNumber = " ";
                    phoneBook.Entries.Add(entry);
                }
            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
            }

        }

        /// <summary>
        /// 拨号
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        public static void Dial(string username, string password)
        {
            Dial(username, password, Properties.Resources.RasConnectionName);
        }
        /// <summary>
        /// 拨号
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="connectName">连接名</param>
        private static void Dial(string username, string password, string connectName)
        {
            try
            {
                RasDialer dialer = new RasDialer
                {
                    EntryName = connectName,
                    PhoneNumber = " ",
                    AllowUseStoredCredentials = true,
                    PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User),
                    Credentials = new System.Net.NetworkCredential(username, password),
                    Timeout = 1000
                };

                RasHandle hRas = dialer.Dial();

                //连接失败
                //while (hRas.IsInvalid)
                //{
                //    //TODO: Add code here
                //    //继续连接
                //}

                if (!hRas.IsInvalid)
                {
                    foreach (RasConnection temp in RasConnection.GetActiveConnections())
                    {
                        if (temp.Handle == hRas)
                        {
                            RasIPInfo ipAddr = (RasIPInfo)temp.GetProjectionInfo(RasProjectionType.IP);
                            //
                            PPPoEDialSuccessEvent?.Invoke(null, new Msg(ipAddr.IPAddress.ToString()));
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
                PPPoEDialFailEvent?.Invoke(null, new Msg(e.Message));
            }
        }
        /// <summary>
        /// 断开所有连接
        /// </summary>
        public static void Hangup()
        {
            try
            {
                foreach (RasConnection conn in RasConnection.GetActiveConnections())
                {
                    conn.HangUp();
                    PPPoEHangupSuccessEvent?.Invoke(null, null);
                }
            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
                PPPoEHangupFailEvent?.Invoke(null, new Msg(e.Message));
            }
        }
        /// <summary>
        /// PPPoE拨号成功事件
        /// </summary>
        public static EventHandler<Msg> PPPoEDialSuccessEvent;
        /// <summary>
        /// PPPoE拨号失败事件
        /// </summary>
        public static EventHandler<Msg> PPPoEDialFailEvent;
        /// <summary>
        /// PPPoE挂断成功事件
        /// </summary>
        public static EventHandler PPPoEHangupSuccessEvent;
        /// <summary>
        /// PPPoE挂断失败事件
        /// </summary>
        public static EventHandler<Msg> PPPoEHangupFailEvent;


    }

    /// <summary>
    /// 简单的消息传输器
    /// </summary>
    public class Msg : EventArgs
    {
        public string Message
        {
            set;
            get;
        }
        public Msg(string _msg)
        {
            Message = _msg;
        }
    }
}
