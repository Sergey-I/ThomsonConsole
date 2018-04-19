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
            var zipFiles = SearchFilesService.FindAllZipFiles();

            var countZipFiles = zipFiles == null ? 0 : zipFiles.Count;
            Console.WriteLine($"Find {countZipFiles} zip files. In {Constans.FilesSourceDirectory}");

            var cvsFiles = HandlingFilesService.UnZipCVSFiles(zipFiles);

            var countCVSFiles = zipFiles == null ? 0 : cvsFiles.Count;
            Console.WriteLine($"Unpack {countZipFiles} cvs files.");

            HandlingFilesService.HandleCSVFiles(cvsFiles);

            Console.ReadLine();
        }


    }
}
