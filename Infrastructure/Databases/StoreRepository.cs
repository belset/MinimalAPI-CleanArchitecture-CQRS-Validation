using Application.Stores;
using Application.Stores.Entities;

using Mapster;

using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Databases;

internal class StoreRepository : IStoreRepository
{
    private readonly DataContext context;
    private readonly TimeProvider timeProvider;

    public StoreRepository(DataContext context, TimeProvider timeProvider)
    {
        this.context = context;
        this.timeProvider = timeProvider;
    }

    public virtual async ValueTask<List<Store>> GetStores(CancellationToken cancellationToken)
    {
        var stores = await context.Stores
                        .Include(x => x.Products)
                        .ThenInclude(x => x.Brand)
                        .AsNoTracking()
                        .ProjectToType<Store>()
                        .ToListAsync(cancellationToken);

        return stores;
    }

#nullable enable
    public virtual async ValueTask<Store?> GetStoreById(Guid id, CancellationToken cancellationToken)
    {
        var store = await context.Stores
                        .Where(x => x.Id == id)
                        .Include(x => x.Products)
                        .ThenInclude(x => x.Brand)
                        .AsNoTracking()
                        .ProjectToType<Store>()
                        .FirstOrDefaultAsync(cancellationToken);

        return store;
    }

    public virtual async ValueTask<Store?> GetStoreByName(string storeName, CancellationToken cancellationToken)
    {
        var store = await context.Stores
                        .Where(x => x.StoreName == storeName)
                        .Include(x => x.Products)
                        .ThenInclude(x => x.Brand)
                        .AsNoTracking()
                        .ProjectToType<Store>()
                        .FirstOrDefaultAsync(cancellationToken);

        return store;
    }
#nullable disable

    public virtual async ValueTask<bool> StoreExists(Guid id, CancellationToken cancellationToken)
    {
        return await context.Stores.AnyAsync(x => x.Id == id, cancellationToken);
    }
    public virtual async ValueTask<bool> StoreExists(string storeName, CancellationToken cancellationToken)
    {
        return await context.Stores.AnyAsync(x => x.StoreName == storeName, cancellationToken);
    }
}
