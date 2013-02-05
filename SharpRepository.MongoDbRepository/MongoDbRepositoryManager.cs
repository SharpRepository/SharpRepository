using MongoDB.Driver;

namespace SharpRepository.MongoDbRepository
{
    public static class MongoDbRepositoryManager
    {
        public static bool ServerIsRunning(MongoServer server)
        {
            try
            {
                server.Ping();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool ServerIsRunning(string connectionString)
        {
            return ServerIsRunning(new MongoClient(connectionString).GetServer());
        }

        public static string DatabaseName(string connectionString)
        {
            return MongoUrl.Create(connectionString).DatabaseName;
        }

        public static void DropDatabase(string connectionString)
        {
            var server = new MongoClient(connectionString).GetServer();
            var db = DatabaseName(connectionString);
            server.DropDatabase(db);
        }
    }
}