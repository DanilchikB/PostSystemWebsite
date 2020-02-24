using System;
using Microsoft.AspNetCore.Mvc;
using MvcUser.Controllers;
using Xunit;
using MvcDataContext.Data;
using MvcUser.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;

namespace Application.Tests
{
    public class UserControllerTests
    {
        //Check for correct output index page
        [Fact]
        public async Task IndexTest()
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
                    GetTestUsers(context);
                    context.SaveChanges();
                }

                // Use a separate instance of the context to verify correct data was saved to database
                using (var context = new DataContext(options))
                {
                    int testUserId = 1;
                    var controller = new UserController(context);


                    controller.ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, testUserId.ToString())
                            }, "Basic"))
                        }
                    };

                    var result = await controller.Index(testUserId);

                    var viewResult = Assert.IsType<ViewResult>(result);
                    var model = Assert.IsType<User>(viewResult.ViewData.Model);
                    Assert.Equal("Bob", model.Username);
                    Assert.Equal("123456", model.Password);
                    Assert.Equal("bob@bob.bob", model.Email);
                }
            }
            finally
            {
                connection.Close();
            }
        }  
        [Fact]
        public async Task PostLoginTest(){
            var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
            try
            {
                var options = new DbContextOptionsBuilder<DataContext>()
                    .UseSqlite(connection)
                    .Options;
                using (var context = new DataContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new DataContext(options))
                {
                    GetTestUsers(context);
                    context.SaveChanges();
                }

                //check if errors are present
                using (var context = new DataContext(options))
                {
                    var controller = new UserController(context);
                    controller.ModelState.AddModelError("Email", "Required");
                    User newUser = new User();

                    var result = await controller.Login(newUser);


                    var viewResult = Assert.IsType<ViewResult>(result);
                    Assert.Equal(newUser, viewResult?.Model);
                }
                //check if errors are not present
                using(var context = new DataContext(options)){
                    var controller = new UserController(context);
                    controller.ModelState.AddModelError("Email", "");
                    controller.ModelState.AddModelError("Password", "");
                    controller.ModelState["Email"].Errors.Clear();
                    controller.ModelState["Password"].Errors.Clear();

                    User newUser = new User(){
                        Password = "123456", 
                        Email = "bob@bob.bo"
                    };

                    var result = await controller.Login(newUser);

                    
                    var viewResult = Assert.IsType<ViewResult>(result);
                    Assert.Equal("Wrong email or password", viewResult.ViewData["Error"]);
                }

            }
            finally
            {
                connection.Close();
            }
            
        }
        private void GetTestUsers(DataContext context)
        {
            context.User.Add(new User { Id = 1, Username = "Bob", Password = "123456", Email = "bob@bob.bob"});
            context.User.Add(new User{ Id = 2, Username = "Alice", Password = "123456", Email = "alice@alice.alice"});
            context.User.Add(new User { Id = 3, Username = "Misha", Password = "123456", Email = "misha@misha.misha"});
            context.User.Add(new User { Id = 4, Username = "Mark", Password = "123456", Email = "mark@mark.mark"});
                
                
        }
    }
}
