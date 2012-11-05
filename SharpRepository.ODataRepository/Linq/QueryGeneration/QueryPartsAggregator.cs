using System;
using System.Collections.Generic;
using System.Text;
using Remotion.Linq.Clauses;
using Remotion.Linq.Utilities;

namespace SharpRepository.ODataRepository.Linq.QueryGeneration
{
    public class QueryPartsAggregator
    {
        public QueryPartsAggregator()
        {
            FromParts = new List<string>();
            WhereParts = new List<string>();
        }

        public string SelectPart { get; set; }
        public List<string> FromParts { get; set; }
        public List<string> WhereParts { get; set; }
        public string OrderBy { get; set; }
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


//        private string GetEntityName(IQuerySource querySource)
//        {
//            //return NHibernateUtil.Entity(querySource.ItemType).Name;
//        }
    }
}
