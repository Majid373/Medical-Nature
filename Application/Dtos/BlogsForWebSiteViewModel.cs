using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class BlogsForWebSiteViewModel
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime Insertdate { get; set; }
    }
}
