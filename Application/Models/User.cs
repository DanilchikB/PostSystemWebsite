using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MvcPost.Models;

namespace MvcUser.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username not entered")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password not entered")]
        [StringLength(30, MinimumLength=6, ErrorMessage = "Password must be greater than 6 and less than 30 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email not entered")]
        [EmailAddress(ErrorMessage = "Email is not entered correctly")]
        public string Email { get; set; }

        public int Admin { get; set; }
        [DataType(DataType.Date)]
        public DateTime RedisterDate { get; set; }

        public List<Post> Posts { get; set; }
    }
}