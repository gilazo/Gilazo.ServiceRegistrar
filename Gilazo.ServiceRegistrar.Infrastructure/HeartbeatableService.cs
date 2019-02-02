using System;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using Gilazo.ServiceRegistrar.Application;

namespace Gilazo.ServiceRegistrar.Infrastructure
{
    public sealed class HeartbeatableService : IStatusable<ServiceStatus>
    {
        private readonly Service _service;
        private readonly HttpClient _client;

        public HeartbeatableService(Service service, HttpClient client)
        {
            _service = service;
            _client = client;
        }

        public Task<ServiceStatus> Status => GetPulse();

        private async Task<ServiceStatus> GetPulse()
        {
            if (string.IsNullOrEmpty(_service.StatusUrl)) return ServiceStatus.None;
            
            using (var source = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                var result = await _client.GetAsync(new Uri(_service.StatusUrl), source.Token);
                return result.IsSuccessStatusCode ? ServiceStatus.Up : ServiceStatus.Down;
            }
        }
    }
}
