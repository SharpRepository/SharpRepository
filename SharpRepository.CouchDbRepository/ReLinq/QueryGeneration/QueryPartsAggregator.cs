using System;
using System.Collections.Generic;
using System.Text;
using Remotion.Linq.Clauses;
using Remotion.Linq.Utilities;

namespace SharpRepository.CouchDbRepository.ReLinq.QueryGeneration
{
    public class QueryPartsAggregator
    {
        public QueryPartsAggregator()
        {
            FromParts = new List<string>();
            WhereParts = new List<string>();
            OrderByParts = new List<string>();
        }

        public string SelectPart { get; set; }
        private List<string> FromParts { get; set; }
        private List<string> WhereParts { get; set; }
        private List<string> OrderByParts { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public bool IsDescending { get; set; }

        public void AddFromPart(IQuerySource querySource)
        {
            FromParts.Add(string.Format("{0}", querySource.ItemName));
        }

        public void AddWherePart(string formatString, params object[] args)
        {
            WhereParts.Add(string.Format(formatString, args));
        }

        public void AddOrderByPart(IEnumerable<string> orderings)
        {
            OrderByParts.Insert(0, SeparatedStringBuilder.Build(", ", orderings));
        }

        public string BuildCouchDbApiPostData()
        {
            var stringBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(SelectPart) || FromParts.Count == 0)
                throw new InvalidOperationException("A query must have a select part and at least one from part.");

            stringBuilder.Append("{ \"map\":\"function (doc) {");

            if (WhereParts.Count > 0)
            {
                stringBuilder.AppendFormat("if ({0}) ", SeparatedStringBuilder.Build(" && ", WhereParts));
            }

            stringBuilder.AppendFormat("emit(doc.{0}, ", OrderByParts.Count > 0 ? OrderByParts[0] : "_id");

            // TODO: use the SelectParts to only return the properties that are needed by emitting {Name: "Jeff", Title: "Awesome"}
            stringBuilder.Append("doc);}\"}");

            return stringBuilder.ToString();
        }

        private string GetEntityName(IQuerySource querySource)
        {
            // TODO: implement this
            return "TODO: GetEntityName Implemenation";
            //return NHibernateUtil.Entity(querySource.ItemType).Name;
        }
    }
}
