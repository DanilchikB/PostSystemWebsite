using System;
using Microsoft.AspNetCore.Mvc;

namespace Mvc.Error.Controllers{
    public class HomeController : Controller
    {
        [Route("/Error")]
        public IActionResult HttpStatus()
        {
            return View("NotFound");
        }
    }
}