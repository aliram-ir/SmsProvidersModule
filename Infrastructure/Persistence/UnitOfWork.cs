using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private Hashtable? _repositories;

        public UnitOfWork(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            _repositories ??= new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var innerRepo = new GenericRepository<TEntity>(_context);
                var cachedRepo = new CachedGenericRepository<TEntity>(innerRepo, _cache);
                _repositories.Add(type, cachedRepo);
            }

            return (IGenericRepository<TEntity>)_repositories[type]!;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
