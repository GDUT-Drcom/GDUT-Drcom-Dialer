using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Deployment.Application;

namespace Drcom_Dialer.Model.Utils
{
    class Version
    {
        public static string getVersion()
        {
            try
            {
                ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            catch (Exception e)
            {
                Log4Net.WriteLog(e.Message, e);
            }
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
