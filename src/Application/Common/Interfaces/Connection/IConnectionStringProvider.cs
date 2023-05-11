using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces.Connection
{
    public interface IConnectionStringProvider
    {
        Task<string> GetAsync<TContext>(CancellationToken cancellationToken) where TContext : class;
    }
}