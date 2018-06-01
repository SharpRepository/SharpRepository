using SharpRepository.Repository.FetchStrategies;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpRepository.MongoDbRepository
{
    public class MongoDbFetchStrategy<T> : GenericFetchStrategy<T>
    {
        public MongoDbFetchStrategy()
        {
            AllowDiskUse = false;
        }

        public bool AllowDiskUse { get; set; }
    }
}
