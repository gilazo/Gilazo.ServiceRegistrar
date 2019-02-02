using System.Threading.Tasks;

namespace Gilazo.ServiceRegistrar.Application
{
    public interface IStatusable<T>
    {
        Task<T> Status { get; }
    }
}
