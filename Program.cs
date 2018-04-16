using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomsonConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var startDirectory = Directory.GetCurrentDirectory() + @"\Thomson";
            Console.WriteLine($"Searching zip files in {startDirectory}");
            List<string> zipFiles = FilesSearch.FindAllZipFiles(startDirectory);
            if (zipFiles == null || zipFiles.Count == 0)
            {
                Console.WriteLine("   No zip files found");
            }
            else
            {
                foreach (string file in zipFiles)
                {
                    FileInfo f = new FileInfo(file);
                    Console.WriteLine($"{f.Name,-15} : {f.Directory}");
                }
                Console.WriteLine($"   {zipFiles.Count + 1} zip files found");
            }
            Console.ReadLine();
        }
    }
}
