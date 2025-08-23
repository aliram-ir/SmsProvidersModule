using Core.Entities;

namespace Core.Interfaces
{
    public interface ISmsProviderSelector
    {
        Task<SmsProviderEntry?> GetDefaultActiveProviderAsync();
    }
}
