using Application.Interfaces.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Appointments
{
    public class AppointmentCleanupService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAppointmentService _appointmentService;
        private Timer _timer;

        public AppointmentCleanupService( IAppointmentService appointmentService,
            IServiceScopeFactory serviceScopeFactory)
        {

            _appointmentService = appointmentService;
            _serviceScopeFactory = serviceScopeFactory;
        }


        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)); 
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
                await appointmentService.RemoveExpiredAppointmentsAsync();
            }
        }
        private async Task RemoveExpiredAppointmentsAsync()
        {
            _appointmentService.RemoveExpiredAppointmentsAsync();
        }
    }
}
