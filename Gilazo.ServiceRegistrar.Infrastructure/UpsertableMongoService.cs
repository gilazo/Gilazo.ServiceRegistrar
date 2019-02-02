using System.Threading.Tasks;
using Gilazo.ServiceRegistrar.Application;
using MongoDB.Driver;

namespace Gilazo.ServiceRegistrar.Infrastructure
{
    public sealed class UpsertableMongoService
    {
        private readonly IMongoCollection<Service> _collection;

        public UpsertableMongoService(IMongoCollection<Service> collection)
        {
            _collection = collection;
        }

        public Task Upsert(Service service)
        {
            return _collection.ReplaceOneAsync(d => d.Id == service.Id, service, new UpdateOptions {  IsUpsert = true });
        }
    }
}
