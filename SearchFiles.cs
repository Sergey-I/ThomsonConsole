using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ThomsonConsole
{

    public class SearchFiles
    {
        static SearchFiles()
        {
            AppDirectory = Directory.GetCurrentDirectory();
            DirectorySeparatorChar = Path.DirectorySeparatorChar;
            FolderOfSourcefiles = DirectorySeparatorChar + "Thomson" + DirectorySeparatorChar;
            FilesSourceDirectory = AppDirectory + FolderOfSourcefiles;
        }

        static public string AppDirectory { get;}
        static public string FilesSourceDirectory { get;}
        static public string TemptoryDirectory { get; }
        static string FolderOfSourcefiles { get; }
        static public char DirectorySeparatorChar { get; }

        const string ZipFileMask = "*.zip";
        const string CSVFileMask = "*.csv";

        public static List<string> FindAllZipFiles()
        {
            return FindAllFiles(FilesSourceDirectory, ZipFileMask);
        }

        public static List<string> FindAllZipFiles(string root)
        {
            return FindAllFiles(root, ZipFileMask);
        }

        public static List<string> FindAllCSVFiles(string root)
        {
            return FindAllFiles(root, CSVFileMask);
        }


        public static List<string> FindAllFiles(string root, string searchMask)
        {
            List<string> result = new List<string>();
            Stack<string> dirs = new Stack<string>();

            if (Directory.Exists(root))
            {
                dirs.Push(root);

                while (dirs.Count > 0)
                {
                    string currentDir = dirs.Pop();
                    string[] subDirs;
                    try
                    {
                        subDirs = Directory.GetDirectories(currentDir);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }

                    string[] files = null;
                    try
                    {
                        files = Directory.GetFiles(currentDir, searchMask);
                    }

                    catch (UnauthorizedAccessException e)
                    {

                        Console.WriteLine(e.Message);
                        continue;
                    }

                    catch (DirectoryNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }

                    if (files != null && files.Length > 0)
                    {
                        result.AddRange(files);
                    }

                    foreach (string str in subDirs)
                        dirs.Push(str);
                }
            }
            return result;
        }
    }
}
