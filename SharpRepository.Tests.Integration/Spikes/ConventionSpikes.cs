using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class ConventionSpikes
    {
        [Test]
        public void Changed_Default_Suffix_To_Key_Should_Work()
        {
            var origSuffix = DefaultRepositoryConventions.PrimaryKeySuffix;

            DefaultRepositoryConventions.PrimaryKeySuffix = "Key";
            var repository = new InMemoryRepository<ConventionTestItem1>();

            var item = new ConventionTestItem1() { Name = "Test1" };
            repository.Add(item);

            // The PK should have been found and updated so it's not zero anymore
            item.ConventionTestItem1Key.ShouldNotBe(0);

            // reset convention to the default orig for the rest of the tests
            DefaultRepositoryConventions.PrimaryKeySuffix = origSuffix;
        }

        [Test]
        public void Changed_Convention_To_Key_Should_Work()
        {
            var origConvention = DefaultRepositoryConventions.GetPrimaryKeyName;

            DefaultRepositoryConventions.GetPrimaryKeyName = type => type.Name + "Key";
            var repository = new InMemoryRepository<ConventionTestItem1>();

            var item = new ConventionTestItem1() { Name = "Test1" };
            repository.Add(item);

            // The PK should have been found and updated so it's not zero anymore
            item.ConventionTestItem1Key.ShouldNotBe(0);

            // reset convention to the default orig for the rest of the tests
            DefaultRepositoryConventions.GetPrimaryKeyName = origConvention;
        }

        [Test]
        public void Changed_Convention_To_Key_Should_Work_Just_Key_Property()
        {
            var repository = new InMemoryRepository<ConventionTestItem2>();
            repository.Conventions.GetPrimaryKeyName = _ => "Key";

            var item = new ConventionTestItem2() { Name = "Test1" };
            repository.Add(item);

            // The PK should have been found and updated so it's not zero anymore
            item.Key.ShouldNotBe(0);
        }

        [Test]
        public void Changed_Convention_To_SomeRandomPrimaryKeyProperty_Should_Work()
        {
            var repository = new InMemoryRepository<ConventionTestItem3>();
            repository.Conventions.GetPrimaryKeyName = _ =>  "SomeRandomPrimaryKeyProperty" ;

            var item = new ConventionTestItem3() { Name = "Test1" };
            repository.Add(item);

            // The PK should have been found and updated so it's not zero anymore
            item.SomeRandomPrimaryKeyProperty.ShouldNotBe(0);
        }
    }
}
