using System;
using System.ComponentModel.DataAnnotations;
using MvcUser.Models;

namespace MvcPost.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не введено название")]
        public string Title { get; set; }
        public string Description{ get; set; }
        [Required(ErrorMessage = "Не введен текст")]
        public string Text{ get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

    }
}