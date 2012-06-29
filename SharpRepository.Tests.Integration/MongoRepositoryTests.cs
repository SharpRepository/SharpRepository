using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using NUnit.Framework;
using SharpRepository.MongoDbRepository;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration
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

    [TestFixture]
    public class MongoRepositoryTests : TestBase
    {
        [Test]
        public void Mongo()
        {
            MongoServer server = MongoServer.Create("mongodb://localhost");
            var databaseNames = server.GetDatabaseNames();
            foreach (var db in databaseNames)
            {
                server.DropDatabase(db);    
            }
            
            MongoDatabase database = server.GetDatabase("Order");
            MongoCollection<Order> orders = database.GetCollection<Order>("Order");
            
            Console.WriteLine("* CREATE *");

            var create = new Order { Name = "Big sale" };
            database.GetCollection<Order>("Order").Insert(create);

            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.OrderId);
            }

            Console.WriteLine("* READ *");

            var read = orders.AsQueryable().FirstOrDefault(e => e.OrderId == create.OrderId);
            read.Name.ShouldEqual(create.Name);
            
            Console.WriteLine("* UPDATE *");

            read.Name = "Really big sale";
            database.GetCollection<Order>("Order").Save(read);

            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.OrderId);
            }
            
            var update = database.GetCollection<Order>("Order").AsQueryable().FirstOrDefault(e => e.OrderId == read.OrderId);
            update.OrderId.ShouldEqual(read.OrderId);
            update.Name.ShouldEqual(read.Name);

            Console.WriteLine("* DELETE *");

            var delete = database.GetCollection<Order>("Order").AsQueryable().FirstOrDefault(e => e.OrderId == update.OrderId);

            database.GetCollection<Order>("Order").Remove(Query.EQ("OrderId", delete.OrderId));
            
            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.OrderId);
            }

            database.GetCollection<Order>("Order").RemoveAll();

            Console.WriteLine("* DELETE ALL *");
            
            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.OrderId);
            }

            server.DropDatabase("Order");
        }
        
        [Test]
        public void MongoRepository_Supports_Basic_Crud_Operations()
        {
            const string connectionString = "mongodb://127.0.0.1";
            var repo = new MongoDbRepository<Order, string>(connectionString);
            
            // Create 
            var create = new Order { Name = "Big sale" };
            repo.Add(create);
            
            // Read 
            var read = repo.Get(create.OrderId);
            read.Name.ShouldEqual(create.Name);
            
            // Update
            read.Name = "Really big sale";
            repo.Update(read);

            var all = repo.GetAll();
            
            var update = repo.Get(read.OrderId);
            update.OrderId.ShouldEqual(read.OrderId);
            update.Name.ShouldEqual(read.Name);

            // Delete
            repo.Delete(update);
            var delete = repo.Get(read.OrderId);
            delete.ShouldBeNull();
        }
    }
}