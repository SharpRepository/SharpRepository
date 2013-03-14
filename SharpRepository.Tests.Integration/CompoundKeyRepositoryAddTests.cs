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
    public class CompoundKeyRepositoryAddTests : TestBase
    {
        [ExecuteForAllCompoundKeyRepositories]
        public void Add_Should_Result_In_Proper_Total_Items(ICompoundKeyRepository<User, string, int> repository)
        {
            repository.Add(new User { Username = "Test User", Age = 11, FullName = "Test User - 11"});
            
            var result = repository.GetAll();
            result.Count().ShouldEqual(1);
        }

        [ExecuteForAllCompoundKeyRepositories]
        public void Add_InBatchMode_Should_Delay_The_Action(ICompoundKeyRepository<User, string, int> repository)
        {
            using (var batch = repository.BeginBatch())
            {
                batch.Add(new User { Username = "Test User", Age = 11, FullName = "Test User - 11" });

                repository.GetAll().Count().ShouldEqual(0); // shouldn't have really been added yet

                batch.Add(new User { Username = "Test User", Age = 21, FullName = "Test User - 21" });

                repository.GetAll().Count().ShouldEqual(0); // shouldn't have really been added yet

                batch.Commit();
            }

            repository.GetAll().Count().ShouldEqual(2);
        }

        [ExecuteForCompoundKeyRepositories(RepositoryTypes.Ef5)]
        public void Using_TransactionScope_Without_Complete_Should_Not_Add(ICompoundKeyRepository<User, string, int> repository)
        {
            repository.Get("test", 1); // used to create the SqlCe database before being inside the transaction scope since that throws an error

            using (var trans = new TransactionScope())
            {
                repository.Add(new User { Username = "Test User", Age = 11, FullName = "Test User - 11" });
            }

            repository.GetAll().Count().ShouldEqual(0);
        }

        [ExecuteForCompoundKeyRepositories(RepositoryTypes.Ef5)]
        public void Using_TransactionScope_With_Complete_Should_Add(ICompoundKeyRepository<User, string, int> repository)
        {
            repository.Get("test", 1); // used to create the SqlCe database before being inside the transaction scope since that throws an error

            using (var trans = new TransactionScope())
            {
                repository.Add(new User { Username = "Test User", Age = 11, FullName = "Test User - 11" });

                trans.Complete();
            }

            repository.GetAll().Count().ShouldEqual(1);
        }
    }
}