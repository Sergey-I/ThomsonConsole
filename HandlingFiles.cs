using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Microsoft.VisualBasic.FileIO;
using System.Dynamic;

namespace ThomsonConsole
{
    class HandlingFiles
    {
        static HandlingFiles()
        {
            FolderForUnzipFiles = SearchFiles.DirectorySeparatorChar + "Temp" + SearchFiles.DirectorySeparatorChar;
            TemporaryDirectory = SearchFiles.AppDirectory + FolderForUnzipFiles;
            DelimitersCSV = new string[] { "," };
        }

        static public string TemporaryDirectory { get; }
        static string FolderForUnzipFiles { get; }
        static string[] DelimitersCSV { get; }

        private const int MinimalQuantityOfFieldsInCVSFile = 1;
 
        public void ClearTemporaryDirectory()
        {
            ClearDirectory(TemporaryDirectory);
        }

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

            return TemporaryDirectory + ParthOfPathToZipfile + SearchFiles.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(zipFile);
            
        }

        public static void HandleCSVFiles(IList<string> csvFiles)
        {
            if (csvFiles != null)
            {
                foreach (string cvsFile in csvFiles)
                {
                    var csvFileData = ParseCSVFile(cvsFile);
                    PrintFirstCVSData(csvFileData);
                }
            }
        }

        public static void PrintFirstCVSData(List<dynamic> csvFileData)
        {
            if (csvFileData != null)
            {
                foreach (var field in csvFileData.First())
                {
                    Console.WriteLine($"{field.Key,-25}|||{field.Value}");
                }
            }
        }

        public static List<dynamic> ParseCSVFile(string csvFile)
        {
            using (var csvReader = new TextFieldParser(csvFile))
            {
                csvReader.SetDelimiters(DelimitersCSV);
                string[] fields, namesOfFields = null;
                List<dynamic> csvFileData = new List<dynamic>();

                fields = csvReader.ReadFields();
                while (fields != null)
                {
                    if (namesOfFields == null)
                    {
                        namesOfFields = GetNamesOfFields(fields);
                    }
                    else
                    {
                        dynamic lineData = new ExpandoObject();
                        var line = (IDictionary<string, object>)lineData;
                        for (int i = 0; i < namesOfFields.Length; i++)
                        {
                            try
                            {
                                line.Add(namesOfFields[i], fields[i]);
                            }
                            catch (System.ArgumentException e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine($"ERROR: Names of the columns in a csv file are not unique. File:{csvFile}.");
                                return null;
                            }
                            catch (System.IndexOutOfRangeException e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine($"ERROR: Stucture of a cvs file is wrong. File:{csvFile}. Line {csvReader.LineNumber}");
                                return null;
                            }
                        }
                        csvFileData.Add(lineData);
                    }
                    fields = csvReader.ReadFields();
                }
                return csvFileData;
            }
        }

        private static string[] GetNamesOfFields(string[] fields)
        {
            return (fields.Length > MinimalQuantityOfFieldsInCVSFile)? fields : null;
        }
    }
}
