using System;
using System.IO;
using System.Collections.Generic;

namespace UpdateWaterFM
{
        public interface IFile
        {
            public void downloadFromFtp()
            {
                throw new NotImplementedException();
            }

            public void proccessFile()
            {
                throw new NotImplementedException();
            }

        public void copyToUpdate()
            {
                throw new NotImplementedException();
            }
        }

        public class FileToUpdate: IFile
        {
        static string WaterFMDir = @"C:\Ximple\WaterFMEntry";
        static string updateDir = Path.Combine(WaterFMDir, "Updates");
        static string sourceDir;

        private string _sourceFileName;
        private string _version;

        public static Dictionary<string, string> fileMapping = new Dictionary<string, string>()
        {
            {"config.xml", "config.xml" },
            {"ToolBar.xml", "ToolBar.xml" },
            {"8030111.png",  @"Icons\MainForm\8030111.png"},
            {"Asset.txt",  @"IniReport\財產統計表.xlsx"},
            {"giveWater.txt", @"IniReport\給水改裝明細表.xlsx"},
            {"TWD_Cell.cel", @"WorkSpace\Projects\TaipeiWater\cell\TWD_Cell.cel"},
            {"WaterFMEntry.exe", @"bin\WaterFMEntry.exe"},
            {"WaterFMEntry.exe.config", @"bin\WaterFMEntry.exe.config"},
            {"Ximple.Water.Addins.dll", @"bin\Ximple.Water.Addins.dll"},
            {"Ximple.Water.Addins.dll.config", @"bin\Ximple.Water.Addins.dll.config"},
            {"Ximple.Water.Addins.pdb", @"bin\Ximple.Water.Addins.pdb" },
            {"features.xml", "WorkSpace/Projects/TaipeiWater/xml/features/features.xml" },
            {"criteria.xml", "WorkSpace/Projects/TaipeiWater/xml/criteria/criteria.xml" }
        };

        public FileToUpdate(string sourceFileName, int version)
        {
            _sourceFileName = sourceFileName;
            _version = Convert.ToString(version);
            sourceDir = Path.Combine(updateDir, _version);
        }

        public void copyToUpdate()
        {
            string targetPath;
            var x = Path.GetFileName(_sourceFileName);
            if (!fileMapping.TryGetValue(x, out targetPath))
                return;
            string source = Path.Combine(sourceDir, _sourceFileName);
            string copyToPath = Path.Combine(WaterFMDir, targetPath);
            File.Copy(source, copyToPath, true);
        }
     }
}
