using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class AggregateQueriesTests
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

            var groups = repository.Aggregates.GroupCounts(x => x.ContactTypeId);

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

            var groups = repository.Aggregates.GroupLongCounts(x => x.ContactTypeId);

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

            var groups = repository.Aggregates.GroupItems(x => x.ContactTypeId, x => x.Title);

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

            repository.Aggregates.Count().ShouldEqual(7);
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
            repository.Aggregates.LongCount().ShouldEqual(expected);
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

            repository.Aggregates.Count(x => x.ContactTypeId == 2).ShouldEqual(4);
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
            repository.Aggregates.LongCount(x => x.ContactTypeId == 2).ShouldEqual(expected);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.MongoDb, RepositoryType.CouchDb, Reason = "Not Supported")]
        public void Sum_All_Should_Return_Sum(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =i};
                repository.Add(contact);
            }

            repository.Aggregates.Sum(x => x.ContactTypeId).ShouldEqual(6);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.MongoDb, RepositoryType.CouchDb, Reason = "Not Supported")]
        public void Sum_With_Predicate_Should_Return_Sum(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =i};
                repository.Add(contact);
            }

            repository.Aggregates.Sum(x => x.ContactTypeId, x => x.ContactTypeId > 1).ShouldEqual(5);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.MongoDb, RepositoryType.CouchDb, Reason = "Not Supported")]
        public void Sum_With_Specification_Should_Return_Sum(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =i};
                repository.Add(contact);
            }

            repository.Aggregates.Sum(x => x.ContactTypeId, new Specification<Contact>(x => x.ContactTypeId > 1)).ShouldEqual(5);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.MongoDb, RepositoryType.CouchDb, Reason = "Not Supported")]
        public void Sum_Decimal_All_Should_Return_Sum(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Aggregates.Sum(x => x.SumDecimal).ShouldEqual(7.5m);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.MongoDb, RepositoryType.CouchDb, Reason = "Not Supported")]
        public void Sum_Decimal_With_Predicate_Should_Return_Sum(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Aggregates.Sum(x => x.SumDecimal, x => x.ContactTypeId > 1).ShouldEqual(6m);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.CouchDb, Reason = "Not Supported")]
        public void Min_All_Should_Return_One(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Aggregates.Min(x => x.ContactTypeId).ShouldEqual(1);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.CouchDb, Reason = "Not Supported")]
        public void Min_With_Predicate_Should_Return_Min(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Aggregates.Min(x => x.ContactTypeId, x => x.ContactTypeId > 1).ShouldEqual(2);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.CouchDb, Reason = "Not Supported")]
        public void Min_Decimal_All_Should_Return_Min(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Aggregates.Min(x => x.SumDecimal).ShouldEqual(1.5m);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.CouchDb, Reason = "Not Supported")]
        public void Min_Decimal_With_Predicate_Should_Return_Min(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Aggregates.Min(x => x.SumDecimal, x => x.ContactTypeId > 1).ShouldEqual(2.5m);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.CouchDb, Reason = "Not Supported")]
        public void Max_All_Should_Return_Max(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i};
                repository.Add(contact);
            }

            repository.Aggregates.Max(x => x.ContactTypeId).ShouldEqual(3);
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.CouchDb, Reason = "Not Supported")]
        public void Max_With_Predicate_Should_Return_Max(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i};
                repository.Add(contact);
            }

            repository.Aggregates.Max(x => x.ContactTypeId, x => x.ContactTypeId < 3).ShouldEqual(2);
        }



    }
}
