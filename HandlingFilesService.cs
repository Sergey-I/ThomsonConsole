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
    class HandlingFilesService
    {

        static void CleanDirectory(string directoryName)
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
                CleanDirectory(Constans.TemporaryDirectory);
                foreach (string zipFile in zipFiles)
                {
                    result.AddRange(UnZipCVSFile(zipFile));
                }
            }
            return result;
        }

        static List<string> UnZipCVSFile(string zipFile)
        {
            var unZipDirectory = GetUnzipDirectory(zipFile);
            CleanDirectory(unZipDirectory);
            ZipFile.ExtractToDirectory(zipFile, unZipDirectory);

            return SearchFilesService.FindAllCSVFiles(unZipDirectory);
        }

        static string GetUnzipDirectory(string zipFile)
        {
            string zipFileDirectory = Path.GetDirectoryName(zipFile);
            string PartOfPathToZipfile = "";
            if (zipFileDirectory.Length > Constans.FilesSourceDirectory.Length)
            {
                PartOfPathToZipfile = zipFileDirectory.Substring(Constans.FilesSourceDirectory.Length);
            }

            return Constans.TemporaryDirectory + PartOfPathToZipfile + Constans.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(zipFile);
            
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
                csvReader.SetDelimiters(Constans.DelimitersCSV);
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

        static string[] GetNamesOfFields(string[] fields)
        {
            return (fields.Length > Constans.MinimalQuantityOfFieldsInCVSFile) ? fields : null;
        }
    }
}
