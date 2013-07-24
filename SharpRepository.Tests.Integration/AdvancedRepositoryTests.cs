using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class AdvancedRepositoryTests
    {
        [ExecuteForAllRepositories]
        public void GroupCounts_Should_Return_Proper_Counts(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =1};
                repository.Add(contact);
            }
            for (var i = 4; i <= 7; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = 2};
                repository.Add(contact);
            }

            var groups = repository.Advanced.GroupCounts(x => x.ContactTypeId);

            groups.Count().ShouldEqual(2);
            groups[1].ShouldEqual(3);
            groups[2].ShouldEqual(4);
        }

        [ExecuteForAllRepositories]
        public void GroupLongCounts_Should_Return_Proper_Counts(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =1};
                repository.Add(contact);
            }
            for (var i = 4; i <= 7; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = 2};
                repository.Add(contact);
            }

            var groups = repository.Advanced.GroupLongCounts(x => x.ContactTypeId);

            groups.Count().ShouldEqual(2);

            const long expected1 = 3;
            const long expected2 = 4;
            groups[1].ShouldEqual(expected1);
            groups[2].ShouldEqual(expected2);
        }

        [ExecuteForAllRepositories]
        public void GroupItems_Should_Return_Proper_Items(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =1};
                repository.Add(contact);
            }
            for (var i = 4; i <= 7; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = 2};
                repository.Add(contact);
            }

            var groups = repository.Advanced.GroupItems(x => x.ContactTypeId, x => x.Title);

            groups.Count().ShouldEqual(2);
            groups.First().Key.ShouldEqual(1);
            groups.First().Items.Count().ShouldEqual(3);
            groups.Last().Key.ShouldEqual(2);
            groups.Last().Items.Count().ShouldEqual(4);
        }

        [ExecuteForAllRepositories]
        public void Count_Should_Return_All_Count(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =1};
                repository.Add(contact);
            }
            for (var i = 4; i <= 7; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = 2};
                repository.Add(contact);
            }

            repository.Advanced.Count().ShouldEqual(7);
        }

        [ExecuteForAllRepositories]
        public void LongCount_Should_Return_All_Count(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =1};
                repository.Add(contact);
            }
            for (var i = 4; i <= 7; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = 2};
                repository.Add(contact);
            }

            const long expected = 7;
            repository.Advanced.LongCount().ShouldEqual(expected);
        }

        [ExecuteForAllRepositories]
        public void Count_With_Predicate_Should_Return_Count(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =1};
                repository.Add(contact);
            }
            for (var i = 4; i <= 7; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = 2};
                repository.Add(contact);
            }

            repository.Advanced.Count(x => x.ContactTypeId == 2).ShouldEqual(4);
        }

        [ExecuteForAllRepositories]
        public void LongCount_With_Predicate_Should_Return_Count(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =1};
                repository.Add(contact);
            }
            for (var i = 4; i <= 7; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = 2};
                repository.Add(contact);
            }

            const long expected = 4;
            repository.Advanced.LongCount(x => x.ContactTypeId == 2).ShouldEqual(expected);
        }
    }
}
