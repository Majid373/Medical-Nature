using Application.Interfaces.Contexts;
using Application.Convertors;
using Domain.Appointments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Appointments
{
    public interface IAppointmentService
    {
        void AddAppointment(Appointment apointment);
        List<Appointment> GetAllAppointments(string userId);
        List<Appointment> GetTextualAppointment(string userId);
        List<Appointment> GetByPhoneAppointment(string userId);
        Appointment GetAppointmentById(int id);
        void UpdateAppointment(Appointment apointment);
        void DeleteAppointment(Appointment appointment);
        List<Appointment> GetAppointmentOfUser(string userId);
        Task RemoveExpiredAppointmentsAsync();

    }

    public class AppointmentService : IAppointmentService
    {
        private readonly IDataBaseContext _context;

        public AppointmentService(IDataBaseContext context)
        {
            _context = context;
        }

        public void AddAppointment(Appointment appointment)
        {
            TimeOnly time = new TimeOnly(appointment.Time.Hour, appointment.Time.Minute, 0);
            appointment.Time = time;
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }

        public void DeleteAppointment(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
        }

        public List<Appointment> GetAllAppointments(string userId)
        {
            var data = _context.Appointments.Where(a => a.UserId == userId)
                .Include(a => a.User)
                .OrderBy(a => a.Date)
                .ThenBy(a => a.Time).ToList();
            return data;
        }

        public Appointment GetAppointmentById(int id)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == id);
            return appointment;
        }

        public List<Appointment> GetAppointmentOfUser(string userId)
        {
            var data = _context.Appointments.Where(u => u.SickId == userId).ToList();

            return data;


        }

        public List<Appointment> GetByPhoneAppointment(string userId)
        {
            DateOnly currentDate = Application.Convertors.DateConvertor.ConvertToShamsi(DateTime.Now);
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);



            var data = _context.Appointments
                .Where(a => a.UserId == userId &&
                a.Type == 1 &&
                !a.IsReserved &&
                 (a.Date == currentDate && a.Time > currentTime || a.Date > currentDate)
                ).ToList();

            return data;

        }

        public List<Appointment> GetTextualAppointment(string userId)
        {
            DateOnly currentDate = Application.Convertors.DateConvertor.ConvertToShamsi(DateTime.Now);
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            return _context.Appointments
                .Where(a => a.UserId == userId && a.Type == 0 && !a.IsReserved
                &&
                 (a.Date == currentDate && a.Time > currentTime || a.Date > currentDate)).ToList();
        }

        public async Task RemoveExpiredAppointmentsAsync()
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);

            var expiredAppointments = await _context.Appointments
                .Where(a => !a.IsReserved && (a.Date < currentDate || (a.Date < currentDate &&
                a.Time < currentTime))).ToListAsync();

            _context.Appointments.RemoveRange(expiredAppointments);
            await _context.SaveChangesAsync();
        }

        public void UpdateAppointment(Appointment apointment)
        {
            var data = _context.Appointments.SingleOrDefault(a => a.Id == apointment.Id);
            data.Price = apointment.Price;
            data.Time = apointment.Time;
            data.Date = apointment.Date;
            data.Type = apointment.Type;

            _context.Appointments.Update(data);
            _context.SaveChanges();
        }
    }
}
