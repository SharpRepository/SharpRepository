using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SharpRepository.CouchDbRepository
{
    public static class CouchDbManager
    {
        /// <summary>
        /// Get a list of database on the server.
        /// </summary>
        /// <param name="server">The server URL</param>
        /// <returns>A string array containing the database names
        /// </returns>
        public static IList<string> GetDatabases(string server)
        {
            return JsonConvert.DeserializeObject<IList<string>>(
                CouchDbRequest.Execute(server + "/_all_dbs", "GET")
                );
        }

        /// <summary>
        /// Checks if the database exists already
        /// </summary>
        public static bool HasDatabase(string server, string db)
        {
            return GetDatabases(server).Contains(db);
        }

        /// <summary>
        /// Create a new database.
        /// </summary>
        public static void CreateDatabase(string server, string db)
        {
            var result = CouchDbRequest.Execute(server + "/" + db, "PUT");
            if (result.Trim() != "{\"ok\":true}")
                throw new ApplicationException("Failed to create database: " + result);
        }

        /// <summary>
        /// Delete a database
        /// </summary>
        public static void DeleteDatabase(string server, string db)
        {
            var result = CouchDbRequest.Execute(server + "/" + db, "DELETE");
            if (result.Trim() != "{\"ok\":true}")
                throw new ApplicationException("Failed to delete database: " + result);
        }
    }
}
