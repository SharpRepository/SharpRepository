using MongoDB.Bson;
using MongoDB.Driver;

namespace SharpRepository.MongoDbRepository
{
    public static class MongoDbRepositoryManager
    {
        public static bool ServerIsRunning(IMongoDatabase db)
        {
            try
            {
                db.RunCommandAsync((Command<BsonDocument>)"{ping:1}")
                        .Wait();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool ServerIsRunning(string connectionString)
        {
            var db_name = MongoUrl.Create(connectionString).DatabaseName;
            var cli = new MongoClient(connectionString);
            return ServerIsRunning(cli.GetDatabase(db_name));
        }

        public static string DatabaseName(string connectionString)
        {
            return MongoUrl.Create(connectionString).DatabaseName;
        }

        public static void DropDatabase(string connectionString)
        {
            var cli = new MongoClient(connectionString);
            var db_name = MongoUrl.Create(connectionString).DatabaseName;
            cli.DropDatabase(db_name);
        }
    }
}