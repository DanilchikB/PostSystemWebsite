using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcUser.Controllers
{
    public class UserController : Controller
    {
        
        // GET: /User/

        public IActionResult Index()
        {
            return View();
        }

        
        // GET: /User/Login/ 

        public IActionResult Login ()
        {
            return View();
        }

        // GET: /User/Registration/

        public IActionResult Registration()
        {
            return View();
        }
    }
}