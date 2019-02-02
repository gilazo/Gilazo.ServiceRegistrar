using System;
using System.Threading.Tasks;

namespace Gilazo.ServiceRegistrar.Application
{
    public sealed class Service
    {
        public Service(string id, string name, string documentationUrl, string statusUrl, ServiceStatus status)
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            Name = name;
            DocumentationUrl = documentationUrl;
            StatusUrl = statusUrl;
            Status = status;
        }

        public string Id { get; }
        public string Name { get; }
        public string DocumentationUrl { get; }
        public string StatusUrl { get; }
        public ServiceStatus Status { get; }
    }
}
