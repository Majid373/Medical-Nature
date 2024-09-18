using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Appointments
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int Type { get; set; }
        public bool IsReserved { get; set; }
        public int Price { get; set; }
        public string? SickId { get; set; }


        public User User { get; set; }
        public string UserId { get; set; }

    }
}
