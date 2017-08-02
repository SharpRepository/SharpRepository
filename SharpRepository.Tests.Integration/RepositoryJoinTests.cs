using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryJoinTests : TestBase
    {
        [ExecuteForAllRepositories]
        public void Join_GetAll_Should_Return_All_Items(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 5; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = (i % 2) + 1};
                repository.Add(contact);
            }

            var contactTypeRepository = new InMemoryRepository<ContactType, int>();
            contactTypeRepository.Add(new ContactType() { ContactTypeId = 1, Abbreviation = "T1"});
            contactTypeRepository.Add(new ContactType() { ContactTypeId = 2, Abbreviation = "T2" });

            var compositeRepos = repository.Join(contactTypeRepository, c => c.ContactTypeId, ct => ct.ContactTypeId,
                            (c, ct) => new { Id = c.ContactId, Name = c.Name, TypeAbbrev = ct.Abbreviation });

            var all = compositeRepos.GetAll().ToList();

            all.Count.ShouldBe(5);

            //IEnumerable<Contact> result = repository.GetAll().ToList();
            //result.Count().ShouldBe(5);
        }
    }
}
