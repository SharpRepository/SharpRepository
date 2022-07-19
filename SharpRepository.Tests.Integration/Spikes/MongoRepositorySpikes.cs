using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NUnit.Framework;
using SharpRepository.MongoDbRepository;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Integration.Spikes
{
    // Getting MongoDb installed on Windows
    // http://docs.mongodb.org/manual/tutorial/install-mongodb-on-windows/
    // When defining the log location include a file name like log.txt when the command fails.
    public class Order
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int ContactTypeId { get; set; } // for partitioning on 
        public List<EmailAddress> EmailAddresses { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
    }

    [MongoDbCollectionName("OrderCustomName")]
    public class OrderExtension : Order
    {
    }

    [TestFixture]
    public class MongoRepositorySpikes : TestBase
    {
        [Test]
        public void MongoDb_Supports_Basic_Crud_Operations()
        {
            const string connectionString = "mongodb://localhost/Order";

            if (!MongoDbRepositoryManager.ServerIsRunning(connectionString))
            {
                AssertIgnores.MongoServerIsNotRunning();
            }

            var cli = new MongoClient(connectionString);
            var databaseNames = cli.ListDatabases().ToList();
            foreach (var db in databaseNames)
            {
                if (db["name"].AsString != "admin")
                {
                    cli.DropDatabase(db["name"].AsString);
                }
            }

            var database = cli.GetDatabase("Order");
            var orders = database.GetCollection<Order>("Order");

            Console.WriteLine("* CREATE *");

            var create = new Order { Name = "Big sale" };
            database.GetCollection<Order>("Order").InsertOne(create);

            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.OrderId);
            }

            Console.WriteLine("* READ *");

            var filter = Builders<Order>.Filter.Eq(o => o.OrderId, create.OrderId);
            var read = orders.Find(filter).ToList().FirstOrDefault();
            read.Name.ShouldBe(create.Name);

            Console.WriteLine("* UPDATE *");

            var update = Builders<Order>.Update.Set(o => o.Name, "Really big sale");
            orders.UpdateOne(filter, update);

            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.OrderId);
            }

            var read_updated = orders.Find(filter).ToList().FirstOrDefault();
            read_updated.OrderId.ShouldBe(read.OrderId);
            read_updated.Name.ShouldBe("Really big sale");

            Console.WriteLine("* DELETE *");

            orders.DeleteOne(filter);

            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.OrderId);
            }

            orders.CountDocuments(filter).ShouldBe(0);

            Console.WriteLine("* DELETE ALL *");
            orders.DeleteMany(new BsonDocument());

            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.OrderId);
            }

            orders.AsQueryable().Count().ShouldBe(0);

            cli.DropDatabase("Order");
        }

        [Test]
        public void MongoRepository_Supports_Basic_Crud_Operations()
        {
            const string connectionString = "mongodb://127.0.0.1/test";

            if (!MongoDbRepositoryManager.ServerIsRunning(connectionString))
            {
                AssertIgnores.MongoServerIsNotRunning();
            }

            var repo = new MongoDbRepository<Order, string>(connectionString);

            // Create 
            var create = new Order { Name = "Big sale" };
            repo.Add(create);

            // Read 
            var read = repo.Get(create.OrderId);
            read.Name.ShouldBe(create.Name);

            // Update
            read.Name = "Really big sale";
            repo.Update(read);

            var all = repo.GetAll();

            var update = repo.Get(read.OrderId);
            update.OrderId.ShouldBe(read.OrderId);
            update.Name.ShouldBe(read.Name);

            // Delete
            repo.Delete(update);
            var delete = repo.Get(read.OrderId);
            delete.ShouldBeNull();
        }


        [Test]
        public void MongoRepository_Generate_Query_With_AllowDiskSave()
        {
            const string connectionString = "mongodb://127.0.0.1/test";

            if (!MongoDbRepositoryManager.ServerIsRunning(connectionString))
            {
                AssertIgnores.MongoServerIsNotRunning();
            }

            var repo = new MongoDbRepository<Order, string>(connectionString);
            
            // Create 
            var create = new Order { Name = "Big sale" };
            repo.Add(create);
            
            var fetchStrategy = new MongoDbFetchStrategy<Order>
            {
                AllowDiskUse = true
            };

            // Read 
            var read = repo.Get(create.OrderId, fetchStrategy);
            read.Name.ShouldBe(create.Name);

            var all = repo.GetAll(fetchStrategy);
            var all2 = repo.AsQueryable(fetchStrategy);
        }
        
        [Test]
        public void MongoRepository_Collection_Name_Attribute()
        {
            const string connectionString = "mongodb://127.0.0.1/test";

            if (!MongoDbRepositoryManager.ServerIsRunning(connectionString))
            {
                AssertIgnores.MongoServerIsNotRunning();
            }

            var cli = new MongoClient(connectionString);
            cli.DropDatabase("test");

            var repo = new MongoDbRepository<Order, string>(connectionString);

            // Create 
            var create = new Order { Name = "Big sale" };
            repo.Add(create);

            var repoExt = new MongoDbRepository<OrderExtension, string>(connectionString);

            // Create 
            var create2 = new OrderExtension { Name = "Big sale" };
            repoExt.Add(create2);

            Assert.IsTrue(cli.GetDatabase("test").ListCollections().ToList().Any(x => x["name"] == "Order"));
            Assert.IsTrue(cli.GetDatabase("test").ListCollections().ToList().Any(x => x["name"] == "OrderCustomName"));
            Assert.IsFalse(cli.GetDatabase("test").ListCollections().ToList().Any(x => x["name"] == "OrderExtended"));
        }
    }
}