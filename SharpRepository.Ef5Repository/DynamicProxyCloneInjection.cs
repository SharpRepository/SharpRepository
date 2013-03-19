using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;

namespace SharpRepository.Ef5Repository
{
    // TODO: allow setting a limit on the levels deep that we go, since this most likely is lazy loading, then it will be hitting the database on each property we follow, so we shouldn't follow it 10 levels deep
    //  maybe we have a default max and allow it to be changed via config files
    public class DynamicProxyCloneInjection : LoopValueInjection
    {
        private const string DynamicProxyNamespace = "System.Data.Entity.DynamicProxies";

        // here we store the DyamicProxy classes that we've seen already going down the tree
        //  that way we can check to see if we've already visited this object and if so it's a circular reference so we can ignore the 2nd time we see it
        private readonly HashSet<object> _foundProxies;
        private int _currentDepth;
        private readonly int _maxDepth;

        public DynamicProxyCloneInjection(int maxDepth = 10)
        {
            _maxDepth = maxDepth;
            _foundProxies = new HashSet<object>();
            _currentDepth = 0;
        }

        // so we can pass in what was already found at a previous level to avoid circular references
        public DynamicProxyCloneInjection(int maxDepth, HashSet<object> foundDocs, int currentDepth)
        {
            _maxDepth = maxDepth;
            _foundProxies = foundDocs;
            _currentDepth = currentDepth;
        }

        protected override void Inject(object source, object target)
        {
            _foundProxies.Add(source);
            _currentDepth++;

            if (_currentDepth > _maxDepth)
                throw new Exception("Maximum depth reached, throw error so it doesn't get cached in QueryManager.");

            base.Inject(source, target);
        }

        protected override object SetValue(object v)
        {
            // have we seen this object already?  if so then it's a circular reference and let's not try to load it
            //  just set it to null so we don't loop back
            if (v == null || _foundProxies.Contains(v))
                return null;

            var type = v.GetType();
            if (type.Namespace == DynamicProxyNamespace)
            {
                var baseType = type.BaseType;
                if (baseType != null)
                {
                    // let's clean up this property because it's to a DynamicProxy class as well
                    return Activator.CreateInstance(baseType).InjectFrom(new DynamicProxyCloneInjection(_maxDepth, _foundProxies, _currentDepth), v);
                }
            }

            // let's check for a collection of DynamicProxies, if so we need to clean it up
            if (type.Name == "HashSet`1")
            {
                var genericType = type.GetGenericArguments()[0];
                var cleanHashSet = Activator.CreateInstance(type);
                var addMethod = type.GetMethod("Add");

                foreach (var item in (IEnumerable)v)
                {
                    var tmp = Activator.CreateInstance(genericType).InjectFrom(new DynamicProxyCloneInjection(_maxDepth, _foundProxies, _currentDepth), item);
                    addMethod.Invoke(cleanHashSet, new object[] {tmp});
                    //cleanHashSet.Add(tmp);
                }

                return cleanHashSet;
            }

            return base.SetValue(v);
        }
    }
}