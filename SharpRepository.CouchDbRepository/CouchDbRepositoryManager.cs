using System;
using System.Linq;
using System.Net;
using RedBranch.Hammock;

namespace SharpRepository.CouchDbRepository
{
    public static class CouchDbRepositoryManager
    {
        public static bool ServerIsRunning(string url)
        {
            try
            {
                // Send a HEAD request to the url
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 2000;
                request.Method = WebRequestMethods.Http.Get;

                // this line throws a WebException if there is a problematic status code
                request.GetResponse();

            }
            catch
            {
                return false;
            }

            return true;
        }

        public static void CreatDatabase(string url, string databaseName)
        {
            var connection = new Connection(new Uri(url));

            if (!connection.ListDatabases().Contains(databaseName))
            {
                connection.CreateDatabase(databaseName);
            }
        }

        public static void DropDatabase(string url, string databaseName)
        {
            var connection = new Connection(new Uri(url));

            if (connection.ListDatabases().Contains(databaseName))
            {
                connection.DeleteDatabase(databaseName);    
            }
        }
    }
}
