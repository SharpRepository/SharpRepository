namespace SharpRepository.ODataRepository.Linq.QueryGeneration
{
    public class CommandData
    {
        public CommandData(QueryPartsAggregator queryParts)
        {
            QueryParts = queryParts;
        }

        public QueryPartsAggregator QueryParts { get; set; }

        public ODataQuery CreateQuery(string url, string databaseName)
        {
            return new ODataQuery(url, databaseName, QueryParts);
        }
    }
}
