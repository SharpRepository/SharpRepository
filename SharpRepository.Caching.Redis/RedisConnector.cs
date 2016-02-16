using StackExchange.Redis;

namespace SharpRepository.Caching.Redis
{
    public static class RedisConnector
    {
        public static ConnectionMultiplexer Connection { get; set; }
    }
}
