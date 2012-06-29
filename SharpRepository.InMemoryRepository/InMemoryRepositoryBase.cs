using System;
using System.Collections.Generic;
using System.Linq;
using SharpRepository.Repository.Caching;
using SharpRepository.Repository.FetchStrategies;

namespace SharpRepository.Repository
{
    public abstract class InMemoryRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey> where T : class, new()
    {
        private readonly IList<T> _items = new List<T>();

        internal InMemoryRepositoryBase(ICachingStrategy<T, TKey> cachingStrategy = null) : base(cachingStrategy) 
        {   
        }

        protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
        {
            return CloneList(_items).AsQueryable();
        }
        
        private static IEnumerable<T> CloneList(IList<T> list)
        {
            // when you Google deep copy of generic list every answer uses either the IClonable interface on the T or having the T be Serializable
            //  since we can't really put those constraints on T I'm going to do it via reflection

            var type = typeof (T);
            var properties = type.GetProperties();

            var clonedList = new List<T>(list.Count);

            foreach (T item in list)
            {
                var newItem = new T();
                foreach (var propInfo in properties)
                {
                    propInfo.SetValue(newItem, propInfo.GetValue(item, null), null);
                }

                clonedList.Add(newItem);
            }

            return clonedList;
        }

        protected override void AddItem(T entity)
        {
            TKey id;

            if (GetPrimaryKey(entity, out id) && object.Equals(id, default(TKey)))
            {
                id = GeneratePrimaryKey();
                SetPrimaryKey(entity, id);
            }

            _items.Add(entity);
        }

        protected override void DeleteItem(T entity)
        {
            TKey pkValue;
            GetPrimaryKey(entity, out pkValue);

            var index = _items.ToList().FindIndex(x => MatchOnPrimaryKey(x, pkValue));
            if (index >= 0)
            {
                _items.RemoveAt(index);
            }
        }

        protected override void UpdateItem(T entity)
        {
            TKey pkValue;
            GetPrimaryKey(entity, out pkValue);

            var index = _items.ToList().FindIndex(x => MatchOnPrimaryKey(x, pkValue));            
            if (index >= 0)
            {
                _items[index] = entity;
            }           
        }

        // need to match on primary key instead of using Equals() since the objects are not the same and are a cloned copy
        private bool MatchOnPrimaryKey(T item, TKey keyValue)
        {
            TKey value;
            return GetPrimaryKey(item, out value) && keyValue.Equals(value);
        }

        //protected override IEnumerable<T> GetAllQuery()
        //{
        //    var results = base.GetAllQuery();
        //    return results;
        //}

        protected override void SaveChanges()
        {
            
        }

        public override void Dispose()
        {
            
        }

        private TKey GeneratePrimaryKey()
        {
            if (typeof(TKey) == typeof(Guid))
            {
                return (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));
            }

            if (typeof(TKey) == typeof(string))
            {
                return (TKey)Convert.ChangeType(Guid.NewGuid().ToString(), typeof(TKey));
            }

            if (typeof(TKey) == typeof(Int32))
            {
                TKey pkValue;

                var last = _items.LastOrDefault() ?? new T();
                GetPrimaryKey(last, out pkValue);

                var nextInt = Convert.ToInt32(pkValue) + 1;
                return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
            }

            throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and String.");
        }

        public override string ToString()
        {
            return "SharpRepository.InMemoryRepository";
        }
    }
}
