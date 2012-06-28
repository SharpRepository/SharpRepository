using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryJoinTests : TestBase
    {
        [ExecuteForAllRepositories]
        public void Join_GetAll_Should_Return_All_Items(IRepository<Contact, int> repository)
        {
            for (int i = 1; i <= 5; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = (i % 2) + 1};
                repository.Add(contact);
            }

            var contactTypeRepository = new InMemoryRepository<ContactType, int>();
            contactTypeRepository.Add(new ContactType() { ContactTypeId = 1, Abbreviation = "T1"});
            contactTypeRepository.Add(new ContactType() { ContactTypeId = 2, Abbreviation = "T2" });
            
            var compositeRepos = repository.Join<int, ContactType, ContactTypeResult, int>(contactTypeRepository, c => c.ContactTypeId, ct => ct.ContactTypeId,
                            (c, ct) => new ContactTypeResult { Id = c.ContactId, Name = c.Name, TypeAbbrev = ct.Abbreviation });

            var all = compositeRepos.GetAll().ToList();

            all.Count.ShouldEqual(5);

            //IEnumerable<Contact> result = repository.GetAll().ToList();
            //result.Count().ShouldEqual(5);
        }

        public class ContactTypeResult
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string TypeAbbrev { get; set; }
        }
    }
}
