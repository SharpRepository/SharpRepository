using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;
using SharpRepository.Tests.Integration.TestObjects;
using SharpRepository.NHibernateRepository;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class NHibernateSpikes
    {
        [Test]
        public void NHibernateShouldGetASession()
        {
            // reference: http://dotnetslackers.com/articles/ado_net/Your-very-first-NHibernate-application-Part-1.aspx#implementing-and-mapping-the-first-object-of-the-domain-model
            var cfg = Fluently.Configure().Database(SQLiteConfiguration.Standard.InMemory)
                               .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateContactMapping>());
            
                var sessionFactory = cfg.BuildSessionFactory();

            //yield return new TestCaseData(new NHibernateRepository<Contact, string>(session)).SetName("NHibernateRepository Test");
        }
    }
}
