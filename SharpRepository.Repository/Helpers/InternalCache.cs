using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace SharpRepository.Repository.Helpers
{
    internal static class InternalCache
    {
        internal static readonly IDictionary<Tuple<Type, Type>, PropertyInfo> PrimaryKeyMapping = new ConcurrentDictionary<Tuple<Type, Type>, PropertyInfo>();
    }
}
