using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOPDemo.Infrastructure.Castle
{
    public class MemoryCacheProvider : ICacheProvider
    {

        private IMemoryCache Cache;
        public MemoryCacheProvider(IMemoryCache cache)
        {
            Cache = cache;
        }
        public object Get(string cacheKey)
        {
            return Cache.Get(cacheKey);
        }

        public void Set(string cacheKey, object cacheValue, TimeSpan absoluteExpirationRelativeToNow)
        {
            Cache.Set(cacheKey, cacheValue, absoluteExpirationRelativeToNow);
        }
    }
}
