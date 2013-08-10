using System.IO;

namespace SharpRepository.Tests.Integration.Data
{
    public class XmlDataDirectoryFactory
    {
        public static string Build(string type)
        {
            var dataDirectory = DataDirectoryHelper.GetDataDirectory();
            var xmlDataDirectoryPath = Path.Combine(dataDirectory, @"Xml");

            if (!Directory.Exists(xmlDataDirectoryPath))
            {
                Directory.CreateDirectory(xmlDataDirectoryPath);
            }

            var file = string.Format("{0}\\{1}.xml", xmlDataDirectoryPath, type);
            
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            return xmlDataDirectoryPath;
        }
    }
}