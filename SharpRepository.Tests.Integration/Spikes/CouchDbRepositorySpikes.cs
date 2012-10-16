using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpRepository.CouchDbRepository;
using SharpRepository.MongoDbRepository;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class CouchDbRepositorySpikes : TestBase
    {
        [Test]
        public void Test_SharpCouch()
        {
//            var server = new CouchDb();
//            var dbs = server.GetDatabases("http://localhost:5984");
//
//            dbs.Count.ShouldBeInRange(1, 100);
        }

        [Test]
        public void CouchRepository_Supports_Basic_Crud_Operations()
        {
            if (!CouchDbRepositoryManager.ServerIsRunning())
            {
                AssertIgnores.CouchDbServerIsNotRunning();
            }

            var repo = new CouchDbRepository<Order>();

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
