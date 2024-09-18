using Application.Dtos.Payments;
using Application.Interfaces.Contexts;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Payments
{
    public interface IPaymentService
    {
        PaymentOfAppointmentDto PayForAppointment(int appointmentId);
        PaymentDto GetPayment(Guid id);
        //------------------------
        void AddPayment(Payment payment);
        Payment GetPaymentById(Guid id);
        void UpdatePayment(Payment payment);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IDataBaseContext _context;

        public PaymentService(IDataBaseContext context)
        {
            _context = context;
        }

        public void AddPayment(Payment payment)
        {
            _context.payments.Add(payment);
            _context.SaveChanges();
        }

        public PaymentDto GetPayment(Guid id)
        {
            var payment = _context.payments.Include(p => p.Appointment.User).SingleOrDefault(x => x.Id == id);

            var user = _context.Users.SingleOrDefault(u => u.Id == payment.Appointment.UserId);

            string description = $"پرداخت سفارش شماره {payment.AppointmentId} " + Environment.NewLine;

            PaymentDto paymentDto = new PaymentDto()
            {
                Amount = payment.Amount,
                Email = user.Email,
                UserId = user.Id,
                Id = payment.Id,
                Description = description
            };

            return paymentDto;
        }

        public Payment GetPaymentById(Guid id)
        {
            var payment = _context.payments.SingleOrDefault(x => x.Id == id);
            return payment;
        }

        public PaymentOfAppointmentDto PayForAppointment(int appointmentId)
        {
            var appointment = _context.Appointments.Include(a => a.User).SingleOrDefault(a => a.Id == appointmentId);

            if (appointment == null)
            {
                throw new Exception("");
            }

            var payment = _context.payments.SingleOrDefault(p => p.AppointmentId == appointment.Id);

            //if(payment == null)
            //{
            //    payment = new Payment(appointment.Price, appointment.Id); 
            //    _context.payments.Add(payment);
            //    _context.SaveChanges();
            //}

            return new PaymentOfAppointmentDto()
            {
                Amount = payment.Amount,
                PaymentId = payment.Id,

            };

        }

        public void UpdatePayment(Payment payment)
        {
           _context.payments.Update(payment);
            _context.SaveChanges(); 
        }
    }
}
