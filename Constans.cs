using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ThomsonConsole
{
    static class Constans
    {
        static Constans()
        {
            AppDirectory = Directory.GetCurrentDirectory();
            DirectorySeparatorChar = Path.DirectorySeparatorChar;
            FolderOfSourcefiles = DirectorySeparatorChar + "Thomson" + DirectorySeparatorChar;
            FilesSourceDirectory = AppDirectory + FolderOfSourcefiles;
            FolderForUnzipFiles = DirectorySeparatorChar + "Temp" + DirectorySeparatorChar;
            TemporaryDirectory = AppDirectory + FolderForUnzipFiles;
            DelimitersCSV = new string[] { "," };
        }

        public static string AppDirectory { get; }
        public static char DirectorySeparatorChar { get; }
        public static string FolderOfSourcefiles { get; }
        public static string FilesSourceDirectory { get; }
        public static string FolderForUnzipFiles { get; }
        public static string TemporaryDirectory { get; }
        public static string[] DelimitersCSV { get; }

        public const string ZipFileMask = "*.zip";
        public const string CSVFileMask = "*.csv";
        public const int MinimalQuantityOfFieldsInCVSFile = 1;
    }
}
