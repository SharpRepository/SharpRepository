using NUnit.Framework;
using SharpRepository.Repository;
using System.Linq;
using SharpRepository.Repository.Transactions;
using Shouldly;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Samples
{
    [TestFixture]
    public class HowToBatch
    {
       /*
        * What is a Batch?
        *
        * RepositoryBase doesn't manage a unit of work which is only 
        * persisted when a Save operation is explicitly executed. Instead 
        * the repository forces a Save following any Add, Update and Delete 
        * operation. 
        * 
        * For example, when the EfRepository.Add() method is called, 
        * the repository adds the entity to context and then context changes 
        * are immediatey saved so the entity is persisted to the database. 
        * 
        * Though this eager saving pattern is very convenient, you may need 
        * deal with multiple operation as a single unit of work. That's where 
        * batching comes in.  A batch is a queued set of repository operations 
        * which can be committed (saved/persisted) or rolled back (disregarded)
        * as a single unit of work. 
        * 
        * Does Batch == Transaction?        
        * 
        * Yikes! You might be thinking that batches are transactions. Don't
        * be fooled! Batching is a queuing mechanism. A batch merely groups 
        * queued CRUD operations into a common save routine. Though a 
        * transaction guarentees that all work will be rollbacked if one 
        * operation fails, that's not necessarily the case with a batch (unless
        * the repository offers that magic.) 
        * 
        * Why Should I Use Batch?        
        * 
        * It's convenient and it may offer better performance since your
        * reducing your persistance operations to a single call.
        * 
        * How Do I Batch?
        * 
        * Three repository operations may be scoped to a given batch:
        * Add, Update, and Delete. Once the batch is queued, it can be
        * committed (saved/persisted) or rolled back (disregarded.) 
        *          
        * Let's work through a few examples with Orders and a OrderRepository         
        */
        public class Order
        {
            public int OrderId{ get; set; }
            public string Name { get; set; }
        }
        
       /*
        * Batches are repository specific so our OrderRepository provides
        * a new Order Batch.
        */
        [Test]
        public void Repository_Provides_New_Batch()
        {
            var repo = new InMemoryRepository<Order, int>();
            IBatch<Order> batch = repo.BeginBatch();
            batch.ShouldNotBeNull();
        }

        /*
         * Batches are IDisposable types so a using statement may define 
         * your batch scope 
         */
        [Test]
        public void Batch_Is_IDisposable_Type()
        {
            var repo = new InMemoryRepository<Order, int>();
            using (var batch = repo.BeginBatch())
            {
                batch.ShouldNotBeNull();
            }
        }

        /*
         * Add, Update and Delete operations may be batched.
         * Batched operations are queued and only persisted 
         * when the batch is committed. 
         */
        [Test]
        public void Add_Operations_Can_Be_Batched()
        {
            var repo = new InMemoryRepository<Order, int>();

            using (var batch = repo.BeginBatch())
            {
                batch.Add(new Order { Name = "Order 1" });
                batch.Add(new Order { Name = "Order 2" });
                repo.GetAll().Count().ShouldBe(0);

                batch.Commit();
            }

            repo.GetAll().Count().ShouldBe(2);
        }
        
        [Test]
        public void Update_Operations_Can_Be_Batched()
        {
            var repo = new InMemoryRepository<Order, int>();
            repo.Add(new Order { Name = "Order 1" });
            repo.Add(new Order { Name = "Order 2" });
            repo.GetAll().Count().ShouldBe(2);

            using (var batch = repo.BeginBatch())
            {
                foreach (var order in repo.GetAll())
                {
                    order.Name = "Updated";
                    batch.Update(order);
                }
                
                repo.GetAll().Count().ShouldBe(2);
                repo.GetAll().Count(x => x.Name.StartsWith("Update")).ShouldBe(0);

                batch.Commit();
            }

            repo.GetAll().Count().ShouldBe(2);
            repo.GetAll().Count(x => x.Name.StartsWith("Update")).ShouldBe(2);
        }
        
        [Test]
        public void Delete_Operations_Can_Be_Batched()
        {
            var repo = new InMemoryRepository<Order, int>();

            repo.Add(new Order { Name = "Order 1" });
            repo.Add(new Order { Name = "Order 2" });

            repo.GetAll().Count().ShouldBe(2);

            using (var batch = repo.BeginBatch())
            {
                foreach (var order in repo.GetAll().ToList())
                {
                    batch.Delete(order);
                }

                repo.GetAll().Count().ShouldBe(2);
                batch.Commit();
            }

            repo.GetAll().Count().ShouldBe(0);
        }

        /*
         * Batched operations may be queued and rolled back.         
         */
        [Test]
        public void Batch_Can_Be_Rolled_Back()
        {
            var repo = new InMemoryRepository<Order, int>();

            using (var batch = repo.BeginBatch())
            {
                batch.Add(new Order { Name = "Order 1" });
                batch.Add(new Order { Name = "Order 2" });
                repo.GetAll().Count().ShouldBe(0);
                batch.Rollback();
                batch.Commit();
            }

            repo.GetAll().Count().ShouldBe(0);
        }

        /*
         * Batched operations are implicitly rolled back if no commit
         * or rollback action is taken and the batch is disposed.
         */
        [Test]
        public void Batch_Can_Be_Disregarded()
        {
            var repo = new InMemoryRepository<Order, int>();

            using (var batch = repo.BeginBatch())
            {
                batch.Add(new Order { Name = "Order 1" });
                batch.Add(new Order { Name = "Order 2" });
            }

            repo.GetAll().Count().ShouldBe(0);
        }
    }
}