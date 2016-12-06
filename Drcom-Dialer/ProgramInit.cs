using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drcom_Dialer
{
    internal static class ProgramInit
    {
        [STAThread]
        private static void Main(string[] args)
        {
            //进行必要的初始化工作
            Model.Utils.Log4Net.SetConfig();
            Model.DialerConfig.Init();
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case Model.Utils.VPNFixer.StartupArgs:
                         Model.Utils.VPNFixer.AddRouteRule();
                         return;
                    default:
                        Model.Utils.Log4Net.WriteLog("未知的启动参数: " + args);
                        break;
                }
            }

            if (Model.DialerConfig.isFixVPN)
            {
                Model.Utils.VPNFixer.Elevate();
            }

            Model.PPPoE.Init();
            Model.Dial.Init();

            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
