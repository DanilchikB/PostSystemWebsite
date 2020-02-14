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
using Microsoft.AspNetCore.Http;
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
        public async Task AddComment([FromBody]JsonComment data){
            Comment comment = new Comment{
                Text=System.Web.HttpUtility.HtmlEncode(data.Text),
                UserId=Int32.Parse(User.Identity.Name),
                PostId=data.PostId
            };
            _context.Add(comment);
            await _context.SaveChangesAsync();
        }
        //get comments for detail post
        [HttpPost]
        public async Task<JsonResult> GetComments([FromBody]int PostId){
            Post post = await _context.Post
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(s => s.Id == PostId);
            List<ViewComments> comments = new List<ViewComments>(post.Comments.Count());
            foreach(Comment item in post.Comments){
                comments.Add(new ViewComments{
                    UserName = item.User.Username,
                    Text = item.Text
                });
                Console.WriteLine(item.Text);
                Console.WriteLine(item.User.Username);
                 Console.WriteLine(post.Comments.Count());
            }
            //Console.WriteLine(PostId);
            return Json(comments);
        }
        
        //add avatar
        [HttpPost]
        public async Task AddPhoto(getImage avatar){
            if (avatar != null){
                
                string path = "/users/avatars/" + "test.png";
                
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await avatar.Image.CopyToAsync(fileStream);
                }
                
            }
            
        }
    }
    public class JsonLike{
        public string PostId {get; set;}
        public string Status {get; set; }
    }
    public class JsonComment{
        public int PostId {get; set;}
        public string Text {get; set;}
    }
    public class ViewComments{
        public string UserName {get; set;}
        public string Text {get; set;}
    }
    public class getImage{
        public IFormFile Image {get; set;}
    }
}