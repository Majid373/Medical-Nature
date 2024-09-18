using Domain.Appointments;

namespace WebSite.Models.ViewModels.User
{
    public class DetailAppointmentViewModel
    {
        public Domain.Users.User User { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
