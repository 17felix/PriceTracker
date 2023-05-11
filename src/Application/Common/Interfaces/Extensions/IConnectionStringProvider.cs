using CleanArchitecture.Application.Common.Interfaces.Connection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CleanArchitecture.Application.Common.Interfaces.Extensions;
public interface IConnectionStringProvider
{
    Task<string> GetAsync<TContext>(CancellationToken cancellationToken) where TContext : class;
}


public class TenantActorConnectionStringProvider : IConnectionStringProvider
{
    private readonly IMemoryCache cache;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TenantActorConnectionStringProvider" /> class.
    /// </summary>
    public TenantActorConnectionStringProvider(IMemoryCache cache)
    {
        this.cache = cache;
    }

    public async Task<string> GetAsync<TContext>(CancellationToken cancellationToken) where TContext : class
    {
        var contextName = typeof(TContext).Name;
        //var tenantId = currentTenantProvider.GetId();
        var tenantId = 1;

        var cacheKey = CacheKeys.GetTenantConnectionstringCacheKey(tenantId, contextName);
        if (!cache.TryGetValue(cacheKey, out DbSettingsDTO dbSettings))
        {
            //dbSettings = await tenantConfiguration.GetDbSettings(tenantId, contextName, cancellationToken);

            var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(cts.Token));

            cache.Set(cacheKey, dbSettings, cacheEntryOptions);
        }

        if (dbSettings == null)
        {
            throw new Exception($"The database settings for cache key '{cacheKey}' could not be found");
        }

        return dbSettings.ConnectionString;
    }
}

public class MessageConnectionStringProvider : IConnectionStringProvider
{
    private readonly IMemoryCache cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageConnectionStringProvider"/> class.
    /// </summary>
    public MessageConnectionStringProvider(IMemoryCache cache)
    {
        this.cache = cache;
    }

    public async Task<string> GetAsync<TContext>(CancellationToken cancellationToken) where TContext : class
    {
        var contextName = typeof(TContext).Name;
        //var tenantId = currentTenantProvider.GetId();
        var tenantId = 1;

        var cacheKey = CacheKeys.GetTenantConnectionstringCacheKey(tenantId, contextName);
        if (!cache.TryGetValue(cacheKey, out DbSettingsDTO dbSettings))
        {
            //dbSettings = await tenantConfiguration.GetDbSettings(tenantId, contextName, cancellationToken);

            var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(cts.Token));

            cache.Set(cacheKey, dbSettings, cacheEntryOptions);
        }

        return dbSettings.ConnectionString;
    }
}