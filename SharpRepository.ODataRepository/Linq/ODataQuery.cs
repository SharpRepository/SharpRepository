using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Remotion.Linq.Utilities;
using SharpRepository.ODataRepository.Linq.QueryGeneration;

namespace SharpRepository.ODataRepository.Linq
{
    public class ODataQuery
    {
        private readonly string _url;
        private readonly string _collectionName;
        private readonly QueryPartsAggregator _queryParts;

        public ODataQuery(string url, string collectionName, QueryPartsAggregator queryParts)
        {
            _url = url;
            _collectionName = collectionName;
            _queryParts = queryParts;
        }

        public IEnumerable<T> Enumerable<T>()
        {
            var querystring = String.Empty;
            var resultType = typeof (T);

            if (_queryParts.ReturnCount)
            {
                querystring += "/$count";
            }

            querystring += "?";

            if (_queryParts.Take.HasValue)
                querystring += "$top=" + _queryParts.Take.Value + "&";

            if (_queryParts.Skip.HasValue)
                querystring += "$skip=" + _queryParts.Skip.Value + "&";

            if (!String.IsNullOrEmpty(_queryParts.OrderBy))
                querystring += "$orderby=" + _queryParts.OrderBy + "&";

            var filter = SeparatedStringBuilder.Build(" and ", _queryParts.WhereParts);
            if (!String.IsNullOrEmpty(filter))
                querystring += "$filter=" + filter + "&";

            if (!String.IsNullOrEmpty(_queryParts.SelectPart))
                querystring += "$select=" + _queryParts.SelectPart + "&";

            var fullUrl = _url + "/" + _collectionName + querystring;
            var json = UrlHelper.Get(fullUrl);

            //var json = ODataRequest.Execute(fullUrl, "POST", _queryParts.BuildODataApiPostData(), "application/json");

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

            return items;
        }
    }
}
