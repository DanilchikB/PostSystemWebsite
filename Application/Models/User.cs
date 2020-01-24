using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MvcPost.Models;

namespace MvcUser.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не введен логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не введен пароль")]
        [StringLength(30, MinimumLength=6, ErrorMessage = "Пароль должен быть больше 6 и меньше 30 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не введен Email")]
        [EmailAddress(ErrorMessage = "Не правильно введен Email")]
        public string Email { get; set; }

        public int Admin { get; set; }
        [DataType(DataType.Date)]
        public DateTime RedisterDate { get; set; }

        public List<Post> Posts { get; set; }
    }
}