using Domain.Appointments;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public bool IsDelete { get; set; }
        public string ImageName { get; set; } = "avatar.jpg";
        public bool IsDoctor { get; set; }
        public string? DegreeOfEducation { get; set; }
        public string Expertise { get; set; }
        public int PriceTextual { get; set; }
        public int PriceByPhone { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
