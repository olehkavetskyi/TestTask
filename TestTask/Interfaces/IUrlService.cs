using TestTask.Models;

namespace TestTask.Interfaces;

public interface IUrlService
{
    public Task RemoveUrlAsync(int id);

    public Task RemoveAllUrlsAsync();

    public Task<Url> AddUrlAsync(string fullUrl, string createdByUserId);
}
