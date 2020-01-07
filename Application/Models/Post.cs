using System;
using System.ComponentModel.DataAnnotations;
using MvcUser.Models;

namespace MvcPost.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description{ get; set; }
        public string Text{ get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

    }
}