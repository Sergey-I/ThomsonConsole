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
        }

        static public string TemporaryDirectory { get; }
        static string FolderForUnzipFiles { get; }


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
                    Console.WriteLine($"{Path.GetFileName(cvsFile),-15} : {Path.GetDirectoryName(cvsFile)}");
                    var csvFileData = ParseCSVFile(cvsFile);
                    SaveCSVtoXML(csvFileData);
                }
            }
        }

        private static void SaveCSVtoXML(List<dynamic> csvFileData)
        {
            var i = 0;
            foreach(dynamic line in csvFileData)
            {
                i++;
                if (i > 10)
                {
                    break;
                }
                var fields = (IDictionary<string, object>)line;
                foreach(var field in fields)
                    Console.WriteLine($"Field name: {field.Key, -20}, field type: {field.Value.GetType(), -10}, field value: {field.Value}");
            }
        }

        public static List<dynamic> ParseCSVFile(string csvFile)
        {
            using (var csvReader = new TextFieldParser(csvFile))
            {
                csvReader.SetDelimiters(new string[] { "," });
                string[] fields, namesOfFields = null;
                List<dynamic> csvFileData = new List<dynamic>();

                while (true)
                {
                    fields = csvReader.ReadFields();
                    if (fields == null)
                    {
                        break;
                    }
                    if (namesOfFields == null)
                    {
                        if (fields.Length > MinimalQuantityOfFieldsInCVSFile)
                        {
                            namesOfFields = fields;
                        }
                    }
                    else
                    {
                        dynamic lineData = new ExpandoObject();
                        var line = (IDictionary<string, object>)lineData;
                        for(int i = 0;i < namesOfFields.Length; i++)
                        {
                            line.Add(namesOfFields[i], fields[i]);
                        }
                        csvFileData.Add(lineData);
                    }
                }
                return csvFileData;
            }
        }
    }
}
