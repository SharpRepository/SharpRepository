using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Queries;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class CompoundKeyRepositoryFindTests : TestBase
    {
        [ExecuteForAllCompoundKeyRepositories]
        public void Find_Should_Return_Single_Item_Which_Satisfies_Specification(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.Find(new Specification<User>(p => p.Username == "Test User 1"));
            result.Username.ShouldBe("Test User 1");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Find_Should_Return_Single_Item_Which_Satisfies_Predicate(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.Find(p => p.Username == "Test User 1");
            result.Username.ShouldBe("Test User 1");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Find_Should_Return_Single_Item_Which_Satisfies_Composite_Specification(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.Find(new Specification<User>(p => p.Username == "Test User 1").OrElse(new Specification<User>(p => p.Username == "Test User 1000")));
            result.Username.ShouldBe("Test User 1");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Find_Should_Return_Single_Item_Which_Satisfies_Composite_Predicate(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.Find(p => p.Username == "Test User 1" || p.Username == "Test User 1000");
            result.Username.ShouldBe("Test User 1");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Find_Should_Return_First_Ordered_Item_Which_Satisfies_Specification(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.Find(new Specification<User>(p => p.Username.StartsWith("Test")), new SortingOptions<User>("Username", true));
            result.Username.ShouldBe("Test User 3");

            var result2 = repository.Find(new Specification<User>(p => p.Username.StartsWith("Test")), new SortingOptions<User>("Username", false));
            result2.Username.ShouldBe("Test User 1");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Find_Should_Return_First_Ordered_Item_Which_Satisfies_Predicate(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.Find(p => p.Username.StartsWith("Test"), new SortingOptions<User>("Username", true));
            result.Username.ShouldBe("Test User 3");

            var result2 = repository.Find(p => p.Username.StartsWith("Test"), new SortingOptions<User>("Username", false));
            result2.Username.ShouldBe("Test User 1");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Find_Should_Return_First_Ordered_Item_Which_Satisfies_Specification_WIth_Sorting_Predicate(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.Find(new Specification<User>(p => p.Username.StartsWith("Test")), new SortingOptions<User, string>(c => c.Username, true));
            result.Username.ShouldBe("Test User 3");

            var result2 = repository.Find(new Specification<User>(p => p.Username.StartsWith("Test")), new SortingOptions<User, string>(c => c.Username, false));
            result2.Username.ShouldBe("Test User 1");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Find_Should_Return_First_Ordered_Item_Which_Satisfies_Predicate_WIth_Sorting_Predicate(ICompoundKeyRepository<User, string, int> repository)
        {
            for (var i = 1; i <= 3; i++)
            {
                repository.Add(new User { Username = "Test User " + i, Age = i });
            }

            var result = repository.Find(p => p.Username.StartsWith("Test"), new SortingOptions<User, string>(c => c.Username, true));
            result.Username.ShouldBe("Test User 3");

            var result2 = repository.Find(p => p.Username.StartsWith("Test"), new SortingOptions<User, string>(c => c.Username, false));
            result2.Username.ShouldBe("Test User 1");
        }
    }
}