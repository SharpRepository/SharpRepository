using System;
using System.Collections.Generic;
using Omu.ValueInjecter;

namespace SharpRepository.Ef5Repository
{
    public class DynamicProxyCloneInjection : LoopValueInjection
    {
        // here we store the DyamicProxy classes that we've seen already going down the tree
        //  that way we can check to see if we've already visited this object and if so it's a circular reference so we can ignore the 2nd time we see it
        private readonly List<object> _foundDocs;

        public DynamicProxyCloneInjection()
        {
            _foundDocs = new List<object>();
        }

        // so we can pass in what was already found at a previous level to avoid circular references
        public DynamicProxyCloneInjection(List<object> foundDocs)
        {
            _foundDocs = foundDocs;
        }

        protected override void Inject(object source, object target)
        {
            _foundDocs.Add(source);
            base.Inject(source, target);
        }

        protected override object SetValue(object v)
        {
            // have we seen this object already?  if so then it's a circular reference and let's not try to load it
            //  just set it to null so we don't loop back
            if (v == null || _foundDocs.Contains(v))
                return null;

            var type = v.GetType();
            var typeName = type.FullName;

            if (typeName.StartsWith("System.Data.Entity.DynamicProxies"))
            {
                var baseType = type.BaseType;
                if (baseType != null)
                {
                    // let's clean up this property because it's to a DynamciProxy class as well
                    return Activator.CreateInstance(baseType).InjectFrom(new DynamicProxyCloneInjection(_foundDocs), v);
                }
            }

            return base.SetValue(v);
        }
    }
}