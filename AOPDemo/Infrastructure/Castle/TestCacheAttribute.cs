using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOPDemo.Infrastructure.Castle
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class TestCacheAttribute:Attribute
    {
        public int AbsoluteExpiration { get; set; } = 30;

        //add other settings ...
    }
}
