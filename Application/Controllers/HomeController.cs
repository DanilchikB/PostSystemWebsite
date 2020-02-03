using System;
using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Models;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //Feedback
        [HttpPost]
        public JsonResult Feedback([FromBody]FeedbackList data)
        {
            string email = System.Web.HttpUtility.HtmlEncode(data.Email);            
            string text = System.Web.HttpUtility.HtmlEncode(data.Text);
            string massege = "";
            data.Email = data.Email.Trim();
            if(String.IsNullOrEmpty(email)){
                massege = "Not email";
            }
            //string messege = "Not email";
            return Json(massege);
        }
    }
    public class FeedbackList{
        public string Email {get; set;}
        public string Text {get; set; }
    }
    
}
