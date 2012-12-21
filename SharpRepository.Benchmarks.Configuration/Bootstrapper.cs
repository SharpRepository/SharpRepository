using SharpRepository.Ioc.StructureMap;
using StructureMap;

namespace SharpRepository.Benchmarks.Configuration
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                x.ForRepositoriesUseSharpRepository();
            });
        }
    }
}
