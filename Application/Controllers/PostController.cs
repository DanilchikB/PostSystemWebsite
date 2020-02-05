using System;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MvcPost.Models;
using MvcDataContext.Data;
using MvcUser.Models;
using System.Linq;
using paginationPage.Models;
using Microsoft.AspNetCore.Authorization;

namespace MvcPost.Controllers
{
    public class PostController : Controller
    {
        //dependency injection for database context
        private readonly DataContext _context;

        public PostController(DataContext context)
        {
            _context = context;
        }
        // GET: /Post/ 
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        //GET: /Post/List/
        public async Task<IActionResult> List(int page=1)
        {
            int itemsSize = 5;
            int countButton;
            IQueryable<Post> posts = _context.Post.Include(p=>p.User);
            posts = posts.OrderByDescending(x => x.Id);
            int postCount = posts.Count();
            float count = (float)postCount/itemsSize-postCount/itemsSize;
            if(count>0){
                countButton = postCount/itemsSize + 1;
            }else{
                countButton = postCount/itemsSize;
            }

            var items = await posts.Skip((page - 1)*itemsSize).Take(itemsSize).ToListAsync();

            ListPages viewModel = new ListPages{
                Posts = items,
                PageCount = countButton,
                ActualPage = page
            };

            return View(viewModel);
        }
        [Authorize]
        //GET: /Post/MyList/
        public async Task<IActionResult> MyList(int page=1)
        {
            int itemsSize = 5;
            int countButton;

            IQueryable<Post> posts = _context.Post.Include(p=>p.User);
            posts = posts.Where(p => p.UserId == Int32.Parse(User.Identity.Name));
            posts = posts.OrderByDescending(x => x.Id);
            int postCount = posts.Count();

            float count = (float)postCount/itemsSize - postCount/itemsSize;
            if(count>0){
                countButton = postCount/itemsSize + 1;
            }else{
                countButton = postCount/itemsSize;
            }

            ListPages viewModel = new ListPages{
                Posts = await posts.Skip((page - 1)*itemsSize).Take(itemsSize).ToListAsync(),
                PageCount = countButton,
                ActualPage = page
            };
            return View(viewModel);
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
        [Authorize]
        //GET: /Post/Create/
        public IActionResult Create()
        {
            return View();
        }

        //POST: /Post/Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid){
                DateTime thisDay = DateTime.Now;
                post.Date = thisDay;
                Console.WriteLine(thisDay.ToString());
                post.UserId = Int32.Parse(User.Identity.Name);

                _context.Add(post);
                await _context.SaveChangesAsync();
                return View(post);
            }
            return View(post);
        }
        [Authorize]
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
        [Authorize]
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
        
        
    }
}