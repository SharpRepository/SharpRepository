using System;

namespace SharpRepository.MongoDbRepository
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoDbCollectionNameAttribute: Attribute
    {
        public string CollectionName { get; }

        public MongoDbCollectionNameAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
