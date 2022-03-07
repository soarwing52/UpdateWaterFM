using System;
using System.Collections.Generic;

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
            List<int> availableVersions = application.GetAvailableVersions();

            foreach (int version in availableVersions)
            {
                if (version <= currentVersion)
                    continue;

                application.downloadFiles(version);
                application.updateFile(version);
                application.updateVersion(version);
            }
        }
    }
}
