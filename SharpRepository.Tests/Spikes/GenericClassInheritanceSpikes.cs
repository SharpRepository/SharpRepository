
using NUnit.Framework;
using SharpRepository.Repository;
using SharpRepository.Tests.TestObjects;

namespace SharpRepository.Tests.Spikes
{
    [TestFixture]
    public class GenericClassInheritanceSpikes
    {
        [Test]
        public void SomeTest()
        {
            IThing<Contact> thing = new Thing<Contact>();
            IThing<Contact, int> thing2 = new Thing<Contact>();
            IThing<Contact, string> thing3 = new Thing<Contact, string>();
        }
    }

    public interface IThing<T, T2> where T : class
    {
        T DoSomething(T2 t2);
    }

    public interface IThing<T> : IThing<T, int> where T : class
    {
    }

    public class Thing<T> : IThing<T> where T : class
    {
        public T DoSomething(int t2)
        {
            return null;
        }
    }

    public class Thing<T, T2> : IThing<T, T2> where T : class
    {
        public T DoSomething(T2 t2)
        {
            return null;
        }
    }
}
