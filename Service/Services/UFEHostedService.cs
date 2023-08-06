using Microsoft.Extensions.Hosting;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UFEHostedService : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private IUFEService _uFEService;

        public UFEHostedService(IUFEService uFEService)
        {
            _uFEService = uFEService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(UpdateFee, null, TimeSpan.Zero,
                TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        private void UpdateFee(object? state)
        {
            _uFEService.UpdateFee();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
