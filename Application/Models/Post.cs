using System;
using System.ComponentModel.DataAnnotations;
using MvcUser.Models;

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

        public int UserId { get; set; }
        public User User { get; set; }

    }
}