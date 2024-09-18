using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class BlogDetailViewModel
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime  InsertDate { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
    }
}
