namespace SharpRepository.Tests.Integration.Data
{
    public static class CouchDbUrl
    {
        public static string Url
        {
            get { return "http://127.0.0.1:5984/"; }
        }

        public static string Host
        {
            get { return "localhost"; }
        }

        public static int Port
        {
            get { return 5984; }
        }
    }
}
