using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomsonConsole
{
    class ClassGenerator
    {
        const string NameOfClass = "class AutomaticGenerated";
        private List<dynamic> Data {get; set;}
        private List<string> NamesOfFields { get; set; }
        public List<string> ClassDescriprion { get; private set; }

        public ClassGenerator(List<dynamic> data, List<string> namesOfFields)
        {
            Data = data;
            NamesOfFields = namesOfFields;
            ClassDescriprion = GetClassDescription();
        }

        List<string> GetClassDescription()
        {
            List<string> classDescription = new List<string>();
            classDescription.Add(NameOfClass);
            classDescription.Add("{");
            foreach (var nameOfField in NamesOfFields)
            {   
                classDescription.Add(GetPropertyDescription(nameOfField));
            }
            classDescription.Add("}");
            return classDescription;
        }

        private string GetPropertyDescription(string nameOfField)
        {
            string propertyDescription = "";
            if (!string.IsNullOrWhiteSpace(nameOfField))
            {
                var columnData = Data.Select(line =>
                                            ((IDictionary<string, object>)line).Where(x => x.Key == nameOfField)
                                            .Select(row => (string)row.Value).Single());
                propertyDescription = $"public {TypeAsString(columnData)} {NormalizeNameOfProperty(nameOfField)} {{ get; set; }}";
            }
            return propertyDescription;
        }

        private object NormalizeNameOfProperty(string nameOfField)
        {
            return nameOfField;
        }

        public static string TypeAsString(IEnumerable<string> columnData)
        {
            string typeAsString = "string";

            if (AllDateTimeValues(columnData))
            {
                typeAsString = "DateTime";
            }
            else if (AllIntValues(columnData))
            {
                typeAsString = "int";
            }
            else if (AllDoubleValues(columnData))
            {
                typeAsString = "double";
            }
            return typeAsString;
        }

        public static bool AllDoubleValues(IEnumerable<string> values)
        {
            double d;
            return values.All(val => double.TryParse(val, out d));
        }

        public static bool AllIntValues(IEnumerable<string> values)
        {
            int d;
            return values.All(val => int.TryParse(val, out d));
        }

        public static bool AllDateTimeValues(IEnumerable<string> values)
        {
            DateTime d;
            return values.All(val => DateTime.TryParse(val, out d));
        }

        public void PrintWriteDescription()
        {
            foreach (var line in ClassDescriprion)
            {
                Console.WriteLine(line);
            }
        }
    }
}
