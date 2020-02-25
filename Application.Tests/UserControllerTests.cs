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
using Helpers.User.PasswordHasher;

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
        [Fact]
        public async Task PostRegistrationTest(){
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

                    var result = await controller.Registration(newUser);


                    var viewResult = Assert.IsType<ViewResult>(result);
                    Assert.Equal(newUser, viewResult?.Model);
                }
                //check if errors are not present
                using(var context = new DataContext(options)){
                    
                    var controller = new UserController(context);
                    User newUser = new User(){
                        Username = "Misha",
                        Password = "123456", 
                        Email = "bob@bob.bo"
                    };

                    var result = await controller.Registration(newUser);


                    var viewResult = Assert.IsType<ViewResult>(result);
                    Assert.Equal("A user with this username exists", viewResult.ViewData.ModelState["Username"].Errors[0].ErrorMessage);
                }
                //check right registration
                using(var context = new DataContext(options)){

                    var controller = new UserController(context);
                    User newUser = new User(){
                        Username = "David",
                        Email = "david@david.david",
                        Password = "123123"
                    };
                    PasswordHasher ph = new PasswordHasher();
                    
                    var result = await controller.Registration(newUser);
                    User resultUser = await context.User.FirstOrDefaultAsync(u=>u.Email=="david@david.david");

                    var viewResult = Assert.IsType<RedirectToActionResult>(result);
                    Assert.Equal("Login", viewResult.ActionName);
                    Assert.Equal("User", viewResult.ControllerName);
                    Assert.Equal("david@david.david", resultUser.Email);
                    Assert.True(ph.Check(resultUser.Password, "123123"));
                }

            }
            finally
            {
                connection.Close();
            }
        }

        //help data
        private void GetTestUsers(DataContext context)
        {
            context.User.Add(new User { Id = 1, Username = "Bob", Password = "123456", Email = "bob@bob.bob"});
            context.User.Add(new User{ Id = 2, Username = "Alice", Password = "123456", Email = "alice@alice.alice"});
            context.User.Add(new User { Id = 3, Username = "Misha", Password = "123456", Email = "misha@misha.misha"});
            context.User.Add(new User { Id = 4, Username = "Mark", Password = "123456", Email = "mark@mark.mark"}); 
        }
    }
}
