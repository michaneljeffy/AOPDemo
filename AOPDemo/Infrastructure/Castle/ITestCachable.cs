﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOPDemo.Infrastructure.Castle
{
    public interface ITestCachable
    {
        string CacheKey { get; }
    }
}