using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository.Traits;
using SharpRepository.Tests.TestObjects;
using Shouldly;

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
                batch.BatchActions.Count.ShouldBe(0);
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