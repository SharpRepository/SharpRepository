using System.Linq;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class AggregateQueriesTests
    {
        [ExecuteForAllRepositoriesExcept("GroupCount_Should_Return_Proper_Counts", RepositoryType.MongoDb, Reason = "GroupBy Not Supported")]
        public void GroupCount_Should_Return_Proper_Counts(IRepository<Contact, string> repository)
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

            var groups = repository.GroupCount(x => x.ContactTypeId);

            groups.Count().ShouldBe(2);
            groups[1].ShouldBe(3);
            groups[2].ShouldBe(4);
        }

        [ExecuteForAllRepositoriesExcept("GroupLongCount_Should_Return_Proper_Counts", RepositoryType.MongoDb, Reason = "GroupBy Not Supported")]
        public void GroupLongCount_Should_Return_Proper_Counts(IRepository<Contact, string> repository)
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

            var groups = repository.GroupLongCount(x => x.ContactTypeId);

            groups.Count().ShouldBe(2);

            const long expected1 = 3;
            const long expected2 = 4;
            groups[1].ShouldBe(expected1);
            groups[2].ShouldBe(expected2);
        }

        [ExecuteForAllRepositoriesExcept("Group_Should_Return_Proper_Items", RepositoryType.MongoDb, Reason = "GroupBy Not Supported")]
        public void Group_Should_Return_Proper_Items(IRepository<Contact, string> repository)
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

            var groups = repository.GroupBy(new Specification<Contact>(x => x.ContactTypeId > 0), x => x.ContactTypeId,
                                          x => new { ContactTypeId = x.Key, Count = x.Count(), Average = x.Average(o => o.ContactTypeId) });

            //var groups = repository.GroupItems(x => x.ContactTypeId, x => x.Title);
            groups.First().ContactTypeId.ShouldBe(1);
            groups.First().Count.ShouldBe(3);
            groups.First().Average.ShouldBe(1.0);
            groups.Last().ContactTypeId.ShouldBe(2);
            groups.Last().Count.ShouldBe(4);
            groups.Last().Average.ShouldBe(2.0);
        }

        [ExecuteForAllRepositories("Count_Should_Return_All_Count")]
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

            repository.Count().ShouldBe(7);
        }

        [ExecuteForAllRepositories("LongCount_Should_Return_All_Count")]
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
            repository.LongCount().ShouldBe(expected);
        }

        [ExecuteForAllRepositories("Count_With_Predicate_Should_Return_Count")]
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

            repository.Count(x => x.ContactTypeId == 2).ShouldBe(4);
        }

        [ExecuteForAllRepositories("LongCount_With_Predicate_Should_Return_Count")]
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
            repository.LongCount(x => x.ContactTypeId == 2).ShouldBe(expected);
        }

        [ExecuteForAllRepositories("Sum_All_Should_Return_Sum")]
        public void Sum_All_Should_Return_Sum(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =i};
                repository.Add(contact);
            }

            repository.Sum(x => x.ContactTypeId).ShouldBe(6);
        }

        [ExecuteForAllRepositories("Sum_With_Predicate_Should_Return_Sum")]
        public void Sum_With_Predicate_Should_Return_Sum(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =i};
                repository.Add(contact);
            }

            repository.Sum(x => x.ContactTypeId > 1, x => x.ContactTypeId).ShouldBe(5);
        }

        [ExecuteForAllRepositories("Sum_With_Specification_Should_Return_Sum")]
        public void Sum_With_Specification_Should_Return_Sum(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =i};
                repository.Add(contact);
            }

            repository.Sum(new Specification<Contact>(x => x.ContactTypeId > 1), x => x.ContactTypeId).ShouldBe(5);
        }

        [ExecuteForAllRepositories("Sum_Decimal_All_Should_Return_Sum")]
        public void Sum_Decimal_All_Should_Return_Sum(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Sum(x => x.SumDecimal).ShouldBe(7.5m);
        }

        [ExecuteForAllRepositories("Sum_Decimal_With_Predicate_Should_Return_Sum")]
        public void Sum_Decimal_With_Predicate_Should_Return_Sum(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Sum(x => x.ContactTypeId > 1, x => x.SumDecimal).ShouldBe(6m);
        }

        [ExecuteForAllRepositories("Average_All_Should_Return_Average")]
        public void Average_All_Should_Return_Average(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i};
                repository.Add(contact);
            }

            repository.Average(x => x.ContactTypeId).ShouldBe(2.0);
        }

        [ExecuteForAllRepositories("Average_With_Predicate_Should_Return_Average")]
        public void Average_With_Predicate_Should_Return_Average(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =i};
                repository.Add(contact);
            }

            repository.Average(x => x.ContactTypeId > 1, x => x.ContactTypeId).ShouldBe(2.5);
        }

        [ExecuteForAllRepositories("Average_With_Specification_Should_Return_Average")]
        public void Average_With_Specification_Should_Return_Average(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId =i};
                repository.Add(contact);
            }

            repository.Average(new Specification<Contact>(x => x.ContactTypeId > 1), x => x.ContactTypeId).ShouldBe(2.5);
        }

        [ExecuteForAllRepositories("Average_Decimal_All_Should_Return_Average")]
        public void Average_Decimal_All_Should_Return_Average(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Average(x => x.SumDecimal).ShouldBe(2.5m);
        }

        [ExecuteForAllRepositories("Average_Decimal_With_Predicate_Should_Return_Average")]
        public void Average_Decimal_With_Predicate_Should_Return_Average(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Average(x => x.ContactTypeId > 1, x => x.SumDecimal).ShouldBe(3m);
        }

        [ExecuteForAllRepositories("Min_All_Should_Return_One")]
        public void Min_All_Should_Return_One(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Min(x => x.ContactTypeId).ShouldBe(1);
        }

        [ExecuteForAllRepositories("Min_With_Predicate_Should_Return_Min")]
        public void Min_With_Predicate_Should_Return_Min(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Min(x => x.ContactTypeId > 1, x => x.ContactTypeId).ShouldBe(2);
        }

        [ExecuteForAllRepositories("Min_Decimal_All_Should_Return_Min")]
        public void Min_Decimal_All_Should_Return_Min(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Min(x => x.SumDecimal).ShouldBe(1.5m);
        }

        [ExecuteForAllRepositories("Min_Decimal_With_Predicate_Should_Return_Min")]
        public void Min_Decimal_With_Predicate_Should_Return_Min(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i, SumDecimal = 0.5m + i };
                repository.Add(contact);
            }

            repository.Min(x => x.ContactTypeId > 1, x => x.SumDecimal).ShouldBe(2.5m);
        }

        [ExecuteForAllRepositories("Max_All_Should_Return_Max")]
        public void Max_All_Should_Return_Max(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i};
                repository.Add(contact);
            }

            repository.Max(x => x.ContactTypeId).ShouldBe(3);
        }

        [ExecuteForAllRepositories("Max_With_Predicate_Should_Return_Max")]
        public void Max_With_Predicate_Should_Return_Max(IRepository<Contact, string> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                var contact = new Contact { Name = "Test User " + i, ContactTypeId = i};
                repository.Add(contact);
            }

            repository.Max(x => x.ContactTypeId < 3, x => x.ContactTypeId).ShouldBe(2);
        }
    }
}
