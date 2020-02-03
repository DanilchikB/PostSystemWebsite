using System;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MvcUser.Models;
using MvcDataContext.Data;
using Microsoft.EntityFrameworkCore;
using Helpers.User.PasswordHasher;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace MvcUser.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }
        // GET: /User/
        [Authorize]
        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated){
                return View();
            }else{
                return RedirectToAction("Login","User");
            }
        }

        
        // GET: /User/Login/ 

        public IActionResult Login ()
        {
            if(User.Identity.IsAuthenticated){
                return RedirectToAction("Index","User");
            }else{
                return View();
            }

        }
        // POST: /User/Login/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Id, Login,Password,Email,RedisterDate")] User user)
        {
            User login = await _context.User.FirstOrDefaultAsync(b => b.Email == user.Email);
            if(login != null){
                //Check password
                PasswordHasher ph = new PasswordHasher();
                bool check = ph.Check(login.Password, user.Password);
                
                if(check){
                    string userId = login.Id.ToString();
                    await Authenticate(userId);
                    ViewBag.Message = "Success";
                    return RedirectToAction("Index","User");
                }
            }
            
            return View(user);
        }
        // GET: /User/Logout/
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        // GET: /User/Registration/

        public IActionResult Registration()
        {
            if(User.Identity.IsAuthenticated){
                return RedirectToAction("Index","User");
            }else{
                return View();
            }
        }
        // POST: /User/Registration/
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
                    ViewBag.Message = "Success";
                
                PasswordHasher ph = new PasswordHasher();
                user.Password = ph.Hash(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "User");
                }
            }
            return View(user);
        }
        

        //helper method for authorization using cookies
        private async Task Authenticate(string userId)
        {
            
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userId)
            };
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
    new ClaimsPrincipal(claimsIdentity));
        }
    }
}