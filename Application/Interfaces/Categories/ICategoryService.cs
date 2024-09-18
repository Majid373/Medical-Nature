using Application.Dtos;
using Application.Interfaces.Contexts;
using Domain.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Categories
{
    public interface ICategoryService
    {
        CategoryViewModel GetCategory(int id);
        List<CategoryViewModel> GetAllCategory();
        void CreateCategory(CategoryViewModel category);
        void DeleteCategory(int id);
        void UpdateCategory(CategoryViewModel category);
    }


    public class CategoryService : ICategoryService
    {
        private readonly IDataBaseContext _context;

        public CategoryService(IDataBaseContext context)
        {
            _context = context;
        }


        public void CreateCategory(CategoryViewModel category)
        {
            if (category.ImageUrl != null)
            {
                string imagePath = "";

                category.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(category.ImageUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CategoryImage", category.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    category.ImageUrl.CopyTo(stream);
                }
            }

            Category newCategory = new Category()
            {
                ImageName = category.ImageName,
                Name = category.Name,
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }

        public List<CategoryViewModel> GetAllCategory()
        {
            var data = _context.Categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                ImageName = c.ImageName,
            }).ToList();

            return data;
        }

        public CategoryViewModel GetCategory(int id)
        {
            var data = _context.Categories.FirstOrDefault(c => c.Id == id);

            return new CategoryViewModel
            {
                Id = data.Id,
                Name = data.Name,
                ImageName = data.ImageName,
            };
        }

        public void UpdateCategory(CategoryViewModel category)
        {
            if (category.ImageUrl != null)
            {
                string imagePath = "";
                if (category.ImageName != "CategoryDefault.jpg")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CategoryImage", category.ImageName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                category.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(category.ImageUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CategoryImage", category.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    category.ImageUrl.CopyTo(stream);
                }
            }

            var result = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            result.Name = category.Name;
            result.ImageName = category.ImageName;
            _context.Categories.Update(result);
            _context.SaveChanges();
        }
    }
}
