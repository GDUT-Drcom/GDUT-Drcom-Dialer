using NETCONLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Drcom_Dialer.Model.Utils
{
    class NetSharing
    {
        private NetSharingManager nsm;

        public NetSharing()
        {
            nsm = new NetSharingManager();
        }

        public bool Start()
        {
            try
            {
                INetConnection dialer = GetDialerConnection();
                INetConnection wifi = GetAPConnection();
                if (dialer == null || wifi == null)
                    return false;

                INetSharingConfiguration wconf = nsm.INetSharingConfigurationForINetConnection[wifi];
                Disable_ICS_WMI(false);
                if (wconf.SharingEnabled)
                    wconf.DisableSharing();
                wconf.EnableSharing(tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PRIVATE);

                INetSharingConfiguration dconf = nsm.INetSharingConfigurationForINetConnection[dialer];
                Disable_ICS_WMI(true);
                if (dconf.SharingEnabled)
                    dconf.DisableSharing();
                // System.AccessViolationException???
                dconf.EnableSharing(tagSHARINGCONNECTIONTYPE.ICSSHARINGTYPE_PUBLIC);
            }
            catch (Exception e)
            {
                Log4Net.WriteLog("Enable sharing wifi failed", e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/10098419/inetsharingconfiguration-enablesharing-icssharingtype-public-returns-0x80040
        /// </summary>
        public void Disable_ICS_WMI(bool pub)
        {
            string pname = pub ? "IsIcsPublic" : "IsIcsPrivate";

            ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\Microsoft\\HomeNet");

            //create object query
            ObjectQuery query = new ObjectQuery("SELECT * FROM HNet_ConnectionProperties ");

            //create object searcher
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            //get a collection of WMI objects
            ManagementObjectCollection queryCollection = searcher.Get();

            //enumerate the collection.
            foreach (ManagementObject m in queryCollection)
            {
                // access properties of the WMI object
                Console.WriteLine("Connection : {0}", m["Connection"]);
                try
                {
                    PropertyDataCollection properties = m.Properties;
                    foreach (PropertyData prop in properties)
                    {
                        if (prop.Name == pname && ((Boolean)prop.Value) == true)
                        {
                            prop.Value = false;
                            m.Put();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ex " + e.Message);
                    continue;
                }
            }
        }

        private INetConnection GetDialerConnection()
        {
            foreach (INetConnection conn in nsm.EnumEveryConnection)
            {
                var props = nsm.NetConnectionProps[conn];
                Console.WriteLine(props.Name + "," + props.MediaType);
                if (props.Name == Properties.Resources.RasConnectionName &&
                    props.Status == tagNETCON_STATUS.NCS_CONNECTED)
                    return conn;
            }
            return null;
        }

        private INetConnection GetAPConnection()
        {
            foreach (INetConnection conn in nsm.EnumEveryConnection)
            {
                var props = nsm.NetConnectionProps[conn];
                Console.WriteLine(props.Name);
                if (props.Name == "Wi-Fi")
                    return conn;
            }
            return null;
        }
    }
}
