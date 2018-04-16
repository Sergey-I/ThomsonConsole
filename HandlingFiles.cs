using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace ThomsonConsole
{
    class HandlingFiles
    {
        static HandlingFiles()
        {
            AppDirectory = SearchFiles.AppDirectory;
            TemporaryDirectory = AppDirectory + FolderForUnzipFiles;
        }

        static public string AppDirectory { get; }
        static public string TemporaryDirectory { get; }

        const string FolderForUnzipFiles = @"\Temp\";

        public void ClearTempDiectory

        static void ClearDirectory(string directoryName)
        {
            if (Directory.Exists(directoryName))
            {
                Directory.Delete(directoryName, true);
            }
        }

        public static List<string> UnZipCVSFiles(IList<string> zipFiles)
        {
            List<string> result = new List<string>();
            if (zipFiles != null)
            {
                ClearDirectory(TemporaryDirectory);
                foreach (string zipFile in zipFiles)
                {
                    result.AddRange(UnZipCVSFiles(zipFile));
                }
            }
            return result;
        }

        static List<string> UnZipCVSFiles(string zipFile)
        {
            var unZipDirectory = GetUnzipDirectory(zipFile);
            ClearDirectory(unZipDirectory);
            ZipFile.ExtractToDirectory(zipFile, unZipDirectory);

            return SearchFiles.FindAllCSVFiles(unZipDirectory);
        }

        private static string GetUnzipDirectory(string zipFile)
        {
            string zipFileDirectory = Path.GetDirectoryName(zipFile);
            string ParthOfPathToZipfile = "";
            if (zipFileDirectory.Length > SearchFiles.FilesSourceDirectory.Length)
            {
                ParthOfPathToZipfile = zipFileDirectory.Substring(SearchFiles.FilesSourceDirectory.Length);
            }

            return TemporaryDirectory + ParthOfPathToZipfile + @"\" + Path.GetFileNameWithoutExtension(zipFile);
            
        }

        public static void HandleCSVFiles(IList<string> csvFiles)
        {
            if (csvFiles != null)
            {
                foreach (string cvsFile in csvFiles)
                {
                    Console.WriteLine($"{Path.GetFileName(cvsFile),-15} : {Path.GetDirectoryName(cvsFile)}");
                }
            }
        }
    }
}
