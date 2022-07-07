using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UpdateWaterFM
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateApp application = new UpdateApp();
            if (!application.checkEnvironment())
                return;

            int currentVersion = application.checkVersion();
            application.downloadConfigFiles();
            List<int> availableVersions = application.GetAvailableVersions();
            int OfficialVersion = application.getOfficialVersion();
            
            foreach (int version in availableVersions)
            {
                // 如果是降版本 就是從第一版重新更新到指定版本
                if (currentVersion > OfficialVersion)
                    currentVersion = 0;

                // 版本超過的不更新
                if (version > OfficialVersion)
                    break;

                if (version <= currentVersion)
                    continue;

                application.downloadFiles(version);
                application.updateFile(version);
                application.updateVersion(version);
            }

            LaunchCommandLineApp();
        }

        /// <summary>
        /// Launch the application with some options set.
        /// </summary>
        static void LaunchCommandLineApp()
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = @"C:\Ximple\WaterFMEntry\bin\WaterFMEntry.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "-U";

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                // Log error.
            }
            //System.Environment.Exit(1);
        }
    }
}
