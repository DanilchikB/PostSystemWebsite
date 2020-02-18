using System;
using System.ComponentModel.DataAnnotations;
using MvcUser.Models;
using MvcPost.Models;


namespace MvcComment.Models
{
    public class Comment
    {
        public int Id {get; set;}
        public string Text {get; set;}

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int UserId {get; set;}
        public User User {get; set;}
        public int PostId {get; set;}
        public Post Post {get; set;}


    }
}