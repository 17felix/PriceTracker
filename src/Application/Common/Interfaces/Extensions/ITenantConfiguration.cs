using CleanArchitecture.Application.Common.Interfaces.Connection;

namespace CleanArchitecture.Application.Common.Interfaces.Extensions;
public interface ITenantConfiguration
{
    Task<IEnumerable<TenantDTO>> GetAllTenants(CancellationToken cancellationToken);

    Task<TenantDTO> GetTenant(long tenantId, CancellationToken cancellationToken);

    Task<DbSettingsDTO> GetDbSettings(long tenantId, string databaseName, CancellationToken cancellationToken);
}