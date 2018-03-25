using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOPDemo.Service
{
    public interface IDateTimeService2
    {
        string GetCurrentUtcTime();
    }

    public class DateTimeService2:IDateTimeService2
    {
        public string GetCurrentUtcTime()
        {
            return DateTime.UtcNow.ToString();
        }
    }
}
