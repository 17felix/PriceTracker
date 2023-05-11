namespace CleanArchitecture.Application.Common.Interfaces.Connection
{
    public static class CacheKeys
    {
        public static string GetTenantConnectionstringCacheKey(long tenantId, string contextName)
        {
            return $"{tenantId}_{contextName}";
        }
    }
}