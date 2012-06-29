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
            return ServerIsRunning(MongoServer.Create(connectionString));
        }

        public static string DatabaseName(string connectionString)
        {
            return MongoUrl.Create(connectionString).DatabaseName;
        }

        public static void DropDatabase(string connectionString)
        {
            MongoServer server = MongoServer.Create(connectionString);
            string db = DatabaseName(connectionString);
            server.DropDatabase(db);
        }
    }
}