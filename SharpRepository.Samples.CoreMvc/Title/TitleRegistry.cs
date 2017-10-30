using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpRepository.CoreMvc
{
    public class TitleRegistry : Registry
    {
        public TitleRegistry()
        {
            For<ITitle>().Use<Title>();
        }
    }
}
