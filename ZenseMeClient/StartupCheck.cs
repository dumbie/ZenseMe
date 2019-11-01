using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ZenseMe.Client
{
    class StartupCheck
    {
        public StartupCheck()
        {
            if (Process.GetProcessesByName("ZenseMe").Length > 1)
            {
                MessageBox.Show("ZenseMe is already running.", "ZenseMe");
                Environment.Exit(1);
            }

            if (Process.GetProcessesByName("Zune").Length == 1)
            {
                MessageBox.Show("Please close Zune before launching ZenseMe.", "ZenseMe");
                Environment.Exit(1);
            }

            if (Process.GetProcessesByName("songbird").Length == 1)
            {
                MessageBox.Show("Please close Songbird before launching ZenseMe.", "ZenseMe");
                Environment.Exit(1);
            }

            if (Process.GetProcessesByName("MediaGo").Length == 1)
            {
                MessageBox.Show("Please close Media Go before launching ZenseMe.", "ZenseMe");
                Environment.Exit(1);
            }

            if (Process.GetProcessesByName("MediaMonkey").Length == 1)
            {
                MessageBox.Show("Please close Media Monkey before launching ZenseMe.", "ZenseMe");
                Environment.Exit(1);
            }

            if (!File.Exists("./Resources/Interop.PortableDeviceApiLib.dll"))
            {
                MessageBox.Show("File: Resources\\Interop.PortableDeviceApiLib.dll could not be found.", "ZenseMe");
                Environment.Exit(1);
            }

            if (!File.Exists("./Resources/Interop.PortableDeviceTypesLib.dll"))
            {
                MessageBox.Show("File: Resources\\Interop.PortableDeviceTypesLib.dll could not be found.");
                Environment.Exit(1);
            }

            if (!File.Exists("./Resources/System.Data.SQLite.dll"))
            {
                MessageBox.Show("File: Resources\\System.Data.SQLite.dll could not be found.", "ZenseMe");
                Environment.Exit(1);
            }

            if (!File.Exists("./Resources/ZenseMeResources.dll"))
            {
                MessageBox.Show("File: Resources\\ZenseMeResources.dll could not be found.", "ZenseMe");
                Environment.Exit(1);
            }

            if (!File.Exists("ZenseMe.exe.config"))
            {
                MessageBox.Show("File: ZenseMe.exe.config could not be found.", "ZenseMe");
                Environment.Exit(1);
            }
        }
    }
}