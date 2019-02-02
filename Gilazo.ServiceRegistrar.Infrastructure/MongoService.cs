using Gilazo.ServiceRegistrar.Application;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gilazo.ServiceRegistrar.Infrastructure
{
    public sealed class MongoService
    {
        public string MongoId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string DocumentationUrl { get; set; }
        public string StatusUrl { get; set; }
        public ServiceStatus Status { get; set; }

        public Service ToService() => new Service(Id, Name, DocumentationUrl, StatusUrl, Status);
    }
}
