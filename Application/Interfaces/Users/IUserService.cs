using Application.Dtos;
using Application.Interfaces.Contexts;
using Application.Services;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Users
{
    public interface IUserService
    {

        void AddComment(Comment comment);
        PaginatedList<Comment> GetComment(string doctorId, int? pageNumber);
        List<Comment> GetComments(string doctorId);
        List<Comment> GetAllCommentForWebSite();
    }

    public class UserService : IUserService
    {
        private readonly IDataBaseContext _context;

        public UserService(IDataBaseContext context)
        {
            _context = context;
        }

        public void AddComment(Comment newComment)
        {
            newComment.InsertDate = DateTime.Now;
            newComment.IsDelete = false;
            newComment.IsAdminRead = false;

            _context.Comments.Add(newComment);
            _context.SaveChanges();
        }

        public List<Comment> GetAllCommentForWebSite()
        {
            return _context.Comments.Include(u => u.User).Where(u => !u.IsDelete).ToList();
        }

        public PaginatedList<Comment> GetComment(string doctorId, int? pageNumber)
        {
            var data = _context.Comments
                .Include(u => u.User)
                .Where(c => c.DoctorId == doctorId && !c.IsDelete)
                .OrderByDescending(u => u.InsertDate)
                .ToList();

            var pageSize = 5;
            var result = PaginatedList<Comment>.Create(data, pageNumber ?? 1, pageSize);
            return result;
        }

        public List<Comment> GetComments(string doctorId)
        {
            var data = _context.Comments.Include(u => u.User).Where(c => c.DoctorId == doctorId && !c.IsDelete).OrderByDescending(u => u.InsertDate).ToList();

            return data;
        }
    }




}
