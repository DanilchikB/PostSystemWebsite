using MvcUser.Models;
using MvcDataContext.Data;
using MvcPost.Models;

namespace HelpTests{
    public static class HelpData{
        public static void SetTestUsers(DataContext context)
        {
            context.User.Add(new User { Id = 1, Username = "Bob", Password = "123456", Email = "bob@bob.bob"});
            context.User.Add(new User{ Id = 2, Username = "Alice", Password = "123456", Email = "alice@alice.alice"});
            context.User.Add(new User { Id = 3, Username = "Misha", Password = "123456", Email = "misha@misha.misha"});
            context.User.Add(new User { Id = 4, Username = "Mark", Password = "123456", Email = "mark@mark.mark"}); 
        }
        public static void GetTestPost(DataContext context)
        {
            context.Post.Add(new Post{
                Id = 1,
                Title = "FirstTitle",
                Description = "FirstDescription",
                Text = "FirstText",
                UserId = 1
            });
            context.Post.Add(new Post{
                Id = 2,
                Title = "SecondTitle",
                Description = "SecondDescription",
                Text = "SecondText",
                UserId = 2
            });
            context.Post.Add(new Post{
                Id = 3,
                Title = "ThirdTitle",
                Description = "ThirdDescription",
                Text = "ThirdText",
                UserId = 2
            });
            context.Post.Add(new Post{
                Id = 4,
                Title = "FourthTitle",
                Description = "FourthDescription",
                Text = "FourthText",
                UserId = 1
            });
            context.Post.Add(new Post{
                Id = 5,
                Title = "FifthTitle",
                Description = "FifthDescription",
                Text = "FifthText",
                UserId = 1
            });
            context.Post.Add(new Post{
                Id = 6,
                Title = "SixthTitle",
                Description = "SixthDescription",
                Text = "SixthText",
                UserId = 2
            });
            context.Post.Add(new Post{
                Id = 7,
                Title = "SeventhTitle",
                Description = "SeventhDescription",
                Text = "SeventhText",
                UserId = 1
            });
        }
        public static Post GetEditTestPost(){
            Post post = new Post(){
                        Id = 6,
                        Title = "EditSixthTitle",
                        Description = "EditSixthDescription",
                        Text = "EditSixthText",
                        UserId = 2
            };
            return post;
        }
    }
}