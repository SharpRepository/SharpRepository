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

        public static bool ServerIsRunning(string connectionString, SslSettings sslSettings = null)
        {
            MongoClient cli;
            if (sslSettings != null)
            {
                var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                settings.SslSettings = sslSettings;
                cli = new MongoClient(settings);
            }
            else
                cli = new MongoClient(connectionString);

            var dbName = DatabaseName(connectionString);
            return ServerIsRunning(cli.GetDatabase(dbName));
        }

        public static string DatabaseName(string connectionString)
        {
            return MongoUrl.Create(connectionString).DatabaseName;
        }

        public static void DropDatabase(string connectionString, SslSettings sslSettings = null)
        {
            MongoClient cli;
            if (sslSettings != null)
            {
                var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                settings.SslSettings = sslSettings;
                cli = new MongoClient(settings);
            }
            else
                cli = new MongoClient(connectionString);

            var dbName = DatabaseName(connectionString);
            cli.DropDatabase(dbName);
        }
    }
}