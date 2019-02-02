using Gilazo.ServiceRegistrar.Application;

namespace Gilazo.ServiceRegistrar.Presentation.WebApi.Models
{
    public struct ServiceRegistrationRequest
    {
        public ServiceRegistrationRequest(Service service)
        {
            Service = service;
        }

        public Service Service { get; }
    }
}
