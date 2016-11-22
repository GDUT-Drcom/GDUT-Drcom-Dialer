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
    static class PPPoE
    {
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
                    RasDevice device = RasDevice.GetDevices().Where(o => o.DeviceType == RasDeviceType.PPPoE).First();
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
        public static bool Dial(string username, string password)
        {
            return Dial(username, password, Properties.Resources.RasConnectionName);
        }
        /// <summary>
        /// 拨号
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="connectName">连接名</param>
        public static bool Dial(string username, string password, string connectName)
        {
            try
            {
                RasDialer dialer = new RasDialer();
                dialer.EntryName = connectName;
                dialer.PhoneNumber = " ";
                dialer.AllowUseStoredCredentials = true;
                dialer.PhoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User);
                dialer.Credentials = new System.Net.NetworkCredential(username, password);
                dialer.Timeout = 1000;

                RasHandle hRas = dialer.Dial();

                while (hRas.IsInvalid)
                {
                    //TODO: Add code here
                }
                if (!hRas.IsInvalid)
                {
                    foreach (RasConnection conn in RasConnection.GetActiveConnections())
                    {
                        if (conn.Handle == hRas)
                        {
                            RasIPInfo ipAddr = (RasIPInfo)conn.GetProjectionInfo(RasProjectionType.IP);
                            //ipAddr.IPAddress.ToString();
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
            }
            return false;
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
                }
            }
            catch (Exception e)
            {
                Utils.Log4Net.WriteLog(e.Message, e);
            }
        }

        public static EventHandler PPPoEConnectSuccess;
    }
}