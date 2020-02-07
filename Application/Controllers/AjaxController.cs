using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MvcLike.Models;
using MvcUser.Models;
using System.Collections.Generic;
using MvcDataContext.Data;
using Microsoft.EntityFrameworkCore;


namespace Mvc.Error.Controllers{
    public class AjaxController : Controller
    {
        private readonly DataContext _context;

        public AjaxController(DataContext context)
        {
            _context = context;
        }
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
    }
    public class JsonLike{
        public string PostId {get; set;}
        public string Status {get; set; }
    }
}