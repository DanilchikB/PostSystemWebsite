using System.Collections.Generic;
using MvcPost.Models;

namespace paginationPage.Models
{
    public class ListPages
    {
        public IEnumerable<Post> Posts {get; set;}
        public int PageCount {get; set;}
        public int ActualPage {get; set;}
    }
}