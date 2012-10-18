// Inspired by SharpCouch and tweaked for our specific needs

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpRepository.CouchDbRepository
{

    /// <summary>
    /// Used to return metadata about a document.
    /// </summary>
    public class DocInfo
    {
        public string Id;
        public string Revision;
    }

    /// <summary>
    /// A simple wrapper class for the CouchDB HTTP API. No
    /// initialisation is necessary, just create an instance and
    /// call the appropriate methods to interact with CouchDB.
    /// All methods throw exceptions when things go wrong.
    /// </summary>
    public class CouchDbClient<T> where T : class, new()
    {
        private readonly string _server;
        private readonly string _database;

        public CouchDbClient()
        {
        }

        public CouchDbClient(string server, string database)
        {
            _server = server;
            _database = database;
        }

        /// <summary>
        /// Get the document count for the given database.
        /// </summary>
        /// <returns>The number of documents in the database</returns>
        public int CountDocuments()
        {
            // Get information about the database...
            var result = CouchDbRequest.Execute(_server + "/" + _database, "GET");

            var dictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(result);

            // The document count is a field within...
            return Int32.Parse(dictionary["doc_count"]);
        }

        /// <summary>
        /// Get information on all the documents in the given database.
        /// </summary>
        /// <returns>An array of DocInfo instances</returns>
        public IList<T> GetAllDocuments()
        {
            // terribly inefficient way of doing this
            //  get all document ids and then do a query for each one
            var result = CouchDbRequest.Execute(_server + "/" + _database + "/_all_docs", "GET");

            var res = JObject.Parse(result);
            var rows = res["rows"];
            var list = rows.ToObject<IList<IDictionary<string, object>>>();

            return list.Select(item => item["id"].ToString()).Select(GetDocument).ToList();

            //var map = "function(doc) { emit(doc.id, doc); }";
            //var result = ExecTempView(map, null);
            //// can get total_rows also if needed for pagination at some point
            //var obj = JObject.Parse(result);
            //var rows = obj["rows"];
            //var list = rows.ToObject<IList<IDictionary<string, object>>>();

            //var items = new List<T>();

            //foreach (var item in list)
            //{
            //    // this is not working
            //    items.Add(item["value"] as T);
            //}

            //return items;
        }

        /// <summary>
        /// Execute a temporary view and return the results.
        /// </summary>
        /// <param name="map">The javascript map function</param>
        /// <param name="reduce">The javascript reduce function or
        /// null if not required</param>
        /// <returns>The result (JSON format)</returns>
        public string ExecTempView(string map, string reduce)
        {
            // Generate the JSON view definition from the supplied
            // map and optional reduce functions...
            var viewdef = "{ \"map\":\"" + map + "\"";
            if (reduce != null)
                viewdef += ",\"reduce\":\"" + reduce + "\"";
            viewdef += "}";

            var url = _server + "/" + _database + "/_temp_view";

            return CouchDbRequest.Execute(url, "POST", viewdef, "application/json");
        }

        /// <summary>
        /// Create a new document. If the document has no ID field,
        /// it will be assigned one by the server.
        /// </summary>
        /// <param name="entity">The item to store in the database.</param>
        public void CreateDocument(T entity, string id)
        {
            var item = JsonConvert.SerializeObject(entity);

            // add the _id so that it is the same as the PK of the entity
            //  this is a crappy way of doing it I think, but just trying to get it to work for now
            if (!String.IsNullOrEmpty(id))
            {
                item = "{\"_id\":\"" + id + "\"," + item.Substring(1);
            }

            CouchDbRequest.Execute(_server + "/" + _database, "POST", item, "application/json");
        }

        public string GetLatestRevision(string id)
        {
            var json = Get(id);

            var obj = JObject.Parse(json);

            return obj.Value<string>("_rev");
        }

        /// <summary>
        /// Updates a document.
        /// </summary>
        /// <param name="entity">The item to store in the database.</param>
        /// <param name="id">The document ID.</param>
        public void UpdateDocument(T entity, string id)
        {
            var item = JsonConvert.SerializeObject(entity);

            //needs the revision for updates and deletes, this isn't a great way because it's an extra API call, but it is what it is for now, just want to get it working then refactor
            var rev = GetLatestRevision(id);

            // add the _id so that it is the same as the PK of the entity
            //  this is a crappy way of doing it I think, but just trying to get it to work for now
            item = "{\"_id\":\"" + id + "\",\"_rev\":\"" + rev + "\"," + item.Substring(1);

            CouchDbRequest.Execute(_server + "/" + _database + "/" + id, "PUT", item, "application/json");
        }

        /// <summary>
        /// Get a document.
        /// </summary>
        /// <param name="id">The document ID.</param>
        /// <returns>The document contents (JSON)</returns>
        public T GetDocument(string id)
        {
            var json = Get(id);
            if (String.IsNullOrEmpty(json))
                return null;

            return JsonConvert.DeserializeObject<T>(json);
        }

        private string Get(string id)
        {
            var url = _server + "/" + _database + "/" + id;
            return CouchDbRequest.Execute(url, "GET");
        }

        /// <summary>
        /// Delete a document.
        /// </summary>
        /// <param name="id">The document ID.</param>
        public void DeleteDocument(string id)
        {
            //needs the revision for updates and deletes, this isn't a great way because it's an extra API call, but it is what it is for now, just want to get it working then refactor
            var rev = GetLatestRevision(id);

            CouchDbRequest.Execute(_server + "/" + _database + "/" + id + "?rev=" + rev, "DELETE");
        }
    }
}

