using Domain.Appointments;
using Domain.Users;

namespace WebSite.Models.ViewModels.Home
{
    public class ReservationViewModel
    {
        public Domain.Users.User User { get; set; }
        public List<Appointment> TextualAppointments { get; set; }
        public List<Appointment> ByPhoneAppointments { get; set; }

    }
}
