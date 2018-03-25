using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AOPDemo.BLL;
using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AOPDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/Bll")]
    public class BllController : Controller
    {
        private ILifetimeScope _scope;

        private DateTimeBLL _dateTimeBLL;

        public BllController(ILifetimeScope scope,DateTimeBLL dateTimeBLL)
        {
            this._dateTimeBLL = dateTimeBLL;
            this._scope = scope;
        }

        public IActionResult Index()
        {
            return Content(_dateTimeBLL.GetCurrentUtcTime());
        }
    }
}