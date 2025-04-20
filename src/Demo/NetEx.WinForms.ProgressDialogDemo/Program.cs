using NetExDemo.ProgressDialog;
using System;
using System.Windows.Forms;

namespace NetEx.WinForms.ProgressDialogDemo
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
#if NETFRAMEWORK
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#else
            ApplicationConfiguration.Initialize();
#endif
            Application.Run(new ProgressDialogForm());
        }
    }
}