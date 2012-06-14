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
            return Path.Combine(rd.FullName, @"Data");
        }
    }
}