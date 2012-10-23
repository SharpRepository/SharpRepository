// ref: http://www.codeproject.com/Articles/42059/re-linq-ishing-the-Pain-Using-re-linq-to-Implement
namespace SharpRepository.CouchDbRepository.ReLinq
{
    public class CouchDbQueryFactory
    {
        public static CouchDbQueryable<T> Queryable<T>(string url, string databaseName)
        {
            return new CouchDbQueryable<T>(url, databaseName);
        }
    }
}
