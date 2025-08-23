using Core.Interfaces;
using Infrastructure.Cache;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class CachedGenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IGenericRepository<T> _inner;
        private readonly IMemoryCache _cache;
        private readonly ICacheKeyTracker _tracker;
        private readonly string _typePrefix;

        public CachedGenericRepository(IGenericRepository<T> inner, IMemoryCache cache, ICacheKeyTracker tracker)
        {
            _inner = inner;
            _cache = cache;
            _tracker = tracker;
            _typePrefix = typeof(T).Name;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var cacheKey = $"{_typePrefix}_Id_{id}";
            if (_cache.TryGetValue(cacheKey, out T? cached))
                return cached;

            var entity = await _inner.GetByIdAsync(id);
            if (entity != null)
            {
                _cache.Set(cacheKey, entity, TimeSpan.FromMinutes(5));
                _tracker.Track(cacheKey);
            }
            return entity;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var cacheKey = $"{_typePrefix}_All";
            if (_cache.TryGetValue(cacheKey, out IReadOnlyList<T>? cachedList) && cachedList is not null)
                return cachedList;

            var list = await _inner.GetAllAsync();
            _cache.Set(cacheKey, list, TimeSpan.FromMinutes(5));
            _tracker.Track(cacheKey);
            return list;
        }

        public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            // در حالت پیشرفته می‌توانیم کش برای query خاص بسازیم
            return await _inner.FindAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            ClearCache();
            await _inner.AddAsync(entity);
        }

        public void Update(T entity)
        {
            ClearCache();
            _inner.Update(entity);
        }

        public void Delete(T entity)
        {
            ClearCache();
            _inner.Delete(entity);
        }

        private void ClearCache()
        {
            var keys = _tracker.KeysStartingWith(_typePrefix).ToList();
            foreach (var k in keys) _cache.Remove(k);
            _tracker.RemoveStartingWith(_typePrefix);
        }
    }
}
