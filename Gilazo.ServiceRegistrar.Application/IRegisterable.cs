using System.Threading.Tasks;

namespace Gilazo.ServiceRegistrar.Application
{
    public interface IRegisterable<T>
    {
         Task Register(T item);
    }
}
