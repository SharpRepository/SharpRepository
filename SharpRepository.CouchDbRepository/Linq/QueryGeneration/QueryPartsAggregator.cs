using System;
using System.Collections.Generic;
using System.Text;
using Remotion.Linq.Clauses;
using Remotion.Linq.Utilities;

namespace SharpRepository.CouchDbRepository.Linq.QueryGeneration
{
    public class QueryPartsAggregator
    {
        public QueryPartsAggregator()
        {
            FromParts = new List<string>();
            WhereParts = new List<string>();
        }

        public string SelectPart { get; set; }
        private List<string> FromParts { get; set; }
        private List<string> WhereParts { get; set; }
        private string OrderBy { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public bool OrderByIsDescending { get; set; }
        public bool ReturnCount = false;

        public void AddFromPart(IQuerySource querySource)
        {
            FromParts.Add(string.Format("{0}", querySource.ItemName));
        }

        public void AddWherePart(string formatString, params object[] args)
        {
            WhereParts.Add(string.Format(formatString, args));
        }

        public void AddOrderByPart(string orderBy, bool isDescending)
        {
            OrderBy = orderBy;
            OrderByIsDescending = isDescending;
        }

        public string BuildCouchDbApiPostData()
        {
            var stringBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(SelectPart) || FromParts.Count == 0)
                throw new InvalidOperationException("A query must have a select part and at least one from part.");

            stringBuilder.Append("{ \"map\":\"function (doc) {");

            if (WhereParts.Count > 0)
            {
                stringBuilder.AppendFormat("if ({0}) ", string.Join(" && ", WhereParts));
            }

            stringBuilder.AppendFormat("emit({0}, ", !String.IsNullOrEmpty(OrderBy) ? OrderBy : "doc._id");

            // TODO: use the SelectParts to only return the properties that are needed by emitting {Name: "Jeff", Title: "Awesome"}

            var select = "doc"; // return the entire thing by default
            if (!String.IsNullOrEmpty(SelectPart))
                select = SelectPart;

            stringBuilder.Append(select + ");}\"}");

            return stringBuilder.ToString();
        }

//        private string GetEntityName(IQuerySource querySource)
//        {
//            //return NHibernateUtil.Entity(querySource.ItemType).Name;
//        }
    }
}
