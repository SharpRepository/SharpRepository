using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpRepository.CouchDbRepository.Linq
{
    public class CouchDbQueryProvider: IQueryProvider
    {
        private readonly string _databaseName;
        private readonly string _url;
//        private readonly string _typeName;

        public CouchDbQueryProvider(string url, string databaseName)
        {
            _url = url;
            _databaseName = databaseName;
//            _typeName = typeof (T).Name.ToLower();
        }

        public IQueryable<T> CreateQuery<T>()
        {
            return new Query<T>(this);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        public IQueryable<T> CreateQuery<T>(Expression expression)
        {
            return new Query<T>(this, expression);
        }

        public object Execute(Expression expression)
        {
            return Execute<object>(expression);
        }

         public IEnumerable<T> ExecuteEnumerable<T>(Expression expression)
         {
             string url, postData;

             Translate(expression, out url, out postData);

             //             if (String.IsNullOrEmpty(postData))
             //                 return (TResult)_client.GetAllDocuments();

             var json = CouchDbRequest.Execute(url, "POST", postData, "application/json");

             // get the rows property and deserialize that
             var res = JObject.Parse(json);
             var rows = res["rows"];

             return rows.Select(row => row["value"].ToObject<T>()).ToList();
         }

        public TResult Execute<TResult>(Expression expression)
        {
            throw new NotImplementedException();
        }

        private void Translate(Expression expression, out string url, out string postData)
        {
            url = null;

            string querystring;
            var visitor = new CouchDbExpressionVisitor();
            visitor.Parse(expression, _databaseName, out postData, out querystring);

            if (String.IsNullOrEmpty(postData))
               return;

            url = _url + "/" + _databaseName + "/_temp_view?" + querystring;
        }

//        private T DeserializeObject<T>(string json)
//        {
//            try
//            {
//                return JsonConvert.DeserializeObject<T>(json);
//            }
//            catch (JsonSerializationException ex)
//            {
//
//            }
//
//            var jobject = JsonConvert.DeserializeObject(json) as JObject;
//            var objectString = String.Empty;
//
//            try
//            {
//                var token = jobject["d"];
//
//                try
//                {
//                    var results = token["results"]; // netflix OData service (http://odata.netflix.com/v2/Catalog/) adds a results array inside the d array while other services don't
//
//                    if (results != null)
//                    {
//                        objectString = results.ToString();
//                    }
//                }
//                catch
//                {
//                    
//                }
//
//                if (String.IsNullOrEmpty(objectString))
//                {
//                    objectString = token.ToString();
//                }
//                
//            }
//            catch
//            {
//                
//            }
//
//
//            var items = JsonConvert.DeserializeObject<T>(objectString);
//
//            return items;
//        }


    }
}