using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.Integration.TestAttributes;
using SharpRepository.Tests.Integration.TestObjects;
using Should;

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

        [ExecuteForAllRepositoriesExcept(RepositoryType.RavenDb, Reason = "Depends on driver to generate a value")]
        public void Add_Should_Save_But_Not_Assign_New_Id_When_GenerateKeyOnAdd_Is_False(IRepository<Contact, string> repository)
        {
            var contact = new Contact { ContactId = string.Empty, Name = "Test User" };
            repository.GenerateKeyOnAdd = false;
            repository.Add(contact);
            contact.ContactId.ShouldBeEmpty();
        }

        [ExecuteForAllRepositories]
        public void Add_Should_Result_In_Proper_Total_Items(IRepository<Contact, string> repository)
        {
            repository.Add(new Contact { Name = "Test User" });
            
            var result = repository.GetAll();
            result.Count().ShouldEqual(1);
        }

        [ExecuteForAllRepositories]
        public void Add_InBatchMode_Should_Delay_The_Action(IRepository<Contact, string> repository)
        {
            using (var batch = repository.BeginBatch())
            {
                batch.Add(new Contact { Name = "Test User 1" });

                repository.GetAll().Count().ShouldEqual(0); // shouldn't have really been added yet

                batch.Add(new Contact { Name = "Test User 2" });

                repository.GetAll().Count().ShouldEqual(0); // shouldn't have really been added yet

                batch.Commit();
            }

            repository.GetAll().Count().ShouldEqual(2);
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
            added.Count().ShouldEqual(3);
        }

        [ExecuteForRepositories(RepositoryType.Ef5)]
        public void Using_TransactionScope_Without_Complete_Should_Not_Add(IRepository<Contact, string> repository)
        {
            repository.Get("test"); // used to create the SqlCe database before being inside the transaction scope since that throws an error

            using (var trans = new TransactionScope())
            {
                repository.Add(new Contact {Name = "Contact 1"});
            }

            repository.GetAll().Count().ShouldEqual(0);
        }

        [ExecuteForRepositories(RepositoryType.Ef5)]
        public void Using_TransactionScope_With_Complete_Should_Add(IRepository<Contact, string> repository)
        {
            repository.Get("test"); // used to create the SqlCe database before being inside the transaction scope since that throws an error

            using (var trans = new TransactionScope())
            {
                repository.Add(new Contact { Name = "Contact 1" });

                trans.Complete();
            }

            repository.GetAll().Count().ShouldEqual(1);
        }
    }
}