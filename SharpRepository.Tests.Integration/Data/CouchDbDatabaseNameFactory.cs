using System;

namespace SharpRepository.Tests.Integration.Data
{
    public class CouchDbDatabaseNameFactory
    {
        private static int _num = 1;

        public static string Build(string type)
        {
            var databaseName = String.Concat(type.ToLower(), _num);
            _num++; // since it goes through and calls this for each test before running them, we need a different database for each test 
            return databaseName;
        }
    }
}
