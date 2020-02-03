using System;
using System.ComponentModel.DataAnnotations;

namespace MvcFeedback.Models
{
    public class Feedback
    {
        public int Id { get; set; }
       [Required(ErrorMessage = "Email not entered")]
        [EmailAddress(ErrorMessage = "Email is not entered correctly")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Text not entered")]
        public string Text{ get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}