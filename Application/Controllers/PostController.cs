using System;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MvcPost.Models;
using MvcUser.Data;
using MvcUser.Models;

namespace MvcPost.Controllers
{
    public class PostController : Controller
    {
        //dependency injection for database context
        private readonly MvcUserContext _context;

        public PostController(MvcUserContext context)
        {
            _context = context;
        }
        // GET: /Post/ 
        public IActionResult Index()
        {
            return View();
        }

        //GET: /Post/List/
        public async Task<IActionResult> List()
        {
            var posts = _context.Post.Include(p=>p.User);
            return View(await posts.ToListAsync());
        }
        //GET: /Post/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = await _context.Post
            .FirstOrDefaultAsync(m => m.Id == id);
            if(post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        //GET: /Post/Create/
        public IActionResult Create()
        {
            return View();
        }

        //POST: /Post/Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user, Post post)
        {
            DateTime thisDay = DateTime.Now;
            post.Date = thisDay;
            Console.WriteLine(thisDay.ToString());
            post.UserId = Int32.Parse(User.Identity.Name);

            _context.Add(post);
            await _context.SaveChangesAsync();
            return View(post);
        }

        //GET: /Post/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        //POST: /Post/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {   
                if(post.UserId.ToString() == User.Identity.Name){
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));
                }else{
                    return NotFound();
                }
            }
            return View(post);
        }
        //GET: /Post/Delete/
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
        //POST: /Post/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            
            var post = await _context.Post.FindAsync(id);
            if(post.UserId.ToString() == User.Identity.Name){
                _context.Post.Remove(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }else{
                return NotFound();
            }
        }
        
        //GET: /Post/MyList/
    }
}