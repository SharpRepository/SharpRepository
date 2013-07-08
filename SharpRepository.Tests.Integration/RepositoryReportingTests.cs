using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryReportingTests
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

            var groups = repository.Reporting.GroupCounts(x => x.ContactTypeId);

            groups.Count().ShouldEqual(2);
            groups.First().Key.ShouldEqual(1);
            groups.First().Count.ShouldEqual(3);
            groups.Last().Key.ShouldEqual(2);
            groups.Last().Count.ShouldEqual(4);
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

            var groups = repository.Reporting.GroupItems(x => x.ContactTypeId, x => x.Title);

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

            repository.Reporting.Count().ShouldEqual(7);
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

            repository.Reporting.Count(x => x.ContactTypeId == 2).ShouldEqual(4);
        }
    }
}
