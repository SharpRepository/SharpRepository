using System;
using System.IO;

namespace SharpRepository.Tests.Integration.Data
{
    public class NHibernateDataDirectoryFactory
    {
        private static int _num = 1;

        public static string Build()
        {
            var dataDirectory = DataDirectoryHelper.GetDataDirectory();
            var dataDirectoryPath = Path.Combine(dataDirectory, @"NHibernate");

            var file = String.Format("{0}\\TestEntitiesDb{1}.db", dataDirectoryPath, _num);
            _num++; // since it goes through and calls this for each test before running them, we need a different database for each test or else the auto increment goes to 2 on the second test with an add and it fails
                    // surprisingly the timing isn;'t too bad for this, the first test takes about 7 secs for EF to model the DB and create it, then each other test is super quick in creating the DB file

            if (File.Exists(file)) { File.Delete(file); }

            return file;
        }
    }
}
