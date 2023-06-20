using Microsoft.EntityFrameworkCore;
using TestTask.Helpers;
using TestTask.Interfaces;
using TestTask.Models;

namespace TestTask.Services;

public class UrlService : IUrlService
{
    private readonly IRepository<Url> _repo;

    public UrlService(IRepository<Url> repo)
    {
        _repo = repo;
    }

    public async Task RemoveUrlAsync(int id)
    {
        var url = await _repo.GetByIdAsync(id);
        if (url != null)
        {
            await _repo.DeleteAsync(url);
        }


    }

    public async Task RemoveAllUrlsAsync()
    {
        await _repo.DeleteAllAsync();
    }

    public async Task<Url> AddUrlAsync(string fullUrl, string createdByUserId)
    {
        if (_repo.Find(u => u.FullUrl == fullUrl).Any())
        {
            throw new Exception($"{fullUrl} already exists");
        }

        string key;

        do
        {
            key = UrlShortener.GenerateShortUrl(5);
        } while (_repo.Find(u => u.ShortUrl == key).Any());

        var url = new Url
        {
            FullUrl = fullUrl,
            CreatedAt = DateTime.UtcNow,
            ShortUrl = key,
            CreatedByUserId = createdByUserId
        };

        await _repo.AddAsync(url);

        return url;
    }

}
