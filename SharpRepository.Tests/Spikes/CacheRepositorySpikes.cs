using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using SharpRepository.CacheRepository;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Configuration;
using Shouldly;
using System.Collections.Generic;
using System.Linq;

namespace SharpRepository.Tests.Spikes
{
    [TestFixture]
    class CacheRepositorySpikes
    {
        private IRepository<TempTestObject, int> _repository;

        [SetUp]
        public void Initialize()
        {
            var sharpRepositoryConfiguration = new SharpRepositoryConfiguration();
            sharpRepositoryConfiguration.AddCachingStrategy(new StandardCachingStrategyConfiguration("standard"));
            var cachingProviderConfiguration = new InMemoryCachingProviderConfiguration("inmemory", new MemoryCache(new MemoryCacheOptions()));
            sharpRepositoryConfiguration.AddCachingProvider(cachingProviderConfiguration);
            var cachingProvider = cachingProviderConfiguration.GetInstance();
            sharpRepositoryConfiguration.AddRepository(new CacheRepositoryConfiguration("textFilter",  "TextFilter", cachingProvider, "standard", "inmemory"));

            _repository = sharpRepositoryConfiguration.GetInstance<TempTestObject, int>("textFilter");
        }

        [TearDown]
        public void Cleanup()
        {
            _repository.Delete(x => true);
        }

        [Test]
        public void Add_Multiple_Should_Work()
        {
            var list = new List<TempTestObject>()
                       {
                           new TempTestObject() { Name = "Test 1"},
                           new TempTestObject() {Name = "Test 2"},
                           new TempTestObject() {Name = "Test 3"}
                       };
            _repository.Add(list);

            _repository.GetAll().Count().ShouldBe(3);
        }

        [Test]
        public void Add_Multiple_Should_Work2()
        {
            var list = new List<TempTestObject>()
                       {
                           new TempTestObject() {Name = "Test 4"},
                           new TempTestObject() {Name = "Test 5"}
                       };
            _repository.Add(list);

            _repository.GetAll().Count().ShouldBe(2);
        }
    }

    public class TempTestObject
    {
        [RepositoryPrimaryKey]
        public int TestTempKey { get; set; }
        public string Name { get; set; }
    }
}
