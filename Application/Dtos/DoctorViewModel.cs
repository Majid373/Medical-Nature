using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public  class DoctorViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmation { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
