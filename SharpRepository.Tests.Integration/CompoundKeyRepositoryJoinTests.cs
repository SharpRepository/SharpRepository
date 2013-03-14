using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class CompoundKeyRepositoryJoinTests : TestBase
    {
        // TODO: create a foreign key class to use to do the join with
//        [ExecuteForAllRepositories]
//        public void Join_GetAll_Should_Return_All_Items(ICompoundKeyRepository<User, string, int> repository)
//        {
//            for (var i = 1; i <= 5; i++)
//            {
//                var item = new User { Username = "Test User " + 1, Age = (i % 2) + 1 };
//                repository.Add(item);
//            }
//
//            var userRepository = new InMemoryRepository<User, string, int>();
//            userRepository.Add(new ContactType() { ContactTypeId = 1, Abbreviation = "T1" });
//            userRepository.Add(new ContactType() { ContactTypeId = 2, Abbreviation = "T2" });
//
//            var compositeRepos = repository.Join(contactTypeRepository, c => c.ContactTypeId, ct => ct.ContactTypeId,
//                            (c, ct) => new { Id = c.ContactId, Name = c.Name, TypeAbbrev = ct.Abbreviation });
//
//            var all = compositeRepos.GetAll().ToList();
//
//            all.Count.ShouldEqual(5);
//
//            //IEnumerable<Contact> result = repository.GetAll().ToList();
//            //result.Count().ShouldEqual(5);
//        }
    }
}
