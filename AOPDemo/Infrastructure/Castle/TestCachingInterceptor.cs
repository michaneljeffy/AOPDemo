using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AOPDemo.Infrastructure.Castle
{
    public class TestCachingInterceptor : IInterceptor
    {

        private ICacheProvider CacheProvider;

        public TestCachingInterceptor(ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;
        }
        public void Intercept(IInvocation invocation)
        {
            var testCacheAtttribute = this.GetCacheAttributeInfo(invocation.MethodInvocationTarget??invocation.Method);

            if(testCacheAtttribute !=null)
            {
                ProceedCaching(invocation,testCacheAtttribute);
            }
            else
            {
                invocation.Proceed();
            }
        }

        private TestCacheAttribute GetCacheAttributeInfo(MethodInfo method)
        {
            return method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(TestCacheAttribute)) as TestCacheAttribute;
        }


        private void ProceedCaching(IInvocation invocation,TestCacheAttribute cacheAttribute)
        {
            var cacheKey = GenerateCacheKey(invocation);

            var cacheValue = CacheProvider.Get(cacheKey);
            if (cacheValue != null)
            {
                invocation.ReturnValue = cacheValue;
                return;
            }

            invocation.Proceed();

            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                CacheProvider.Set(cacheKey, invocation.ReturnValue, TimeSpan.FromSeconds(cacheAttribute.AbsoluteExpiration));
            }

        }

        private string GenerateCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = this.FormatArgumentsToPartOfCacheKey(invocation.Arguments);
            return GenerateCacheKey(typeName,methodName,methodArguments);
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
