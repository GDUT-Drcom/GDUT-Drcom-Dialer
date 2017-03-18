using System;

namespace Drcom_Dialer
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (!Initializer.Initialize(args))
                return;

            try
            {
                App app = new App();
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception e)
            {
                Model.Utils.Log4Net.WriteLog("顶级异常", e);
            }
        }
    }
}
