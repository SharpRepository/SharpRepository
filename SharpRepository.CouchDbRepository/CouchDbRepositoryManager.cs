using System;
using System.Globalization;
using System.Net;
using System.Net.Http;

namespace SharpRepository.CouchDbRepository
{
    public static class CouchDbRepositoryManager
    {
        public static bool ServerIsRunning(string host = "localhost", int port = 5984)
        {
            try
            {
                var url = String.Format("http://{0}:{1}", host, port);
                
                using (var client = new HttpClient())
                {
                    var message = client.GetAsync(url).Result;
                    var result = message.Content.ReadAsStringAsync().Result; // Wrong connection will throw exception
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static void CreateDatabase(string host, int port, string database)
        {
            var url = String.Format("http://{0}:{1}", host, port);

            if (!CouchDbManager.HasDatabase(url, database))
            {
                CouchDbManager.CreateDatabase(url, database);
            }
        }

        public static void DropDatabase(string host, int port, string database)
        {
            var url = String.Format("http://{0}:{1}", host, port);

            if (CouchDbManager.HasDatabase(url, database))
            {
                CouchDbManager.DeleteDatabase(url, database);
            }
        }
    }
}
