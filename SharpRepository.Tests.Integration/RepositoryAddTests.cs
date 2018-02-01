using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using SharpRepository.EfRepository;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Shouldly;

namespace SharpRepository.Tests.Integration
{
    [TestFixture]
    public class RepositoryAddTests : TestBase
    {
        [ExecuteForAllRepositories]
        public void Add_Should_Save_And_Assigned_New_Id(IRepository<Contact, string> repository)
        {
            var contact = new Contact { Name = "Test User" };
            repository.Add(contact);
            contact.ContactId.ShouldNotBeEmpty();
        }

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, RepositoryType.MongoDb, Reason = "Depends on driver to generate a value")]
        public void Add_Should_Save_But_Not_Assign_New_String_Id_When_GenerateKeyOnAdd_Is_False(IRepository<Contact, string> repository)
        {
            var contact = new Contact { ContactId = string.Empty, Name = "Test User" };
            repository.GenerateKeyOnAdd = false;
            repository.Add(contact);
            contact.ContactId.ShouldBeEmpty();
        }

        [TestCase]
        public void Add_Should_Save_And_Assign_1_To_Ef_Int_Id_When_GenerateKeyOnAdd_Is_False()
        {
            var dbPath = EfDataDirectoryFactory.Build();
            var repository = new EfRepository<ContactInt, int>(new TestObjectContext("Data Source=" + dbPath));
            var contact = new ContactInt { Name = "Test User" };
            repository.GenerateKeyOnAdd = false;
            repository.Add(contact);
            contact.ContactIntId.ShouldBe(1);
        }

        [TestCase]
        public void Add_Should_Save_And_Assign_1_To_InMemory_Int_Id()
        {
            var dbPath = EfDataDirectoryFactory.Build();
            var repository = new InMemoryRepository<ContactInt, int>();
            var contact = new ContactInt { Name = "Test User" };
            
            repository.Add(contact);
            contact.ContactIntId.ShouldBe(1);
        }
        
        [TestCase]
        public void Add_Should_Save_But_Not_Assign_New_InMemory_Int_Id_When_GenerateKeyOnAdd_Is_False()
        {
            var dbPath = EfDataDirectoryFactory.Build();
            var repository = new InMemoryRepository<ContactInt, int>();
            var contact = new ContactInt { Name = "Test User" };
            repository.GenerateKeyOnAdd = false;
            
            repository.Add(contact);
            contact.ContactIntId.ShouldBe(0);
        }
        
        [ExecuteForAllRepositories]
        public void Add_Should_Result_In_Proper_Total_Items(IRepository<Contact, string> repository)
        {
            repository.Add(new Contact { Name = "Test User" });
            
            var result = repository.GetAll();
            result.Count().ShouldBe(1);
        }

        [ExecuteForAllRepositories("Add_InBatchMode_Should_Delay_The_Action")]
        public void Add_InBatchMode_Should_Delay_The_Action(IRepository<Contact, string> repository)
        {
            using (var batch = repository.BeginBatch())
            {
                batch.Add(new Contact { Name = "Test User 1" });

                repository.GetAll().Count().ShouldBe(0); // shouldn't have really been added yet

                batch.Add(new Contact { Name = "Test User 2" });

                repository.GetAll().Count().ShouldBe(0); // shouldn't have really been added yet

                batch.Commit();
            }

            repository.GetAll().Count().ShouldBe(2);
        }

        [ExecuteForAllRepositories]
        public void Add_Should_Save_And_Assigned_New_Ids_To_Multiple(IRepository<Contact, string> repository)
        {
            IList<Contact> contacts = new List<Contact>
                                            {
                                                new Contact { Name = "Contact 1"},
                                                new Contact { Name = "Contact 2"},
                                                new Contact { Name = "Contact 3"},
                                        };

            repository.Add(contacts);
            contacts.First().ContactId.ShouldNotBeEmpty();
            contacts.Last().ContactId.ShouldNotBeEmpty();
            contacts.First().ShouldNotBeSameAs(contacts.Last().ContactId);
            
            var added = repository.GetAll();
            added.Count().ShouldBe(3);
        }

        [ExecuteForRepositories(RepositoryType.Ef)]
        public void Using_TransactionScope_Without_Complete_Should_Not_Add(IRepository<Contact, string> repository)
        {
            repository.Get("test"); // used to create the SqlCe database before being inside the transaction scope since that throws an error

            using (var trans = new TransactionScope())
            {
                repository.Add(new Contact {Name = "Contact 1"});
            }

            repository.GetAll().Count().ShouldBe(0);
        }

        [ExecuteForRepositories(RepositoryType.Ef)]
        public void Using_TransactionScope_With_Complete_Should_Add(IRepository<Contact, string> repository)
        {
            repository.Get("test"); // used to create the SqlCe database before being inside the transaction scope since that throws an error

            using (var trans = new TransactionScope())
            {
                repository.Add(new Contact { Name = "Contact 1" });
                trans.Complete();
            }

            repository.GetAll().Count().ShouldBe(1);
        }
    }
}