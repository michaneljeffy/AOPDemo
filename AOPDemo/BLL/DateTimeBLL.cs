using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AOPDemo.Infrastructure.Castle;

namespace AOPDemo.BLL
{
    public class DateTimeBLL:ITestCaching
    {

        [TestCache(AbsoluteExpiration =30)]
        public virtual string GetCurrentUtcTime()
        {
            return DateTime.UtcNow.ToString();
        }
    }
}
