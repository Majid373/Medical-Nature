using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class AppointmentDto
    {
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
    }
}
