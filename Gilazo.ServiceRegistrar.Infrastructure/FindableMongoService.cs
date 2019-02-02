using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gilazo.ServiceRegistrar.Application;
using MongoDB.Driver;

namespace Gilazo.ServiceRegistrar.Infrastructure
{
    public sealed class FindableMongoService : Application.IQueryable<Service>
    {
        private readonly IMongoCollection<Service> _collection;

        public FindableMongoService(IMongoCollection<Service> collection)
        {
            _collection = collection;
        }

        public async Task<List<Service>> Query(Expression<Func<Service, bool>> expression)
        {
            return await (await _collection.FindAsync(expression)).ToListAsync();
        }
    }
}
