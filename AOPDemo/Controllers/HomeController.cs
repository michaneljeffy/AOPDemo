using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AOPDemo.Service;

namespace AOPDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : Controller
    {
        private IDateTimeService DateTimeService;
        public HomeController(IDateTimeService _dateTimeService)
        {
            DateTimeService = _dateTimeService;
        }

        public IActionResult Index()
        {
            return Content(DateTimeService.GetCurrentUtcTime());
        }
    }
}