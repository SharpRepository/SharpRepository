using System.IO;
using SharpRepository.Tests.Integration.Helpers;

namespace SharpRepository.Tests.Integration.Data
{
    public class DataDirectoryHelper
    {
        public static string GetDataDirectory()
        {
            var rd = new CurrentDirectory();
            rd.MoveUpToDirectory("SharpRepository.Tests.Integration");
            var path = Path.Combine(rd.FullName, @"Data");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}