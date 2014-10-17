using System;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace SharpRepository.Caching.Redis
{
    public static class StackExchangeRedisExtensions
    {
        public static T Get<T>(this IDatabase cache, string key)
        {
            return Deserialize<T>(cache.StringGet(key));
        }

        public static object Get(this IDatabase cache, string key)
        {
            return Deserialize<object>(cache.StringGet(key));
        }

        public static void Set(this IDatabase cache, string key, object value, TimeSpan? expiry = null)
        {
            cache.StringSet(key, Serialize(value), expiry);
        }

        static string Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(o);
        }

        static T Deserialize<T>(string value)
        {
            if (value == null)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
