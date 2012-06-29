using System.Collections.Generic;
using NUnit.Framework;
using SharpRepository.Tests.Integration.Data;

namespace SharpRepository.Tests.Integration.TestAttributes
{
    public class ExecuteForRepositoriesAttribute : TestCaseSourceAttribute
    {
        private static IEnumerable<TestCaseData> ForRepositoriesTestCaseData
        {
            get
            {
                return RepositoryTestCaseDataFactory.Build(_includeTypes);
            }
        }

        private static RepositoryTypes[] _includeTypes;
                
        public ExecuteForRepositoriesAttribute(params RepositoryTypes[] repositoryTypes) : this()
        {
            _includeTypes = repositoryTypes;
        }

        public ExecuteForRepositoriesAttribute() : base(typeof(ExecuteForRepositoriesAttribute), "ForRepositoriesTestCaseData")
        {
        }
    }

    #region Crap NUnitAddin for TestCaseProvider
    // http://improve.dk/archive/2011/11/28/automated-testing-of-orcamdf-against-multiple-sql-server-versions.aspx
    // http://fzzd.blogspot.com/2011/12/blackbox-testing-with-nunit-using.html

    //[NUnitAddin(Name = "RepositoryTestCaseProvider Addin", Type = ExtensionType.Core)]
    //public class RepositoryTestCaseProviderAddin : IAddin
    //{
    //    public bool Install(IExtensionHost host)
    //    {
    //        host.GetExtensionPoint("TestCaseProviders").Install(new RepositoryTestCaseProvider());
    //        return true;
    //    }
    //}

    //public class RepositoryTestCaseProvider : ITestCaseProvider
    //{
    //    public System.Collections.IEnumerable GetTestCasesFor(MethodInfo method)
    //    {
    //        var attribute = method.GetCustomAttributes(typeof(ExecuteForRepositories), true).FirstOrDefault() as ExecuteForRepositories;

    //        //if (attribute == null || attribute.RepositoryTypes == null || attribute.RepositoryTypes.Length == 0 || !attribute.RepositoryTypes.Contains(RepositoryTypes.InMemory))
    //        //{
    //        //    yield return new TestCaseData(new InMemoryRepository<Contact, int>()).SetName("InMemoryRepository Test");    
    //        //}
           
    //        //if (attribute == null || attribute.RepositoryTypes == null || attribute.RepositoryTypes.Length == 0 || !attribute.RepositoryTypes.Contains(RepositoryTypes.Xml))
    //        //{
    //            var xmlDataDirectoryPath = GetXmlDataDirectoryPath();
    //            yield return new TestCaseData(new XmlRepository<Contact, int>(xmlDataDirectoryPath)).SetName("XmlRepository Test");
    //        //}
    //    }

    //    public bool HasTestCasesFor(MethodInfo method)
    //    {
    //        return method.GetCustomAttributes(typeof (ExecuteForRepositories), false).Any();
    //    }

    //    private static string GetXmlDataDirectoryPath()
    //    {
    //        var rd = new RelativeDirectory();
    //        rd.UpTo("SharpRepository.Tests");

    //        string xmlDataDirectoryPath = Path.Combine(rd.Path, @"Data\Xml");

    //        // start from scratch each time
    //        if (File.Exists(xmlDataDirectoryPath + @"\Contact.xml"))
    //        {
    //            File.Delete(xmlDataDirectoryPath + @"\Contact.xml");
    //        }

    //        return xmlDataDirectoryPath;
    //    }
    //}
    
    //public class ExecuteForAllRepositories : TestCaseSourceAttribute
    //{
    //    private static IEnumerable<TestCaseData> repositories
    //    {
    //        get
    //        {
    //            yield return new TestCaseData(new InMemoryRepository<Contact, int>()).SetName("InMemoryRepository Test");

    //            var xmlDataDirectoryPath = GetXmlDataDirectoryPath();
    //            yield return new TestCaseData(new XmlRepository<Contact, int>(xmlDataDirectoryPath)).SetName("XmlRepository Test");
    //        }
    //    }

    //    public ExecuteForAllRepositories() : base(typeof(ExecuteForAllRepositories), "repositories")
    //    {
    //    }
    //    private static string GetXmlDataDirectoryPath()
    //    {
    //        var rd = new RelativeDirectory();
    //        rd.UpTo("SharpRepository.Tests");

    //        string xmlDataDirectoryPath = Path.Combine(rd.Path, @"Data\Xml");

    //        // start from scratch each time
    //        if (File.Exists(xmlDataDirectoryPath + @"\Contact.xml"))
    //        {
    //            File.Delete(xmlDataDirectoryPath + @"\Contact.xml");
    //        }

    //        return xmlDataDirectoryPath;
    //    }
    //}

#endregion
}