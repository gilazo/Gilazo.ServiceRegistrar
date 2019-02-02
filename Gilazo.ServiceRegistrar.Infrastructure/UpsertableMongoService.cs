using System.Threading.Tasks;
using Gilazo.ServiceRegistrar.Application;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gilazo.ServiceRegistrar.Infrastructure
{
    public sealed class UpsertableMongoService : IRegisterable<Service>
    {
        private readonly IMongoCollection<MongoService> _collection;

        public UpsertableMongoService(IMongoCollection<MongoService> collection)
        {
            _collection = collection;
        }

        public Task Register(Service service)
        {
            var mongoService = new MongoService
            {
                Id = service.Id,
                Name = service.Name,
                DocumentationUrl = service.DocumentationUrl,
                StatusUrl = service.StatusUrl,
                Status = service.Status
            };
            return _collection.ReplaceOneAsync(d => d.Id == mongoService.Id, mongoService, new UpdateOptions {  IsUpsert = true });
        }
    }
}
