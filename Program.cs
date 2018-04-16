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
                foreach (string file in zipFiles)
                {
                    string fileName = Path.GetFileName(file);
                    string fileDirectory = Path.GetDirectoryName(file);
                    Console.WriteLine($"{fileName,-15} : {fileDirectory}");
                    string destination = UnzipDestinatinationDirectory + 
                        fileDirectory.Substring(FilesSourceDirectory.Length) + @"\" + 
                        Path.GetFileNameWithoutExtension(file);
                    Console.WriteLine($"{fileName,-15} :will be unzip to {destination}");
                    ZipFile.ExtractToDirectory(file, destination);
                }
                filesCount = zipFiles.Count + 1;
            }
            Console.WriteLine($"   {filesCount} zip files found");
            Console.ReadLine();

            
            
        }
        const string FolderForUnzipFiles = @"\Temp\";
        const string FolderOfSourcefiles = @"\Thomson\";
    }
}
