// ref: http://www.codeproject.com/Articles/42059/re-linq-ishing-the-Pain-Using-re-linq-to-Implement
namespace SharpRepository.ODataRepository.Linq
{
    public class ODataQueryFactory
    {
        public static ODataQueryable<T> Queryable<T>(string url, string databaseName)
        {
            return new ODataQueryable<T>(url, databaseName);
        }
    }
}
