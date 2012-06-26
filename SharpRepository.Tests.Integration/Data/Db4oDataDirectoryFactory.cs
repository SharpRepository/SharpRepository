using System;
using System.IO;

namespace SharpRepository.Tests.Integration.Data
{
    public class Db4oDataDirectoryFactory
    {
        private static int _num = 1;
        
        public static string Build(string type)
        {
            var dataDirectory = DataDirectoryHelper.GetDataDirectory();
            string db4oPath = Path.Combine(dataDirectory, @"Db4o");

            var file = String.Format("{0}\\{1}.yap", db4oPath, _num);
            _num++; // since it goes through and calls this for each test before running them, we need a different database for each test 
            

            if (File.Exists(file)) { File.Delete(file); }

            return file;
        }
    }
}