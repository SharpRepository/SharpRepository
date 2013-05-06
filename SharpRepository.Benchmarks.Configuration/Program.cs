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
        static Program()
        {
            // This loads the StructureMap registry
            Bootstrapper.Run();
        }

        private const int Max = 250000;
        static void Main(string[] args)
        {
            var tests = new List<BenchmarkItem>()
                            {
                                new BenchmarkItem()
                                    {
                                        Title = "Direct Creation: [new InMemoryRepository()]",
                                        Test = DirectCreation,
                                        Order = 1
                                    },
                                new BenchmarkItem()
                                    {
                                        Title = "Custom repo hardcoded: [UserRepository : InMemoryRepository<User,int>]",
                                        Test = CustomRepositoryHardCoded,
                                        Order = 2
                                    },
                                new BenchmarkItem()
                                    {
                                        Title = "From config file: [RepositoryFactory.GetInstance<User, int>()]",
                                        Test = CreateFromConfigFile,
                                        Order = 3
                                    },
                                new BenchmarkItem()
                                    {
                                        Title = "From config obj: [RepositoryFactory.GetInstance<User, int>(config)]",
                                        Test = CreateFromConfigObject,
                                        Order = 4
                                    },
                                new BenchmarkItem()
                                    {
                                        Title = "Custom repo config: [UserRepository : ConfigurationBasedRepository<User, int>]",
                                        Test = CustomRepositoryFromConfig,
                                        Order = 5
                                    },
                                new BenchmarkItem()
                                    {
                                        Title = "StructureMap w/ config: [ObjectFactory.GetInstance<IRepository<User, int>>()]",
                                        Test = DirectFromStructureMap,
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

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void DirectCreation()
        {
            new InMemoryRepository<User, int>();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void CreateFromConfigFile()
        {
            RepositoryFactory.GetInstance<User, int>();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void CustomRepositoryFromConfig()
        {
            new UserFromConfigRepository();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void CustomRepositoryHardCoded()
        {
            new UserRepository();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void DirectFromStructureMap()
        {
            ObjectFactory.GetInstance<IRepository<User, int>>();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static void CreateFromConfigObject()
        {
            var config = new SharpRepositoryConfiguration();
            config.AddRepository(new InMemoryRepositoryConfiguration("default"));
             RepositoryFactory.GetInstance<User, int>(config);
        }
    }
}
