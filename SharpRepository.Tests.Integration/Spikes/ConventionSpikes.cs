using NUnit.Framework;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class ConventionSpikes
    {
        [Test]
        public void Changed_Convention_To_Key_Should_Work()
        {
            var origConvention = DefaultRepositoryConventions.GetPrimaryKeyName;

            DefaultRepositoryConventions.GetPrimaryKeyName = type => type.Name + "Key";
            var repository = new InMemoryRepository<ConventionTestItem1>();

            var testItem1 = new ConventionTestItem1() { Name = "Test1" };
            repository.Add(testItem1);

            testItem1.ConventionTestItem1Key.ShouldNotEqual(0);

            // reset convention to the default orig for the rest of the tests
            DefaultRepositoryConventions.GetPrimaryKeyName = origConvention;
        }

        [Test]
        public void Changed_Convention_To_Key_Should_Work_Just_Key_Property()
        {
            var repository = new InMemoryRepository<ConventionTestItem2>();
            repository.Conventions.GetPrimaryKeyName = _ => "Key";

            var testItem1 = new ConventionTestItem2() { Name = "Test1" };
            repository.Add(testItem1);

            testItem1.Key.ShouldNotEqual(0);
        }

        [Test]
        public void Changed_Convention_To_SomeRandomPrimaryKeyProperty_Should_Work()
        {
            var repository = new InMemoryRepository<ConventionTestItem3>();
            repository.Conventions.GetPrimaryKeyName = _ =>  "SomeRandomPrimaryKeyProperty" ;

            var testItem1 = new ConventionTestItem3() { Name = "Test1" };
            repository.Add(testItem1);

            testItem1.SomeRandomPrimaryKeyProperty.ShouldNotEqual(0);
        }
    }
}
