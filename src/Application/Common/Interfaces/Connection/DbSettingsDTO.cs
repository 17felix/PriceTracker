namespace CleanArchitecture.Application.Common.Interfaces.Connection;
public class DbSettingsDTO
{
    public long Id { get; set; }

    public string DatabaseName { get; set; }

    public string ConnectionString { get; set; }

    public long TenantId { get; set; }
}