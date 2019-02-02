using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gilazo.ServiceRegistrar.Application;
using MongoDB.Driver;

namespace Gilazo.ServiceRegistrar.Infrastructure
{
    public sealed class FindableMongoService : Application.IQueryable<MongoService, Service>
    {
        private readonly IMongoCollection<MongoService> _collection;

        public FindableMongoService(IMongoCollection<MongoService> collection)
        {
            _collection = collection;
        }

        public async Task<List<Service>> Query(Expression<Func<MongoService, bool>> expression)
        {
            return (await (await _collection.FindAsync(expression)).ToListAsync()).Select(m => m.ToService()).ToList();
        }
    }
}
