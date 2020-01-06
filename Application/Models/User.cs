using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MvcPost.Models;

namespace MvcUser.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        public int Admin { get; set; }
        [DataType(DataType.Date)]
        public DateTime RedisterDate { get; set; }

        public List<Post> Posts { get; set; }
    }
}