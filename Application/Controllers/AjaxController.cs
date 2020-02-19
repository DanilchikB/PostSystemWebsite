using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MvcLike.Models;
using MvcUser.Models;
using System.Collections.Generic;
using MvcDataContext.Data;
using Microsoft.EntityFrameworkCore;
using MvcComment.Models;
using MvcPost.Models;
using System.Threading.Tasks;
using Ajax.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace Mvc.Error.Controllers{
    public class AjaxController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public AjaxController(DataContext context,IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            
        }
        //Like post
        [HttpPost]
        public async Task Like([FromBody]JsonLike data)
        {
            if(data.Status == "false"){
                Like like = new Like{ UserId = Int32.Parse(User.Identity.Name),PostId = Int32.Parse(data.PostId)};
                _context.Add(like);
                await _context.SaveChangesAsync();
            }else{
                User user = _context.User.Include(s=>s.Likes).FirstOrDefault(s => s.Id == Int32.Parse(User.Identity.Name));
                if(user!=null){
                    Like like = user.Likes.FirstOrDefault(s => s.PostId == Int32.Parse(data.PostId));
                    user.Likes.Remove(like);
                    await _context.SaveChangesAsync();
                }
            }
            
        }
        
        //post commenting
        [HttpPost]
        public async Task<JsonResult> AddComment([FromBody]JsonComment data){
            ViewComments viewComment;
            if(data.Text.Trim() != ""){
                User user = _context.User.FirstOrDefault(s => s.Id == Int32.Parse(User.Identity.Name));
                Comment comment = new Comment{
                    Text=System.Web.HttpUtility.HtmlEncode(data.Text),
                    UserId=Int32.Parse(User.Identity.Name),
                    PostId=data.PostId,
                    Date = DateTime.Now
                };
                _context.Add(comment);
                await _context.SaveChangesAsync();
                viewComment = new ViewComments{
                    Id = User.Identity.Name,
                    UserName = user.Username,
                    Avatar = user.Avatar,
                    Date = comment.Date.ToString("dd.MM.yyyy hh:mm tt"),
                    Text = comment.Text,
                    Error = null
                };
            }else{
                viewComment = new ViewComments{
                    Error = "You did not enter a comment"
                };
            }
            return Json(viewComment);
        }
        //get comments for detail post
        [HttpPost]
        public async Task<JsonResult> GetComments([FromBody]int PostId){
            Post post = await _context.Post
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(s => s.Id == PostId);
            List<Comment> sortingComments = post.Comments
                .OrderBy(x => x.Id)
                .ToList();
            List<ViewComments> comments = new List<ViewComments>(post.Comments.Count());
            foreach(Comment item in sortingComments){
                ViewComments view = new ViewComments{
                    Id = User.Identity.Name,
                    UserName = item.User.Username,
                    Avatar = item.User.Avatar,
                    Date = item.Date.ToString("MM.dd.yyyy hh:mm tt"),
                    Text = item.Text
                };
                comments.Add(view);
                
            }
            //Console.WriteLine(PostId);
            return Json(comments);
        }
        
        //add avatar
        [HttpPost]
        public async Task AddPhoto(getImage avatar){
            if (avatar != null){
                string path = "users/avatars/" + User.Identity.Name+".png";
                User user = _context.User.FirstOrDefault(s => s.Id == Int32.Parse(User.Identity.Name));
                user.Avatar = path;
                await _context.SaveChangesAsync();
                
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath +"/"+ path, FileMode.Create))
                {
                    await avatar.Image.CopyToAsync(fileStream);
                }
                
            }
            
        }

        
    }
    
}