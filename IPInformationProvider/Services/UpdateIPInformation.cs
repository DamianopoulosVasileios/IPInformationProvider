using IPInformationProvider.API.Interfaces;

namespace IPInformationProvider.API.Services
{
    public class UpdateIPInformation : IHostedService, IDisposable
    {
        private readonly IIPService _ipService;
        private Timer? _timer = null;

        public UpdateIPInformation(IIPService ipService)
        {
            _ipService = ipService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(60));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            Task.Run(_ipService.UpdateIPInformation);
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
