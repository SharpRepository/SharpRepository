using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;
using SharpRepository.AzureTableRepository;
using Should;

namespace SharpRepository.Tests.Spikes
{
    [TestFixture]
    public class AzureTableSpikes
    {
//        [Test]
//        public void TestAzureTableGet()
//        {
//            var repo = new AzureTableRepository<PoisonMessage>("UseDevelopmentStorage=true");
//            var item = repo.Get("QueueEmailModel", "722b6bea-d609-48e0-a4af-3ed0f5160ad9");
//
//            item.ShouldNotBeNull();
//        }
    }

    public class PoisonMessage : TableEntity
    {
        public string MessageType { get; set; }
        public string Id { get; set; }
        public int DequeueCount { get; set; }
        public string Message { get; set; }
    }
}
