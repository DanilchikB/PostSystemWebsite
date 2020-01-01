using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MvcUser.Models;
using Microsoft.EntityFrameworkCore;

namespace MvcUser.Controllers
{
    public class UserController : Controller
    {
        private readonly MvcUserContext _context;

        public UserController(MvcUserContext context)
        {
            _context = context;
        }
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
        // GET: /User/Registration/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration([Bind("Id,Login, Password, Email,RedisterDate")] User user)
        {
            if (ModelState.IsValid)
            {
                User login = await _context.User.FirstOrDefaultAsync(b => b.Login == user.Login);
                User email = await _context.User.FirstOrDefaultAsync(b => b.Email == user.Email);
                if(login != null){
                    ViewBag.Message = "Login";
                }
                else if(email != null){
                    ViewBag.Message = "Email";
                }
                else{
                
                _context.Add(user);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                }
            }
            return View(user);
        }
    }
}