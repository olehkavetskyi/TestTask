using Xunit;
using Moq;
using TestTask.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

public class UrlShortenerTests
{
    [Fact]
    public void GenerateShortUrl_ReturnsCorrectLength()
    {
        // Arrange
        int shortUrlLength = 8;
        var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();

        // Act
        string shortUrl = UrlShortener.GenerateShortUrl(shortUrlLength);

        // Assert
        Assert.Equal(shortUrlLength, shortUrl.Length);
    }

    [Fact]
    public void GenerateShortUrl_OnlyContainsValidCharacters()
    {
        // Arrange
        int shortUrlLength = 8;
        string expectedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        // Act
        string shortUrl = UrlShortener.GenerateShortUrl(shortUrlLength);

        // Assert
        Assert.All(shortUrl, c => Assert.Contains(c, expectedCharacters));
    }

    [Fact]
    public void GenerateShortUrl_ReturnsUniqueUrls()
    {
        // Arrange
        int numberOfUrls = 100;
        var generatedUrls = new HashSet<string>();

        // Act
        for (int i = 0; i < numberOfUrls; i++)
        {
            string shortUrl = UrlShortener.GenerateShortUrl(8);
            generatedUrls.Add(shortUrl);
        }

        // Assert
        Assert.Equal(numberOfUrls, generatedUrls.Count);
    }
}
