using System;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Transactions;
using SharpRepository.Tests.TestObjects;
using Shouldly;
using System.Linq;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Tests.Batch
{
    [TestFixture]
    public class BatchTests : TestBase
    {
        [Test]
        public void Batch_Default_Should_Contain_No_Actions()
        {
            var repository = new InMemoryRepository<Contact, Int32>();
            
            using (var batch = repository.BeginBatch())
            {
                batch.BatchActions.Count.ShouldBe(0);
            }
        }
        
        [Test]
        public void Batch_Commit_Should_Reset_Actions()
        {
            var repository = new InMemoryRepository<Contact, Int32>();

            using (var batch = repository.BeginBatch())
            {
                batch.Add(new Contact());
                batch.Commit();
                batch.BatchActions.Count.ShouldBe(0);
            }
        }

        [Test]
        public void Batch_Add_Should_Queue_Add_Action()
        {
            var repository = new InMemoryRepository<Contact, Int32>();
            
            using (var batch = repository.BeginBatch())
            {
                var contact = new Contact();
                batch.Add(contact);
                batch.BatchActions.FirstOrDefault().Item.ShouldBe(contact);
                batch.BatchActions.FirstOrDefault().Action.ShouldBe(BatchAction.Add);
            }
        }

        [Test]
        public void Batch_Update_Should_Queue_Update_Action()
        {
            var repository = new InMemoryRepository<Contact, Int32>();

            using (var batch = repository.BeginBatch())
            {
                var contact = new Contact();
                batch.Update(contact);
                batch.BatchActions.FirstOrDefault().Item.ShouldBe(contact);
                batch.BatchActions.FirstOrDefault().Action.ShouldBe(BatchAction.Update);
            }
        }

        [Test]
        public void Batch_Update_Should_Queue_Delete_Action()
        {
            var repository = new InMemoryRepository<Contact, Int32>();

            using (var batch = repository.BeginBatch())
            {
                var contact = new Contact();
                batch.Delete(contact);
                batch.BatchActions.FirstOrDefault().Item.ShouldBe(contact);
                batch.BatchActions.FirstOrDefault().Action.ShouldBe(BatchAction.Delete);
            }
        }

        [Test]
        public void Batch_Rollback_Should_Reset_Actions()
        {
            var repository = new InMemoryRepository<Contact, Int32>();

            using (var batch = repository.BeginBatch())
            {
                batch.Add(new Contact());
                batch.Rollback();
                batch.BatchActions.Count.ShouldBe(0);
            }
        }
    }
}
