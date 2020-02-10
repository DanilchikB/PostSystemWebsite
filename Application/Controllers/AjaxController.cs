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


namespace Mvc.Error.Controllers{
    public class AjaxController : Controller
    {
        private readonly DataContext _context;

        public AjaxController(DataContext context)
        {
            _context = context;
        }
        //Like post
        [HttpPost]
        public void Like([FromBody]JsonLike data)
        {
            if(data.Status == "false"){
                Like like = new Like{ UserId = Int32.Parse(User.Identity.Name),PostId = Int32.Parse(data.PostId)};
                _context.Add(like);
                _context.SaveChangesAsync();
            }else{
                User user = _context.User.Include(s=>s.Likes).FirstOrDefault(s => s.Id == Int32.Parse(User.Identity.Name));
                if(user!=null){
                    Like like = user.Likes.FirstOrDefault(s => s.PostId == Int32.Parse(data.PostId));
                    user.Likes.Remove(like);
                    _context.SaveChangesAsync();
                }
            }
            
        }
        
        //post commenting
        [HttpPost]
        public void AddComment([FromBody]JsonComment data){
            Comment comment = new Comment{
                Text=data.Text,
                UserId=Int32.Parse(User.Identity.Name),
                PostId=data.PostId
            };
            _context.Add(comment);
            _context.SaveChangesAsync();
        }
        //get comments for detail post
        [HttpPost]
        public JsonResult GetComments([FromBody]int PostId){
            Post post = _context.Post
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefault(s => s.Id == PostId);
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
}