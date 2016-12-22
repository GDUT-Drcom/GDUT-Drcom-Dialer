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

            App app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
