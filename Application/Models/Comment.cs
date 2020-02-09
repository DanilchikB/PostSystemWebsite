using MvcUser.Models;
using MvcPost.Models;

namespace MvcComment.Models
{
    public class Comment
    {
        public int Id {get; set;}
        public string Text {get; set;}
        public int UserId {get; set;}
        public User User {get; set;}
        public int PostId {get; set;}
        public Post Post {get; set;}


    }
}