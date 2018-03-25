using AOPDemo.Infrastructure.Castle;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AOPDemo.Infrastructure.AspectCore
{
    public class TestCachingInterceptor : AbstractInterceptor
    {
        [FromContainer]
        private ICacheProvider CacheProvider;

        public TestCachingInterceptor(ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;
        }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var qCachingAttribute = GetCacheAttributeInfo(context.ServiceMethod);
            if (qCachingAttribute != null)
            {
                await ProceedCaching(context, next, qCachingAttribute);
            }
            else
            {
                await next(context);
            }
        }

        private TestCacheAttribute GetCacheAttributeInfo(MethodInfo method)
        {
            return method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(TestCacheAttribute)) as TestCacheAttribute;
        }


        private async Task ProceedCaching(AspectContext context, AspectDelegate next, TestCacheAttribute attribute)
        {
            var cacheKey = GenerateCacheKey(context);

            var cacheValue = CacheProvider.Get(cacheKey);
            if (cacheValue != null)
            {
                context.ReturnValue = cacheValue;
                return;
            }

            await next(context);

            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                CacheProvider.Set(cacheKey, context.ReturnValue, TimeSpan.FromSeconds(attribute.AbsoluteExpiration));
            }
        }


        private string GenerateCacheKey(AspectContext context)
        {
            var typeName = context.ServiceMethod.DeclaringType.Name;
            var methodName = context.ServiceMethod.Name;
            var methodArguments = this.FormatArgumentsToPartOfCacheKey(context.ServiceMethod.GetParameters());

            return this.GenerateCacheKey(typeName, methodName, methodArguments);
        }

        private char _linkChar = '|';
        private string GenerateCacheKey(string typeName,string methodName, IList<string> parameters)
        {
            var builder = new System.Text.StringBuilder();
            builder.Append(typeName);
            builder.Append(_linkChar);

            builder.Append(methodName);
            builder.Append(_linkChar);

            foreach(var parm in parameters)
            {
                builder.Append(parm);
                builder.Append(_linkChar);
            }

            return builder.ToString().TrimEnd(_linkChar);
        }

        private IList<string> FormatArgumentsToPartOfCacheKey(IList<object> methodArguments, int maxCount = 6)
        {
            return methodArguments.Select(this.GetArgumentValue).Take(maxCount).ToList();
        }

        private string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            if (arg is ITestCachable)
                return ((ITestCachable)arg).CacheKey;

            return null;
        }
    }
}
