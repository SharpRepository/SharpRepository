using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SharpRepository.CouchDbRepository.Linq.QueryGeneration;
using System.Net.Http;

namespace SharpRepository.CouchDbRepository.Linq
{
    public class CouchDbQuery
    {
        private readonly string _url;
        private readonly string _databaseName;
        private readonly QueryPartsAggregator _queryParts;

        public CouchDbQuery(string url, string databaseName, QueryPartsAggregator queryParts)
        {
            _url = url;
            _databaseName = databaseName;
            _queryParts = queryParts;
        }

        public IEnumerable<T> Enumerable<T>()
        {
            var querystring = String.Empty;
            var resultType = typeof (T);

            var skip = 0;
            int? take = null;

            // TODO: look into if it's possible to do how they suggest for CouchDB where to do pagination when it requests 5 items for the first page you actually get 6 so you have the 6th key and when you are on page 2, then you use startkey with the 6th key and get 5 more from there
            if (_queryParts.Skip.HasValue)
            {
                skip = _queryParts.Skip.Value;
            }

            if (_queryParts.Take.HasValue)
            {
                take = _queryParts.Take.Value;
            }

            if (take.HasValue)
                querystring += "limit=" + take.Value + "&";

            if (skip != 0)
                querystring += "skip=" + skip + "&";

            if (_queryParts.OrderByIsDescending)
                querystring += "descending=true&";

            var fullUri =  _databaseName + "/_temp_view?" + querystring;

            var json = CouchDbRequest.Execute(_url, fullUri, HttpMethod.Post, _queryParts.BuildCouchDbApiPostData(), "application/json");

            JObject res;
            // check for Count() [Int32] and LongCOunt() [Int64]
            if (_queryParts.ReturnCount && (resultType == typeof(Int32) || resultType == typeof(Int64)))
            {
                var results = new List<T>();

                res = JObject.Parse(json);

                results.Add(res["total_rows"].ToObject<T>());
                return results;
            }

            // get the rows property and deserialize that
            res = JObject.Parse(json);
            var rows = res["rows"];

            return rows.Select(row => row["value"].ToObject<T>());
        }
    }
}
