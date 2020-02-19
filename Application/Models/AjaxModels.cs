using Microsoft.AspNetCore.Http;

namespace Ajax.Models{
    public class JsonLike{
        public string PostId {get; set;}
        public string Status {get; set; }
    }
    public class JsonComment{
        public int PostId {get; set;}
        public string Text {get; set;}
    }
    public class ViewComments{
        public string Id {get; set;}
        public string UserName {get; set;}
        public string Avatar {get; set;}
        public string Date {get; set;}
        public string Text {get; set;}
        public string Error {get; set;}
    }
    public class getImage{
        public IFormFile Image {get; set;}
    }
}