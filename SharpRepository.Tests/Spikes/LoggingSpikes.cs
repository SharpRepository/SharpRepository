using NUnit.Framework;
using SharpRepository.Logging.Log4net;
using SharpRepository.Logging.NLog;
using SharpRepository.Tests.TestObjects;
using SharpRepository.InMemoryRepository;

namespace SharpRepository.Tests.Spikes
{
    [TestFixture]
    public class LoggingSpikes : TestBase
    {
        [Test]
        public void Logging_Via_Aspects()
        {
            var repository = new InMemoryRepository<Contact, int>();
//            repository.Aspects.Add(new Log4NetRepositoryLog<Contact, int>());
//            repository.Aspects.Add(new NLogRepositoryLogger<Contact, int>());


            var contact1 = new Contact() {Name = "Contact 1"};
            repository.Add(contact1);
            repository.Add(new Contact() { Name = "Contact 2"});
            repository.Add(new Contact() { Name = "Contact 3"});

            contact1.Name += " EDITED";
            repository.Update(contact1);

            repository.Delete(2);
        }
    }
}
