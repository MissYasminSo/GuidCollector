using System.Globalization;
using System.Xml;
using CsvHelper;


namespace guidcollector
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || !Directory.Exists(args[0]))
            {
                Console.WriteLine("Error! You should drag and drop a folder onto this exe. Hint: you can create a desktop shortcut.");
                Console.ReadLine();
                return;
            }

            string targetDirectory = args[0];

            var files = Directory.GetFiles(targetDirectory, "*");
            
            var infos = new List<ModelDetails>();
            
            XmlDocument xmlDoc = new XmlDocument();
            
            foreach (var file in files)
            {
                if (file.EndsWith(".xml"))
                {
                    xmlDoc.Load(file);
                    XmlNodeList ModelInfos = xmlDoc.SelectNodes("/ModelInfo");
                    
                    foreach (XmlNode ModelInfo in ModelInfos)
                    {
                        var info = new ModelDetails
                        {
                            XmlName = Path.GetFileNameWithoutExtension(file),
                            Guid = ModelInfo.Attributes["guid"].Value
                        };

                        infos.Add(info);
                    }
                }
            }
            
            using (var writer = new StreamWriter(Path.Combine(Path.GetFileNameWithoutExtension(args[0]) + " Guids.csv")))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(infos);
            }

            Console.WriteLine("Press enter to quit.");
            Console.ReadLine();
        }
        
    }
}

public class ModelDetails
{
    public string XmlName { get; set; }
    public string Guid { get; set; }
}