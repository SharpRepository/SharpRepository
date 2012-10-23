namespace SharpRepository.CouchDbRepository.ReLinq.QueryGeneration
{
    public class CommandData
    {
        public CommandData(QueryPartsAggregator queryParts)
        {
            QueryParts = queryParts;
        }

        public QueryPartsAggregator QueryParts { get; set; }

        public CouchDbQuery CreateQuery(string url, string databaseName)
        {
            return new CouchDbQuery(url, databaseName, QueryParts);
        }
    }
}
