using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ThomsonConsole
{

    public class FilesSearch
    {
        const string SearchFilesMask = "*.zip";

        public static List<string> FindAllZipFiles(string root)
        {
            return TraverseDirectoriesTree(root, SearchFilesMask);
        }

        static List<string> TraverseDirectoriesTree(string root, string searchMask)
        {
            List<string> result = new List<string>();
            Stack<string> dirs = new Stack<string>();

            if (System.IO.Directory.Exists(root))
            {
                dirs.Push(root);

                while (dirs.Count > 0)
                {
                    string currentDir = dirs.Pop();
                    string[] subDirs;
                    try
                    {
                        subDirs = System.IO.Directory.GetDirectories(currentDir);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                    catch (System.IO.DirectoryNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }

                    string[] files = null;
                    try
                    {
                        files = System.IO.Directory.GetFiles(currentDir, searchMask);
                    }

                    catch (UnauthorizedAccessException e)
                    {

                        Console.WriteLine(e.Message);
                        continue;
                    }

                    catch (System.IO.DirectoryNotFoundException e)
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
