using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.TestObjects;
using Should;

namespace SharpRepository.Tests.Enumerable
{
    [TestFixture]
    public class ForEachTests
    {
        [Test]
        public void ForEach_Should_Loop_Through_All()
        {
            var i = 0;
            var repository = new InMemoryRepository<Contact, int>();
            repository.Add(new Contact());
            repository.Add(new Contact());
            repository.Add(new Contact());
            repository.Add(new Contact());
            repository.Add(new Contact());

            foreach (var contact in repository)
            {
                i++;
                contact.ContactId.ShouldEqual(i); // just making sure it's getting each item separately
            }

            i.ShouldEqual(5);
        }
    }
}
