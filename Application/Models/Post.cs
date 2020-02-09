using System;
using System.ComponentModel.DataAnnotations;
using MvcUser.Models;
using MvcLike.Models;
using MvcComment.Models;
using System.Collections.Generic;

namespace MvcPost.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title not entered")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description not entered")]
        public string Description{ get; set; }
        [Required(ErrorMessage = "Text not entered")]
        public string Text{ get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        //connection with the User models
        public int UserId { get; set; }
        public User User { get; set; }

        //connection with the Like models
        public List<Like> Likes {get; set;}
        public Post()
        {
            Likes = new List<Like>();
        }
        //connection with the Comment models
        public List<Comment> Comments{get;set;}
    }
}