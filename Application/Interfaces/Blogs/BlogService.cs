using Application.Dtos;
using Application.Interfaces.Contexts;
using Application.Services;
using Domain.Blogs;

namespace Application.Interfaces.Blogs
{
    public class BlogService : IBlogService
    {
        private readonly IDataBaseContext _context;

        public BlogService(IDataBaseContext context)
        {
            _context = context;
        }

        public int CreateBlog(CreateBlogViewModel createBlog)
        {
            if (createBlog.BlogImgUrl != null)
            {
                string imagePath = "";

                createBlog.BlogImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(createBlog.BlogImgUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/BlogImage", createBlog.BlogImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    createBlog.BlogImgUrl.CopyTo(stream);
                }
            }

            Blog newBlog = new Blog()
            {
                Title = createBlog.Title,
                Category = createBlog.Category,
                Description = createBlog.Description,
                ImageName = createBlog.BlogImageName,
                InsertDate = DateTime.Now,
                IsDelete = false,
                Summary = createBlog.Summary,
            };

            _context.Blogs.Add(newBlog);
            _context.SaveChanges();

            return newBlog.Id;
        }

        public void DeleteBlog(int Id)
        {
            var blog = _context.Blogs.FirstOrDefault(b => b.Id == Id);
            blog.IsDelete = true;
            _context.Blogs.Update(blog);
            _context.SaveChanges();
        }

        public int EditBlog(EditBlogViewModel editBlog)
        {
            if (editBlog.ImageUrl != null)
            {
                string imagePath = "";
                if (editBlog.ImageName != "DefaultBlog.png")
                {
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/BlogImage", editBlog.ImageName);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                editBlog.ImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(editBlog.ImageUrl.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/BlogImage", editBlog.ImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    editBlog.ImageUrl.CopyTo(stream);
                }
            }

            var blog = _context.Blogs.SingleOrDefault(b => b.Id == editBlog.Id);
            blog.Title = editBlog.Title;
            blog.Category = editBlog.Category;
            blog.ImageName = editBlog.ImageName;
            blog.Description = editBlog.Description;
            blog.Summary = editBlog.Summary;

            _context.Blogs.Update(blog);
            _context.SaveChanges();
            return blog.Id;

        }

        public List<BlogsViewModel> GetAllBlogs()
        {
            var data = _context.Blogs.Where(b => !b.IsDelete)
                 .Select(b => new BlogsViewModel()
                 {
                     Id = b.Id,
                     Title = b.Title,
                     Category = b.Category,
                     ImageName = b.ImageName,
                     InsertDate = b.InsertDate,
                     Summary = b.Summary,
                 }).ToList();
            return data;

        }

        public PaginatedList<BlogsForWebSiteViewModel> GetAllBlogsForWebSite(int? pageNumber)
        {
            int pageSize = 2;
            var data = PaginatedList<BlogsForWebSiteViewModel>
                .Create(_context.Blogs.Where(b => !b.IsDelete).Select(b => new BlogsForWebSiteViewModel()
                {
                    Id = b.Id,
                    ImageName = b.ImageName,
                    Title = b.Title,
                    Insertdate = b.InsertDate,
                    Summary = b.Summary,
                }).ToList(), pageNumber ?? 1, pageSize);
            return data;
        }

        //For Index

        public List<BlogsForWebSiteViewModel> GetAllBlogsForWebSite()
        {
            var data = _context.Blogs.Where(b => !b.IsDelete).Select(b => new BlogsForWebSiteViewModel()
            {
                Id = b.Id,
                ImageName = b.ImageName,
                Title = b.Title,
                Insertdate = b.InsertDate,
                Summary = b.Summary,
            }).ToList();
            return data;
        }

        public List<BlogsViewModel> GetAllDeletedBlogs()
        {
            var data = _context.Blogs.Where(b => b.IsDelete)
                 .Select(b => new BlogsViewModel()
                 {
                     Id = b.Id,
                     Title = b.Title,
                     Category = b.Category,
                     ImageName = b.ImageName,
                     InsertDate = b.InsertDate,
                     Summary = b.Summary,
                 }).ToList();
            return data;

        }

        public BlogsViewModel GetBlog(int id)
        {
            var data = _context.Blogs.FirstOrDefault(b => b.Id == id);
            return new BlogsViewModel()
            {
                Id = data.Id,
                Category = data.Category,
                InsertDate = data.InsertDate,
                Title = data.Title,
                Summary = data.Summary,
            };
        }

        public BlogDetailViewModel GetBlogDetail(int id)
        {
            var data = _context.Blogs.FirstOrDefault(b => b.Id == id);
            return new BlogDetailViewModel()
            {
                Title = data.Title,
                Category = data.Category,
                InsertDate = data.InsertDate,
                Description = data.Description,
                ImageName = data.ImageName,
            };
        }

        public EditBlogViewModel GetBlogForEdit(int id)
        {
            var data = _context.Blogs.SingleOrDefault(b => b.Id == id);

            return new EditBlogViewModel()
            {
                Id = data.Id,
                Title = data.Title,
                Category = data.Category,
                ImageName = data.ImageName,
                Description = data.Description,
                Summary = data.Summary,
            };
        }
    }
}
