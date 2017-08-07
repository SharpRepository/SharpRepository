using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using SharpRepository.Benchmarks.Configuration.Models;
using SharpRepository.Benchmarks.Configuration.Repositories;
using SharpRepository.InMemoryRepository;
using SharpRepository.Repository;
using SharpRepository.Repository.Configuration;
using StructureMap;
using StructureMap.Graph;
using SharpRepository.Ioc.StructureMap;

namespace SharpRepository.Benchmarks.Configuration
{
    class BenchmarkItem
    {
        public Action Test { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }

    class Program
    {
        private const int Max = 250000;
        static void Main(string[] args)
        {
            var benchmarks = new Benchmarks();
            var tests = new List<BenchmarkItem>()
                            {
                                new BenchmarkItem()
                                    {
                                        Title = "Direct Creation: [new InMemoryRepository()]",
                                        Test = Benchmarks.DirectCreation,
                                        Order = 1
                                    },
                                new BenchmarkItem()
                                    {
                                        Title = "Custom repo hardcoded: [UserRepository : InMemoryRepository<User,int>]",
                                        Test = Benchmarks.CustomRepositoryHardCoded,
                                        Order = 2
                                    },
                                new BenchmarkItem()
                                    {
                                        Title = "From config file: [RepositoryFactory.GetInstance<User, int>()]",
                                        Test = Benchmarks.CreateFromConfigFile,
                                        Order = 3
                                    },
                                new BenchmarkItem()
                                    {
                                        Title = "From config obj: [RepositoryFactory.GetInstance<User, int>(config)]",
                                        Test = Benchmarks.CreateFromConfigObject,
                                        Order = 4
                                    },
                                new BenchmarkItem()
                                    {
                                        Title = "Custom repo config: [UserRepository : ConfigurationBasedRepository<User, int>]",
                                        Test = Benchmarks.CustomRepositoryFromConfig,
                                        Order = 5
                                    },
                                new BenchmarkItem()
                                    {
                                        Title = "StructureMap w/ config: [container.GetInstance<IRepository<User, int>>()]",
                                        Test = benchmarks.DirectFromStructureMap,
                                        Order = 6
                                    }
                            };

            // run thru each of them once because otherwise the first loop is slower for some reason
            foreach (var item in tests)
            {
                item.Test();
            }

            Console.WriteLine("Running each test {0:#,0} times", Max);
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

            var sw = new Stopwatch();

            foreach (var benchmarkItem in tests.AsQueryable().OrderBy(x => x.Order))
            {
                Console.WriteLine(benchmarkItem.Title);
                sw.Reset();
                sw.Start();

                for (var i = 0; i < Max; i++)
                {
                    benchmarkItem.Test();
                }
                sw.Stop();
                Console.WriteLine("   {0} ms total -- {1} avg ms per\n", sw.Elapsed.TotalMilliseconds, sw.Elapsed.TotalMilliseconds / Convert.ToDouble(Max));
            }
            
            Console.WriteLine("\nDone: press enter to quit");
            Console.Read();
        }
    }

    public class Benchmarks
    {
        public Container container { get; set; }

        public Benchmarks()
        {
            container = new Container(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

                x.ForRepositoriesUseSharpRepository();
            });
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DirectCreation()
        {
            new InMemoryRepository<User, int>();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void CreateFromConfigFile()
        {
            RepositoryFactory.GetInstance<User, int>();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void CustomRepositoryFromConfig()
        {
            
            var config = new SharpRepositoryConfiguration();
            var repoConf = new RepositoryConfiguration("inMemory");
            repoConf.Factory = typeof(InMemoryConfigRepositoryFactory);
            config.AddRepository(repoConf);
            var option = new SharpRepositoryOptions(config);
            new UserFromConfigRepository(option);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void CustomRepositoryHardCoded()
        {
            new UserRepository();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DirectFromStructureMap()
        {
            container.GetInstance<IRepository<User, int>>();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void CreateFromConfigObject()
        {
            var config = new SharpRepositoryConfiguration();
            config.AddRepository(new InMemoryRepositoryConfiguration("default"));
            RepositoryFactory.GetInstance<User, int>(config);
        }
    }
}
