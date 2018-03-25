using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOPDemo.Infrastructure.Castle
{
    public interface ICacheProvider
    {
        object Get(string cacheKey);

        void Set(string cacheKey, object cacheValue, TimeSpan absoluteExpirationRelativeToNow);
    }
}
