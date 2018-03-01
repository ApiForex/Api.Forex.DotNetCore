using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Forex.DotNetCore.Controllers
{
    public class DocumentationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}