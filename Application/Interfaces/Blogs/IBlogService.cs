using Application.Dtos;
using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Interfaces.Blogs
{
    public  interface IBlogService
    {
        List<BlogsViewModel> GetAllBlogs();
        List<BlogsViewModel> GetAllDeletedBlogs();
        BlogsViewModel GetBlog(int id);
        EditBlogViewModel GetBlogForEdit(int id);
        int CreateBlog(CreateBlogViewModel  createBlog);
        void DeleteBlog(int Id);
        int EditBlog(EditBlogViewModel editBlog);
        PaginatedList<BlogsForWebSiteViewModel> GetAllBlogsForWebSite(int? pageNumber);
        List<BlogsForWebSiteViewModel> GetAllBlogsForWebSite();
        BlogDetailViewModel GetBlogDetail(int id);

    }
}
