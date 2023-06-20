using Microsoft.Extensions.Hosting.Internal;
using System.Text;

namespace TestTask.Helpers;

public static class UrlShortener
{
    private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string GenerateShortUrl(int ShortUrlLength)
    {
        var key = new StringBuilder();
        var random = new Random();

        for (int i = 0; i < ShortUrlLength; i++)
        {
            int index = random.Next(0, Characters.Length);
            key.Append(Characters[index]);
        }
        
        return key.ToString();
    }
}