using SharpRepository.InMemoryRepository;
using NUnit.Framework;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Misc
{
    [TestFixture]
    public class MiscTests
    {
        [Test]
        public void EntityType_Returns_Proper_Type()
        {
            var repo = new InMemoryRepository<Contact>();
            repo.EntityType.ShouldEqual(typeof(Contact));
        }

        [Test]
        public void KeyType_Returns_Proper_Type()
        {
            var repo = new InMemoryRepository<Contact, int>();
            repo.KeyType.ShouldEqual(typeof(int));
        }

        [Test]
        public void KeyType_Implied_Returns_Proper_Type()
        {
            var repo = new InMemoryRepository<Contact>();
            repo.KeyType.ShouldEqual(typeof(int));
        }
    }
}
