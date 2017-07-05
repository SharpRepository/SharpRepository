using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.TestObjects;
using SharpRepository.InMemoryRepository;
using Shouldly;

namespace SharpRepository.Tests.Spikes
{
    /// <summary>
    /// Can we implement Delete(T) and Delete(TKey) or must we resort to Delete(T) and DeleteByKey(TKey)?
    /// 
    /// Let's assume the most likely case where both T and TKey were of the same type (e.g. Int32, String or Guid). 
    /// There's no way to explicitly constain a generic type Int32, String or Guid, but the IRepositoryBase interface 
    /// requires that T is a class. So, we can't have IRepository<Int32, Int32> or IRepository<Guid, Guid>.  
    /// However, we could declare as IRepository<string, string> still. But, each implementor of IRepositoryBase 
    /// (InMemory, Ef, Xml, etc) constaints T to new() which means T must have a parameterless constructor. 
    /// In the case of string, it does not. So we're covered. Go ahead and declare IRepository<string, string>. But good luck
    /// using it.
    /// 
    /// But what if someone is silly enough to do something like IRepository<Contact, Contact>? The code would compile 
    /// and run just fine assuming the Delete call was never referenced. If referenced, the code won't build due to 
    /// an Ambiguous Invocation compilation check.
    /// </summary>
    [TestFixture]
    public class DeleteByKeyConstaintSpike : TestBase
    {
        protected IRepository<Contact, int> Repository;

        [SetUp]
        public void Setup()
        {
            Repository = new InMemoryRepository<Contact, int>(); 
        }

        [TearDown]
        public void Teardown()
        {
            Repository = null;
        }

        [Test]
        public void Delete_Should_Remove_Item()
        {
            var contact = new Contact { ContactId = 1, Name = "Test User" };
            Repository.Add(contact);

            var result = Repository.Get(contact.ContactId);
            result.ShouldNotBeNull();

            Repository.Delete(contact);
            result = Repository.Get(contact.ContactId);
            result.ShouldBeNull();
        }

        [Test]
        public void Delete_Should_Remove_Item_By_Key()
        {
            var contact = new Contact { Name = "Test User" };
            Repository.Add(contact);

            var result = Repository.Get(contact.ContactId);
            result.ShouldNotBeNull();

            Repository.Delete(contact.ContactId);
            result = Repository.Get(contact.ContactId);
            result.ShouldBeNull();
        }
    }
}