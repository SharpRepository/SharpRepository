using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SharpRepository.CouchDbRepository.Linq.QueryGeneration;

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

            if (_queryParts.Skip.HasValue)
            {
                skip = _queryParts.Skip.Value;
            }

            if (_queryParts.Take.HasValue)
            {
                // since CouchDb doesn't have a skip param, we need to take all the skipped values as well and then ignore them in the results
                //  so if Skip is 2 and Take is 3, then we actually need to take 5 results and skip the first 2 when we get the results
                // TODO: look into if it's possible to do how they suggest for CouchDB where to do pagination when it requests 5 items for the first page you actually get 6 so you have the 6th key and when you are on page 2, then you use startkey with the 6th key and get 5 more from there

                take = skip + _queryParts.Take.Value;
            }

            if (take.HasValue)
                querystring += "limit=" + take.Value + "&";

            if (_queryParts.OrderByIsDescending)
                querystring += "descending=true&";

            var fullUrl = _url + "/" + _databaseName + "/_temp_view?" + querystring;

            var json = CouchDbRequest.Execute(fullUrl, "POST", _queryParts.BuildCouchDbApiPostData(), "application/json");

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

            var items = rows.Select(row => row["value"].ToObject<T>());

            return items.Skip(skip);
        }
    }
}
