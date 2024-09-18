using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ImageName { get; set; } = "CategoryDefault.jpg";
        public IFormFile? ImageUrl { get; set; }
    }
}
