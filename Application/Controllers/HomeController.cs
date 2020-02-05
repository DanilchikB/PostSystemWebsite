using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Models;
using MvcDataContext.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using MvcFeedback.Models;
using Microsoft.AspNetCore.Authorization;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        //Feedback
        [HttpPost]
        public JsonResult Feedback([FromBody]FeedbackList data)
        {
            //remove the indents at the beginning and at the end, decode the tags
            data.Email = System.Web.HttpUtility.HtmlEncode(data.Email).Trim(); 
            data.Text = System.Web.HttpUtility.HtmlEncode(data.Text).Trim();

            //messages
            Dictionary<string, string> message = new Dictionary<string, string>();
             

            if(String.IsNullOrEmpty(data.Email)){
                message.Add("ErrEmail","You didn't enter an Email");
            }else if(!new EmailAddressAttribute().IsValid(data.Email)){
                message.Add("ErrEmail","You entered the email incorrectly"); 
            }
            if(String.IsNullOrEmpty(data.Text)){
                message.Add("ErrText","You didn't enter an text");
            }
            if(message.Count == 0){
                Feedback feedback = new Feedback();
                feedback.Email = data.Email;
                feedback.Text = data.Text; 
                _context.Add(feedback);
                _context.SaveChangesAsync();
                message.Add("Success","Success");
            }
            return Json(message);
        }
    }
    public class FeedbackList{
        public string Email {get; set;}
        public string Text {get; set; }
    }
    
}
