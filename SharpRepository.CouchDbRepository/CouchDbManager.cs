using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;

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
                CouchDbRequest.Execute(server, "_all_dbs", HttpMethod.Get)
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
            var result = CouchDbRequest.Execute(server, db, HttpMethod.Put);
            if (result.Trim() != "{\"ok\":true}")
                throw new Exception("Failed to create database: " + result); // was a ApplicationException, will be added back netstandard2.0
        }

        /// <summary>
        /// Delete a database
        /// </summary>
        public static void DeleteDatabase(string server, string db)
        {
            var result = CouchDbRequest.Execute(server, db, HttpMethod.Delete);
            if (result.Trim() != "{\"ok\":true}")
                throw new Exception("Failed to delete database: " + result); // was a ApplicationException, will be added back netstandard2.0
        }
    }
}
