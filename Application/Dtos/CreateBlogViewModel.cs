using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class CreateBlogViewModel
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public IFormFile? BlogImgUrl { get; set; }
        public string? BlogImageName { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
    }
}
