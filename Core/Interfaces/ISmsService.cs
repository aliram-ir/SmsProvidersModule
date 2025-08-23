namespace Core.Interfaces
{
    public interface ISmsService
    {
        Task SendSmsAsync(string templateCode, string phoneNumber, Dictionary<string, string> values);
    }
}
