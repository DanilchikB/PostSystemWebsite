using MvcPost.Models;
using MvcUser.Models;


namespace MvcLike.Models
{
    public class Like
    {
        public int UserId {get; set;}
        public User User {get; set;}
        public int PostId {get; set;}
        public Post Post {get; set;}


    }
}