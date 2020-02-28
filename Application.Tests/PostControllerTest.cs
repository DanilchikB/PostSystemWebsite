using Microsoft.AspNetCore.Mvc;
using MvcPost.Controllers;
using Xunit;
using MvcDataContext.Data;
using MvcUser.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using paginationPage.Models;
using HelpTests;
using System.Linq;
using MvcPost.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Application.Tests{

    public class PostControllerTests
    {
        [Fact]
        public async Task ListTest()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
            try
            {
                var options = new DbContextOptionsBuilder<DataContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new DataContext(options))
                {
                    context.Database.EnsureCreated();
                }

                // Run the test against one instance of the context
                using (var context = new DataContext(options))
                {
                    HelpData.SetTestUsers(context);
                    HelpData.GetTestPost(context);
                    context.SaveChanges();
                }

                // news output check
                using (var context = new DataContext(options))
                {
                    var controller = new PostController(context);

                    var result = await controller.List(1);


                    var viewResult = Assert.IsType<ViewResult>(result);
                    var model = Assert.IsType<ListPages>(viewResult.ViewData.Model);

                    Post testPost = null;
                    foreach(var post in model.Posts){
                        if(post.Id == 7){
                            testPost = post;
                        }
                    }

                    Assert.Equal(5,model.Posts.Count());
                    Assert.NotNull(testPost);
                    Assert.Equal("SeventhTitle", testPost.Title);

                }
            }
            finally
            {
                connection.Close();
            }
        }

         [Fact]
        public async Task PostEditTest()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
            try
            {
                var options = new DbContextOptionsBuilder<DataContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new DataContext(options))
                {
                    context.Database.EnsureCreated();
                }

                // Run the test against one instance of the context
                using (var context = new DataContext(options))
                {
                    HelpData.SetTestUsers(context);
                    HelpData.GetTestPost(context);
                    context.SaveChanges();
                }

                //check editing post: wrong user
                using (var context = new DataContext(options))
                {
                    var controller = new PostController(context);
                    Post post = HelpData.GetEditTestPost();

                    var result = await controller.Edit(1, post);


                    var viewResult = Assert.IsType<NotFoundResult>(result);
                }
                //the user is trying to change the record of another user
                using (var context = new DataContext(options))
                {
                    var controller = new PostController(context);
                    controller.ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, "1")
                            }, "Basic"))
                        }
                    };
                    Post post = HelpData.GetEditTestPost();

                    var result = await controller.Edit(6, post);


                    var viewResult = Assert.IsType<NotFoundResult>(result);
                }
                //user is logged
                using (var context = new DataContext(options)){
                    var controller = new PostController(context);
                    controller.ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, "2")
                            }, "Basic"))
                        }
                    };
                    Post post = HelpData.GetEditTestPost();
                    
                    var result = await controller.Edit(6, post);
                    Post resultPost = await context.Post.FirstOrDefaultAsync(p=>p.Id == 6);
    
                    var viewResult = Assert.IsType<RedirectToActionResult>(result);
                    Assert.Equal("EditSixthTitle", resultPost.Title);
                    Assert.Equal("EditSixthDescription", resultPost.Description);
                    Assert.Equal("EditSixthText", resultPost.Text);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}