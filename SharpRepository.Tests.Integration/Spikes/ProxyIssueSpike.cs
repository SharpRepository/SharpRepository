using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using NUnit.Framework;
using SharpRepository.Ef5Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.DynamicProxyObjects;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class ProxyIssueSpike
    {
        [Test]
        public void DynamaicProxyTest()
        {
            var dbContext = new SharpRepositoryTestEntities();
            var repository = new Ef5Repository<MainItem, int>(dbContext, new StandardCachingStrategy<MainItem, int>(new InMemoryCachingProvider()));


            // first time get, puts it into cache
            var item = repository.Get(2);

            var repository2 = new Ef5Repository<MainItem, int>(new SharpRepositoryTestEntities(), new StandardCachingStrategy<MainItem, int>(new InMemoryCachingProvider()));

            // second get will get the dynamic proxy from cache
            item = repository2.Get(2);

            item.Name = "UPDATED ITEM";
            repository2.Update(item); // this will throw an error when it's a dynamic proxy object
        }

        [Test]
        public void DynamaicProxyTest2()
        {
            var dbContext = new SharpRepositoryTestEntities();
            var repository = new Ef5Repository<MainItem, int>(dbContext, new StandardCachingStrategy<MainItem, int>(new InMemoryCachingProvider()));


            // first time get, puts it into cache
            var items = repository.GetAll();

            var repository2 = new Ef5Repository<MainItem, int>(new SharpRepositoryTestEntities(), new StandardCachingStrategy<MainItem, int>(new InMemoryCachingProvider()));

            // second get will get the dynamic proxy from cache
            items = repository2.GetAll();

            var item = items.First(x => x.MainItemId == 2);
            item.Name = "UPDATED ITEM2";
            repository2.Update(item); // this will throw an error when it's a dynamic proxy object
        }
    }
}
