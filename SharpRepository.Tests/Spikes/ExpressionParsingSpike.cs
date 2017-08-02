using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.Specifications;
using SharpRepository.Tests.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Spikes
{
    [TestFixture]
    public class ExpressionParsingSpike : TestBase
    {
        private ICachingProvider cacheProvider;

        [SetUp]
        public void Setup()
        {
            cacheProvider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions()));
        }

        [Test]
        public void Get_Entity_Partition_Value()
        {
            var contact1 = new Contact { ContactId = 1, ContactTypeId = 1 };
            var contact2 = new Contact { ContactId = 1, ContactTypeId = 2 };
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact>(cacheProvider, c => c.ContactTypeId);

            cachingStrategy.TryPartitionValue(contact1, out int contactId);
            contactId.ShouldBe(1);

            cachingStrategy.TryPartitionValue(contact2, out contactId);
            contactId.ShouldBe(2);
        }

        [Test]
        public void Single_Part_Predicate_Constant_On_Right_Should_Not_Match()
        {
            var spec = new Specification<Contact>(contact => contact.Name == "test");
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(false);
            value.ShouldBe(0);
        }

        [Test]
        public void Single_Part_Predicate_GreaterThan_Should_Not_Match()
        {
            var spec = new Specification<Contact>(contact => contact.ContactTypeId > 1);
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(false);
            value.ShouldBe(0);
        }

        [Test]
        public void Single_Part_Predicate_NotEqual_Should_Not_Match()
        {
            var spec = new Specification<Contact>(contact => contact.ContactTypeId != 1);
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(false);
            value.ShouldBe(0);
        }

        [Test]
        public void Single_Part_Predicate_Constant_On_Right_Should_Match()
        {
            var spec = new Specification<Contact>(contact => contact.ContactTypeId == 1);
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Single_Part_Predicate_Using_Equals_Method_On_Right_Should_Match()
        {
            var spec = new Specification<Contact>(contact => contact.ContactTypeId.Equals(1));
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Single_Part_Predicate_Using_Equals_Method_On_Right_With_Variable_Should_Match()
        {
            var contactTypeId = 1;
            var spec = new Specification<Contact>(contact => contact.ContactTypeId.Equals(contactTypeId));
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Single_Part_Predicate_Using_Equals_Method_On_Left_Should_Match()
        {
            var spec = new Specification<Contact>(contact => 1.Equals(contact.ContactTypeId));
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Single_Part_Predicate_Using_Equals_Method_On_Left_With_Variable_Should_Match()
        {
            var contactTypeId = 1;
            var spec = new Specification<Contact>(contact => contactTypeId.Equals(contact.ContactTypeId));
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Single_Part_Predicate_Constant_On_Left_Should_Match()
        {
            var spec = new Specification<Contact>(contact => 1 == contact.ContactTypeId);
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Single_Part_Predicate_Variable_On_Right_Should_Match()
        {
            var contactId = 1;

            var spec = new Specification<Contact>(contact => contact.ContactTypeId == contactId);
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Single_Part_Predicate_Variable_On_Left_Should_Match()
        {
            var contactId = 1;

            var spec = new Specification<Contact>(contact => contactId == contact.ContactTypeId);
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Two_Part_Predicate_Constant_On_Right_Should_Match()
        {
            var spec = new Specification<Contact>(contact => (contact.Name == "test" && contact.ContactTypeId == 1));
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Two_Part_Predicate_Constant_On_Left_Should_Match()
        {
            var spec = new Specification<Contact>(contact => (contact.Name == "test" && 1 == contact.ContactTypeId));
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Two_Part_Predicate_Variable_On_Right_Should_Match()
        {
            var contactId = 1;

            var spec = new Specification<Contact>(contact => (contact.Name == "test" && contact.ContactTypeId == contactId));
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Two_Part_Predicate_Variable_On_Left_Should_Match()
        {
            var contactId = 1;

            var spec = new Specification<Contact>(contact => (contact.Name == "test" && contactId == contact.ContactTypeId));
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }

        [Test]
        public void Partition_Column_Used_More_Than_Once_Should_Not_Match()
        {
            var contactId = 1;

            var spec = new Specification<Contact>(contact => (contact.ContactTypeId == 1 || contactId == 2));
            var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

            var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

            isMatch.ShouldBe(true);
            value.ShouldBe(1);
        }
    }
}
