﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using UpdateWaterFM.Models;

namespace UpdateWaterFM
{
    class UpdateApp
    {
        static string WaterFMDir = @"C:\Ximple\WaterFMEntry";
        static string updateDir = Path.Combine(WaterFMDir, "Updates");
        static string sourceftp = @"ftp://10.68.127.164/WaterFMUpdate/";
        static string versionFileName = Path.Combine(updateDir, "version.txt");
        static string ftpAccount = "water";
        static string ftpPassword = "water";
        static string fileMappingFileName = "fileMapping.json";
        static string updateConfigFileName = "updateConfig.json";
        public bool checkEnvironment()
        {
            if (!Directory.Exists(WaterFMDir))
                return false;

            Directory.CreateDirectory(updateDir);

            return true;
        }

        public int checkVersion()
        {

            if (!File.Exists(versionFileName))
            {
                using (FileStream fs = File.Create(versionFileName))
                {
                    Byte[] title = new UTF8Encoding(true).GetBytes("0\r\n");
                    fs.Write(title, 0, title.Length);
                }
                return 0;
            }
   
            using (StreamReader sr = File.OpenText(versionFileName))
            {
                string s;
                int result;
                while ((s = sr.ReadLine()) != null)
                {
                    if(Int32.TryParse(s, out result))
                        return result;
                }
               return 0;
            }
        }

        public void updateVersion(int version)
        {
            try
            {
                using (FileStream fs = new FileStream(versionFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                {
                    StreamWriter write = new StreamWriter(fs);
                    write.Write($"{version.ToString()}\r\n");
                    write.Flush();
                    write.Close();
                    fs.Close();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<string> GetFileNamesFromFtp(string location)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(location);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential(ftpAccount, ftpPassword);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string names = reader.ReadToEnd();

            reader.Close();
            response.Close();

            return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public List<int> GetAvailableVersions()
        {
            List<string> fileNames = GetFileNamesFromFtp(sourceftp);
            List<int> availableVersions = new List<int>();

            foreach (string item in fileNames)
            {
                int value;
                bool success = Int32.TryParse(item, out value);
                if (success)
                    availableVersions.Add(value);
            }

            availableVersions.Sort();

            return availableVersions;
        }

        public void downloadConfigFiles()
        {
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("water", "water");
                client.DownloadFile($"{sourceftp}/{fileMappingFileName}", $"{updateDir}/{fileMappingFileName}");
                client.DownloadFile($"{sourceftp}/{updateConfigFileName}", $"{updateDir}/{updateConfigFileName}");
            }
        }

        public void downloadFiles(int version)
        {
            try
            {
                string updatePackagePath = Path.Combine(sourceftp, version.ToString());
                List<string> files = GetFileNamesFromFtp(updatePackagePath);
                Directory.CreateDirectory($"{updateDir}/{version}");
                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential("water", "water");
                    foreach (string item in files)
                        client.DownloadFile($"{sourceftp}/{version}/{item}", $"{updateDir}/{version}/{item}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void updateFile(int version)
        {
            Console.WriteLine("更新檔案：");
            string versionPath = Path.Combine(updateDir, Convert.ToString(version));
            Dictionary<string, string> fileMapping = getFileMapping();
            foreach (string file in Directory.GetFiles(versionPath))
            {
                Console.WriteLine(file);
                FileToUpdate fileToUpdate = new FileToUpdate(file, version, fileMapping);
                fileToUpdate.copyToUpdate();
            }
        }

        public Dictionary<string, string> getFileMapping()
        {
            Dictionary<string, string> fileMapping = new Dictionary<string, string>();

            string MappingJson = Path.Combine(updateDir,"fileMapping.json");
            StreamReader r = new StreamReader(MappingJson);
            string jsonString = r.ReadToEnd();
            List<FileMapping> mappings = JsonConvert.DeserializeObject<List<FileMapping>>(jsonString);
            foreach (var item in mappings)
            {
                Console.WriteLine(item.Path);
                fileMapping[item.Name] = item.Path;
            }
            return fileMapping;
        }

        public int getOfficialVersion()
        {
            Dictionary<string, string> fileMapping = new Dictionary<string, string>();

            string ConfigJson = Path.Combine(updateDir, "updateConfig.json");
            StreamReader r = new StreamReader(ConfigJson);
            string jsonString = r.ReadToEnd();
            Config mappings = JsonConvert.DeserializeObject<Config>(jsonString);
            return mappings.Version;
        }
    }
}
