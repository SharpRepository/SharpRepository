using SharpRepository.Ioc.StructureMap;
using StructureMap;
using StructureMap.Graph;

namespace SharpRepository.Benchmarks.Configuration
{
    public static class Bootstrapper
    {
        public static Container Run()
        {
            var container = new Container(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                x.ForRepositoriesUseSharpRepository();
            });

            return container;
        }
    }
}
