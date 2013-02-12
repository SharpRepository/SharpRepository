using NUnit.Framework;
using SharpRepository.Tests.TestObjects;
using Should;
using SharpRepository.InMemoryRepository;

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
            }

            i.ShouldEqual(5);
        }
    }
}
