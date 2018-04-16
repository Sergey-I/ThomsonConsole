using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomsonConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var appDirectory = Directory.GetCurrentDirectory();
            var FilesSourceDirectory = appDirectory + FolderOfSourcefiles;
            var UnzipDestinatinationDirectory = appDirectory + FolderForUnzipFiles;
            Console.WriteLine($"Searching zip files in {FilesSourceDirectory}");

            List<string> zipFiles = FilesTreatment.FindAllZipFiles(FilesSourceDirectory);

            var filesCount = 0;
            if (zipFiles != null)
            {
                Directory.Delete(UnzipDestinatinationDirectory, true);
                foreach (string zipFile in zipFiles)
                {
                    string fileName = Path.GetFileName(zipFile);
                    string fileDirectory = Path.GetDirectoryName(zipFile);
                    Console.WriteLine($"{fileName,-15} : {fileDirectory}");
                    string destination = UnzipDestinatinationDirectory + 
                        fileDirectory.Substring(FilesSourceDirectory.Length) + @"\" + 
                        Path.GetFileNameWithoutExtension(zipFile);
                    ZipFile.ExtractToDirectory(zipFile, destination);
                    List<string> csvFiles = FilesTreatment.FindAllCSVFiles(destination);
                    foreach (string cvsfile in csvFiles)
                    {
                        Console.WriteLine($"{Path.GetFileName(cvsfile),-15} : {Path.GetDirectoryName(cvsfile)}");
                    }
                }
                filesCount = zipFiles.Count;
            }
            Console.WriteLine($"   {filesCount} zip files found");
            Console.ReadLine();

            
            
        }
        const string FolderForUnzipFiles = @"\Temp\";
        const string FolderOfSourcefiles = @"\Thomson\";
    }
}
