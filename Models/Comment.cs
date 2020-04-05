using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogDemo.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }

        // defaults to created time
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public virtual ApplicationUser User { get; set; }
        public int BlogId { get; set; }
        // like a foreign key to blog
        public virtual Blog Blog { get; set; }
    }
}