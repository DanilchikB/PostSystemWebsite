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

                // Use a separate instance of the context to verify correct data was saved to database
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
    }
}