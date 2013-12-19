using System.Linq;
using NUnit.Framework;
using Raven.Client.Embedded;
using SharpRepository.RavenDbRepository;
using Should;

namespace SharpRepository.Tests.Integration.Spikes
{
    [TestFixture]
    public class RavenDbEmbeddedTests
    {
        [Test]
        public void Use_Raven_Db_Embedded_For_Tests()
        {
            var documentStore = new EmbeddableDocumentStore() {RunInMemory = true};
            using (var repos = new RavenDbRepository<RavenTestStringKey>(documentStore))
            {
                repos.Add(new RavenTestStringKey() {Name = "Jeff", Age = 33});
                repos.Add(new RavenTestStringKey() {Name = "Ben", Age = 53}); // :)

                var items = repos.GetAll().ToList();

                items.Count.ShouldEqual(2);

                var item2 = repos.Get("RavenTestStringKeys/2");
                    // is there a way we could allow them to just pass in 2 or the full string, or is that a bad idea
                item2.Name.ShouldEqual("Ben");
            }
        }

        [Test]
        public void Use_Raven_Db_Embedded_For_Tests_With_Int_Key()
        {
            var documentStore = new EmbeddableDocumentStore() { RunInMemory = true };
            using (var repos = new RavenDbRepository<RavenTestIntKey, int>(documentStore))
            {
                repos.Add(new RavenTestIntKey() {Name = "Jeff", Age = 33});
                repos.Add(new RavenTestIntKey() {Name = "Ben", Age = 53}); // :)

                var items = repos.GetAll().ToList();

                items.Count.ShouldEqual(2);

                // this works but won't work if the primary key is [ClassName]Id instead of just Id
                var item1 = repos.Get(1);
                item1.Name.ShouldEqual("Jeff");

                var item2 = repos.Get(2);
                item2.Name.ShouldEqual("Ben");
            }
        }

        [Test]
        public void Use_Raven_Db_Embedded_For_Tests_With_Custom_Int_Key()
        {
            var documentStore = new EmbeddableDocumentStore() { RunInMemory = true };
            using (var repos = new RavenDbRepository<RavenTestCustomIntKey, int>(documentStore))
            {
                repos.Add(new RavenTestCustomIntKey() { Name = "Jeff", Age = 33 });
                repos.Add(new RavenTestCustomIntKey() { Name = "Ben", Age = 53 }); // :)

                var items = repos.GetAll().ToList();

                items.Count.ShouldEqual(2);

                // this works but won't work if the primary key is [ClassName]Id instead of just Id
                var item1 = repos.Get(1);
                item1.Name.ShouldEqual("Jeff");

                var item2 = repos.Get(2);
                item2.Name.ShouldEqual("Ben");
            }
        }

//        [Test]
//        public void Use_Raven_Advanced_Configuration_Aspect()
//        {
//            var documentStore = new EmbeddableDocumentStore() { RunInMemory = true };
//            using (var repos = new RavenDbRepository<RavenTestCustomIntKey, int>(documentStore))
//            {
//                repos.Aspects.Add(new AdvancedConfiguration<RavenTestCustomIntKey, int>(true));
//                repos.Aspects.Add(new NLogRepositoryLogger<RavenTestCustomIntKey, int>());
//                repos.Aspects.OnInitialize(repos); // TODO: this is a hack since right now it's not calling OnIntitialize properly
//
//                repos.Add(new RavenTestCustomIntKey() { Name = "Jeff", Age = 33 });
//                repos.Add(new RavenTestCustomIntKey() { Name = "Ben", Age = 53 }); // :)
//
//                var items = repos.GetAll().ToList();
//
//                items.Count.ShouldEqual(2);
//
//                // this works but won't work if the primary key is [ClassName]Id instead of just Id
//                var item1 = repos.Get(1);
//                item1.Name.ShouldEqual("Jeff");
//
//                var item2 = repos.Get(2);
//                item2.Name.ShouldEqual("Ben");
//            }
//        }

    }

    public class RavenTestStringKey
    {
        // RavenDb by default does keys as strings, the first one will be "RavenTestObjects/1"
        //  I think there is a way to do as ints and increment them but still trying to figure that out :)  that's next
        public string Id { get; set; } 
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class RavenTestIntKey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class RavenTestCustomIntKey
    {
        public int RavenTestCustomIntKeyId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
