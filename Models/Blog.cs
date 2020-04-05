using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlogDemo.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string BlogTitle { get; set; }
        public string BlogText { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}