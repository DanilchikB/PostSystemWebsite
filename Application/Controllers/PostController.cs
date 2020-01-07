using System;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            return View(await _context.Post.ToListAsync());
        }

        //GET: /Post/Details/
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
            
            _context.Add(post);
            await _context.SaveChangesAsync();
            return View(post);
        }
    }
}