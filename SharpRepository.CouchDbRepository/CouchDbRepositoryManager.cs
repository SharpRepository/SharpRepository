using System;
using System.Net;

namespace SharpRepository.CouchDbRepository
{
    public static class CouchDbRepositoryManager
    {
        public static bool ServerIsRunning(string host = "localhost", int port = 5984)
        {
            try
            {
                var url = String.Format("http://{0}:{1}", host, port);

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
