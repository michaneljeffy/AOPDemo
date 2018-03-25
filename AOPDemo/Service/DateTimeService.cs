using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AOPDemo.Infrastructure.Castle;

namespace AOPDemo.Service
{
    public class DateTimeService : IDateTimeService,ITestCaching
    {
        [TestCache(AbsoluteExpiration = 10)]
        public string GetCurrentUtcTime()
        {
            return System.DateTime.UtcNow.ToString();
        }
    }

}
