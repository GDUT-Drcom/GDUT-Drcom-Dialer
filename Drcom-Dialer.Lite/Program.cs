using Drcom_Dialer;
using System;
using System.Windows.Forms;

namespace Drcom_Dialer.Lite
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!Initializer.Initialize(args))
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LiteWindow());
        }
    }
}
