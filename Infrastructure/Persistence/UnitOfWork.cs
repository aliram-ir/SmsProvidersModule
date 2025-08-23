using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly Hashtable _repositories = new();

        public UnitOfWork(AppDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                // Resolve inner GenericRepository<TEntity>
                var inner = (IGenericRepository<TEntity>)_serviceProvider.GetRequiredService(typeof(GenericRepository<TEntity>));
                // Resolve CachedGenericRepository<TEntity> via activator
                var cached = ActivatorUtilities.CreateInstance<CachedGenericRepository<TEntity>>(_serviceProvider, inner);
                _repositories[type] = cached;
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
