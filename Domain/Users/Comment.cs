using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class Comment
    {
        public int Id { get; set; }
        public string comment { get; set; }
        public DateTime InsertDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsAdminRead { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public string DoctorId { get; set; }



    }
}
