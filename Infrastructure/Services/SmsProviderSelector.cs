using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class SmsProviderSelector : ISmsProviderSelector
    {
        private readonly IGenericRepository<SmsProviderEntry> _repo;

        public SmsProviderSelector(IGenericRepository<SmsProviderEntry> repo)
        {
            _repo = repo;
        }

        public async Task<SmsProviderEntry?> GetDefaultActiveProviderAsync()
        {
            var all = await _repo.GetAllAsync();
            return all.FirstOrDefault(p => p.IsActive && p.IsDefault);
        }
    }
}
