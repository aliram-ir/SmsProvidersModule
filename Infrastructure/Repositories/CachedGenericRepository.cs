using Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class CachedGenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IGenericRepository<T> _inner;
        private readonly IMemoryCache _cache;

        public CachedGenericRepository(IGenericRepository<T> inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var cacheKey = $"{typeof(T).Name}_Id_{id}";
            if (_cache.TryGetValue(cacheKey, out T? cachedEntity))
                return cachedEntity;

            var entity = await _inner.GetByIdAsync(id);
            if (entity != null)
                _cache.Set(cacheKey, entity, TimeSpan.FromMinutes(5));

            return entity;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            var cacheKey = $"{typeof(T).Name}_All";
            if (_cache.TryGetValue(cacheKey, out IReadOnlyList<T>? cachedList) && cachedList is not null)
                return cachedList;

            var list = await _inner.GetAllAsync();
            _cache.Set(cacheKey, list, TimeSpan.FromMinutes(5));
            TrackKey(cacheKey);
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

        private readonly HashSet<string> _cacheKeys = new HashSet<string>();

        private void TrackKey(string key)
        {
            _cacheKeys.Add(key);
        }

        private void ClearCache()
        {
            foreach (var key in _cacheKeys.Where(k => k.StartsWith(typeof(T).Name)))
            {
                _cache.Remove(key);
            }
            _cacheKeys.RemoveWhere(k => k.StartsWith(typeof(T).Name));
        }
    }
}
