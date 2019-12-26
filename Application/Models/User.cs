using System;
using System.ComponentModel.DataAnnotations;

namespace MvcUser.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Admin { get; set; }
        [DataType(DataType.Date)]
        public DateTime RedisterDate { get; set; }
    }
}