using System.Collections.Concurrent;

namespace Infrastructure.Cache
{
    public interface ICacheKeyTracker
    {
        void Track(string key);
        IEnumerable<string> KeysStartingWith(string prefix);
        void RemoveStartingWith(string prefix);
    }

    public class CacheKeyTracker : ICacheKeyTracker
    {
        private readonly ConcurrentDictionary<string, byte> _keys = new();

        public void Track(string key) => _keys.TryAdd(key, 0);

        public IEnumerable<string> KeysStartingWith(string prefix) =>
        _keys.Keys.Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

        public void RemoveStartingWith(string prefix)
        {
            var toRemove = _keys.Keys.Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (var k in toRemove)
                _keys.TryRemove(k, out _);
        }
    }
}
