using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Gilazo.ServiceRegistrar.Application;
using Gilazo.ServiceRegistrar.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gilazo.ServiceRegistrar.Presentation.WebApi.Services
{
    public sealed class HeartbeatUpdatableService : IHostedService
    {
        private readonly IServiceProvider _provider;

        public HeartbeatUpdatableService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _provider.CreateScope())
            {
                await StartAsync(
                    scope.ServiceProvider.GetRequiredService<Application.IQueryable<MongoService, Service>>(),
                    scope.ServiceProvider.GetRequiredService<IRegisterable<Service>>(),
                    scope.ServiceProvider.GetRequiredService<HttpClient>()
                );
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task StartAsync(
            Application.IQueryable<MongoService, Service> queryableServices, 
            IRegisterable<Service> registerableService,
            HttpClient client)
        {
            while (true)
            {
                var registeredServices = await queryableServices.Query(_ => true);
                foreach(var s in registeredServices)
                {
                    var updatedService = new Service(
                        s.Id, 
                        s.Name, 
                        s.DocumentationUrl, 
                        s.StatusUrl,
                        await new HeartbeatableService(s, client).Status
                    );
                    await registerableService.Register(updatedService);
                }
                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }
    }
}
