using System;

namespace SharpRepository.Tests.Integration.Data
{
    public class MongoDbConnectionStringFactory
    {
        private static int _num = 1;

        public static string Build(string type)
        {
            var connectionString = String.Format("mongodb://127.0.0.1/{0}{1}", type, _num);
            _num++; // since it goes through and calls this for each test before running them, we need a different database for each test 
            return connectionString;
        }
    }
}