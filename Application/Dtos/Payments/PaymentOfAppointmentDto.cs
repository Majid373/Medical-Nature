using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Payments
{
    public class PaymentOfAppointmentDto
    {
        public Guid PaymentId { get; set; }
        public int Amount { get; set; }
    }
}
