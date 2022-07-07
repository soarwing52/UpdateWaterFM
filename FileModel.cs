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

    public class FileToUpdate : IFile
    {
        static string WaterFMDir = @"C:\Ximple\WaterFMEntry";
        static string updateDir = Path.Combine(WaterFMDir, "Updates");
        static string sourceDir;

        private string _sourceFileName;
        private string _version;

        public static Dictionary<string, string> _fileMapping = new Dictionary<string, string>();

        public FileToUpdate(string sourceFileName, int version, Dictionary<string, string> fileMapping)
        {
            _sourceFileName = sourceFileName;
            _version = Convert.ToString(version);
            _fileMapping = fileMapping;
            sourceDir = Path.Combine(updateDir, _version);
        }

        public void copyToUpdate()
        {
            string targetPath;
            var x = Path.GetFileName(_sourceFileName);
            if (!_fileMapping.TryGetValue(x, out targetPath))
                return;
            string source = Path.Combine(sourceDir, _sourceFileName);
            string copyToPath = Path.Combine(WaterFMDir, targetPath);
            File.Copy(source, copyToPath, true);
        }
    }
}
