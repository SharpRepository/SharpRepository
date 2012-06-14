using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Add_Should_Save_And_Assigned_New_Id(IRepository<Contact, int> repository)
        {
            var contact = new Contact { Name = "Test User" };
            repository.Add(contact);
            contact.ContactId.ShouldEqual(1);
        }
        
        [ExecuteForAllRepositories]
        public void Add_Should_Result_In_Proper_Total_Items(IRepository<Contact, int> repository)
        {
            repository.Add(new Contact { Name = "Test User" });
            
            var result = repository.GetAll();
            result.Count().ShouldEqual(1);
        }

        [ExecuteForRepositories(RepositoryTypes.Xml, RepositoryTypes.InMemory, RepositoryTypes.Ef)]
        public void Add_InBatchMode_Should_Delay_The_Action(IRepository<Contact, int> repository)
        {
            using (var batch = repository.BeginBatch())
            {
                batch.Add(new Contact { Name = "Test User 1" });

                var result = repository.GetAll();
                result.Count().ShouldEqual(0); // shouldn't have really been added yet

                batch.Add(new Contact { Name = "Test User 2" });

                result = repository.GetAll();
                result.Count().ShouldEqual(0); // shouldn't have really been added yet

                batch.Commit();
            }

            repository.GetAll().Count().ShouldEqual(2);
        }

        [ExecuteForAllRepositories]
        public void Add_Should_Save_And_Assigned_New_Ids_To_Multiple(IRepository<Contact, int> repository)
        {
            IList<Contact> contacts = new List<Contact>
                                            {
                                                new Contact { Name = "Contact 1"},
                                                new Contact { Name = "Contact 2"},
                                                new Contact { Name = "Contact 3"},
                                        };

            repository.Add(contacts);
            contacts.First().ContactId.ShouldEqual(1);
            contacts.Last().ContactId.ShouldEqual(3);

            var added = repository.GetAll();
            added.Count().ShouldEqual(3);
        }
    }
}