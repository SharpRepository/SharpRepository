using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SharpRepository.CouchDbRepository.ReLinq.QueryGeneration;

namespace SharpRepository.CouchDbRepository.ReLinq
{
    public class CouchDbQuery
    {
        private readonly string _url;
        private readonly string _databaseName;
        private QueryPartsAggregator _queryParts;

        public CouchDbQuery(string url, string databaseName, QueryPartsAggregator queryParts)
        {
            _url = url;
            _databaseName = databaseName;
            _queryParts = queryParts;
        }

        public IEnumerable<T> Enumerable<T>()
        {
            var querystring = String.Empty;

            if (_queryParts.Take.HasValue)
                querystring += "limit=" + _queryParts.Take.Value + "&";
            //            else if (_isFirst)
            //                querystring += "limit=1&";

            if (_queryParts.IsDescending)
                querystring += "descending=true&";

            var fullUrl = _url + "/" + _databaseName + "/_temp_view?" + querystring;

            var json = CouchDbRequest.Execute(fullUrl, "POST", _queryParts.BuildCouchDbApiPostData(), "application/json");

            // get the rows property and deserialize that
            var res = JObject.Parse(json);
            var rows = res["rows"];

            return rows.Select(row => row["value"].ToObject<T>());
        }
    }
}
