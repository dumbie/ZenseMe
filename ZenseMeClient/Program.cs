using System;
using System.Windows.Forms;
using ZenseMe.Client.Forms;

namespace ZenseMe.Client
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StartupCheck StartupCheck = new StartupCheck();
            Application.Run(new Main());
        }
    }
}