using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gilazo.ServiceRegistrar.Application
{
    public interface IQueryable<T>
    {
        Task<List<T>> Query(Expression<Func<T, bool>> expression);
    }
}
