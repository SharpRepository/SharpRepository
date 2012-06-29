using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;
using SharpRepository.MongoDbRepository;

namespace SharpRepository.Tests.Integration
{
    public class Order
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }

    [TestFixture]
    public class MongoRepositoryTests : TestBase
    {
        [Test]
        public void Mongo()
        {
            MongoServer server = MongoServer.Create("mongodb://localhost");
            server.DropDatabase("Order");
            MongoDatabase database = server.GetDatabase("Order");
            MongoCollection<Order> orders = database.GetCollection<Order>("Order");
            
            Console.WriteLine("* CREATE *");

            var create = new Order { Name = "Big sale" };
            database.GetCollection<Order>("Order").Insert(create);

            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.Id);
            }

            Console.WriteLine("* READ *");

            var read = orders.AsQueryable().FirstOrDefault(e => e.Id == create.Id);
            read.Name.ShouldEqual(create.Name);
            
            Console.WriteLine("* UPDATE *");

            read.Name = "Really big sale";
            database.GetCollection<Order>("Order").Save(read);

            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.Id);
            }
            
            var update = database.GetCollection<Order>("Order").AsQueryable().FirstOrDefault(e => e.Id == read.Id);
            update.Id.ShouldEqual(read.Id);
            update.Name.ShouldEqual(read.Name);

            Console.WriteLine("* DELETE *");

            var delete = database.GetCollection<Order>("Order").AsQueryable().FirstOrDefault(e => e.Id == update.Id);

            database.GetCollection<Order>("Order").Remove(Query.EQ("_id", delete.Id));
            
            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.Id);
            }

            database.GetCollection<Order>("Order").RemoveAll();

            Console.WriteLine("* DELETE ALL *");
            
            foreach (var order in database.GetCollection<Order>("Order").AsQueryable())
            {
                Console.WriteLine(order.Name + ", " + order.Id);
            }

            server.DropDatabase("Order");
        }
        
        [Test]
        public void MongoRepository_Supports_Basic_Crud_Operations()
        {
            const string connectionString = "mongodb://127.0.0.1";
            var repo = new MongoDbRepository<Order, ObjectId>(connectionString);
            
            // Create 
            var create = new Order { Name = "Big sale" };
            repo.Add(create);
            
            // Read 
            var read = repo.Get(create.Id);
            read.Name.ShouldEqual(create.Name);
            
            // Update
            read.Name = "Really big sale";
            repo.Update(read);
            
            var update = repo.Get(read.Id);
            update.Id.ShouldEqual(read.Id);
            update.Name.ShouldEqual(read.Name);

            // Delete
            repo.Delete(update);
            var delete = repo.Get(read.Id);
            delete.ShouldBeNull();
        }
    }
}