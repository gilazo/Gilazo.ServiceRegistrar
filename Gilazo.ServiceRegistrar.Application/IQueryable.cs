using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gilazo.ServiceRegistrar.Application
{
    public interface IQueryable<TIn, TOut>
    {
        Task<List<TOut>> Query(Expression<Func<TIn, bool>> expression);
    }
}
