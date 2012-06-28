using System;
using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Traits;
using SharpRepository.Tests.TestObjects;
using Should;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Tests.Traits
{
    [TestFixture]
    public class ICanBatchTraitTests : TestBase
    {
        [Test]
        public void ICanBatch_Exposes_BeginBatch()
        {
            var repo = new ContactRepository();
            using (var batch = repo.BeginBatch())
            {
                batch.BatchActions.Count.ShouldEqual(0);
            }
        }

        private class ContactRepository : InMemoryRepository<Contact, int>, IContactRepository
        {
         
        }

        private interface IContactRepository : ICanBatch<Contact>
        {

        }
    }
}