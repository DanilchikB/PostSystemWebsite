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
                    context.User.Add(new User { Id = 1, Username = "Bob", Password = "123456", Email = "bob@bob.bob"});
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
    }
}
