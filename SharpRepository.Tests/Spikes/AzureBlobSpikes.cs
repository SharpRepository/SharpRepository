using System;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;
using SharpRepository.AzureBlobRepository;
using Should;

namespace SharpRepository.Tests.Spikes
{
    [TestFixture]
    public class AzureBlobSpikes
    {
//        [Test]
//        public void TestAzureBlobGet()
//        {
////            var repo = new AzureBlobRepository<TestBlob, Guid>("UseDevelopmentStorage=true", "course2015", false);
////            var item = repo.Get(Guid.Parse("722b6bea-d609-48e0-a4af-3ed0f5160ad9"));
////
////            item.ShouldNotBeNull();
//
//            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse("UseDevelopmentStorage=true");
//            var blobClient = storageAccount.CreateCloudBlobClient();
//            var container = blobClient.GetContainerReference("course2015");
//
//            var blob = container.GetBlockBlobReference("722b6bea-d609-48e0-a4af-3ed0f5160ad9");
//            var text = blob.DownloadText();
//
//            text.ShouldNotBeNull();
//        }
    }

    public class TestBlob
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
