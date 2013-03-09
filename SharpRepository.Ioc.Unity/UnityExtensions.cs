using Microsoft.Practices.Unity;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;

namespace SharpRepository.Ioc.Unity
{
    public static class UnityExtensions
    {
        public static void RegisterSharpRepository(this UnityContainer container, string repositoryName = null)
        {
            // using InjectionFactory I can get access to the container but I don't seem to ahve access to a context
            //  in the other Ioc's there is a context where I can get access to the type being resolved and get the generic arguments which is what i need
//            container.RegisterType(typeof (IRepository<>), new InjectionFactory(c =>
//                                                                                    {
//                                                                                        
//                                                                                    });
        }

        public static void RegisterSharpRepository(this UnityContainer container, ISharpRepositoryConfiguration configuration)
        {

        }
    }
}
