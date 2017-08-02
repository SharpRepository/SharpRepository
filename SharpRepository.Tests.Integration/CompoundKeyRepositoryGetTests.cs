using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class CompoundKeyRepositoryGetTests : TestBase
    {
        [ExecuteForAllCompoundKeyRepositories]
        public void Get_Should_Return_Item_If_Item_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            var item = new User {Username = "Test User", Age = 21};
            repository.Add(item);

            var result = repository.Get(item.Username, item.Age);
            result.Username.ShouldBe(item.Username);
            result.Age.ShouldBe(item.Age);
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Get_Should_Return_Null_If_Item_Does_Not_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            var result = repository.Get(string.Empty, 0);
            result.ShouldBeNull();
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Get_With_String_Selector_Should_Return_Item_If_Item_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            var item = new User { Username = "Test User", Age = 21 };
            repository.Add(item);

            var result = repository.Get(item.Username, item.Age, c => c.Username);
            result.ShouldBe("Test User");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Get_With_Int_Selector_Should_Return_Item_If_Item_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            var item = new User { Username = "Test User", Age = 21 };
            repository.Add(item);

            var result = repository.Get(item.Username, item.Age, c => c.Age);
            result.ShouldBe(21);
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Get_With_Anonymous_Class_Selector_Should_Return_Item_If_Item_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            var item = new User { Username = "Test User", Age = 21 };
            repository.Add(item);

            var result = repository.Get(item.Username, item.Age, c => new { c.Age, c.Username });
            result.Age.ShouldBe(21);
            result.Username.ShouldBe("Test User");
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Get_With_String_Selector_Should_Return_Default_If_Item_Does_Not_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            var result = repository.Get(string.Empty, 0, c => c.Username);
            result.ShouldBe(default(string));
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Get_With_Int_Selector_Should_Return_Default_If_Item_Does_Not_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            var result = repository.Get(string.Empty, 0, c => c.Age);
            result.ShouldBe(default(int));
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Get_With_Anonymouse_Class_Selector_Should_Return_Null_If_Item_Does_Not_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            var result = repository.Get(string.Empty, 0, c => new { c.Age, c.Username });
            result.ShouldBeNull();
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void TryGet_Should_Return_True_And_Item_If_Item_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            var item = new User { Username = "Test User", Age = 21 };
            repository.Add(item);

            repository.TryGet(item.Username, item.Age, out User result).ShouldBeTrue();
            result.Username.ShouldBe(item.Username);
            result.Age.ShouldBe(item.Age);
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void TryGet_Should_Return_False_And_Null_If_Item_Does_Not_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            repository.TryGet(string.Empty, 0, out User result).ShouldBeFalse();
            result.ShouldBeNull();
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void TryGet_Should_Return_True_If_Item_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            var item = new User { Username = "Test User", Age = 21 };
            repository.Add(item);

            repository.Exists(item.Username, item.Age).ShouldBeTrue();
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void TryGet_Should_Return_False_If_Item_Does_Not_Exists(ICompoundKeyRepository<User, string, int> repository)
        {
            repository.Exists(string.Empty, 0).ShouldBeFalse();
        }
    }
}