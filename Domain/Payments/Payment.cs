using Domain.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payments
{
    public class Payment
    {

        public Guid Id { get; set; }
        public int Amount { get; set; }
        public bool IsPay { get; set; } = false;
        public DateTime? DatePay { get; set; }
        public string? Authority { get; set; }
        public long RefId { get; set; } = 0;
        public bool IsRemove { get; set; }


        public Appointment Appointment { get; set; }
        public int AppointmentId { get; set; }

    }
}
