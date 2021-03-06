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
using Helpers.Pagination;

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
        [Authorize]
        //GET: /Post/List/
        public async Task<IActionResult> List(int page=1)
        {
            Pagination pagination = new Pagination();
            //int itemsSize = 5;
            //int countButton;
            IQueryable<Post> posts = _context.Post
                .Include(p=>p.User)
                .Include(p=>p.Likes)
                    .ThenInclude(sc => sc.Post);
            posts = posts.OrderByDescending(x => x.Id);
            /*float count = (float)postCount/itemsSize-postCount/itemsSize;
            if(count>0){
                countButton = postCount/itemsSize + 1;
            }else{
                countButton = postCount/itemsSize;
            }*/

            var items = await posts.Skip((page - 1)*pagination.itemsSize).Take(pagination.itemsSize).ToListAsync();

            ListPages viewModel = new ListPages{
                Posts = items,
                PageCount = pagination.pageCalculation(posts.Count()),
                ActualPage = page
            };

            return View(viewModel);
        }
        [Authorize]
        //GET: /Post/MyList/
        public async Task<IActionResult> MyList(int page=1)
        {
            Pagination pagination = new Pagination(6);

            IQueryable<Post> posts = _context.Post.Include(p=>p.User);
            posts = posts.Where(p => p.UserId == Int32.Parse(User.Identity.Name));
            posts = posts.OrderByDescending(x => x.Id);
            int postCount = posts.Count();


            ListPages viewModel = new ListPages{
                Posts = await posts.Skip((page - 1)*pagination.itemsSize).Take(pagination.itemsSize).ToListAsync(),
                PageCount = pagination.pageCalculation(posts.Count()),
                ActualPage = page
            };
            return View(viewModel);
        }
        [Authorize]
        //GET: /Post/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = await _context.Post
            .Include(p=>p.User)
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