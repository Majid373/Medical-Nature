using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogs
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime  InsertDate { get; set; }
        public string Category { get; set; }
        public string ImageName { get; set; } = "DefaultBlog.png";
        public string Description { get; set; }
        public string Summary { get; set; }
        public bool IsDelete { get; set; }

    }
}
