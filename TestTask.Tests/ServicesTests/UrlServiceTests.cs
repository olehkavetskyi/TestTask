using Moq;
using TestTask.Interfaces;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.Tests.ServicesTests;

public class UrlServiceTests
{
    private readonly Mock<IRepository<Url>> _repoMock;
    private readonly IUrlService _urlService;

    public UrlServiceTests()
    {
        _repoMock = new Mock<IRepository<Url>>();
        _urlService = new UrlService(_repoMock.Object);
    }

    [Fact]
    public async Task RemoveUrlAsync_WithExistingId_RemovesUrl()
    {
        // Arrange
        int id = 1;
        var url = new Url { Id = id };
        _repoMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(url);

        // Act
        await _urlService.RemoveUrlAsync(id);

        // Assert
        _repoMock.Verify(repo => repo.DeleteAsync(url), Times.Once);
    }

    [Fact]
    public async Task RemoveUrlAsync_WithNonExistingId_DoesNothing()
    {
        // Arrange
        int id = 1;
        _repoMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(null as Url);

        // Act
        await _urlService.RemoveUrlAsync(id);

        // Assert
        _repoMock.Verify(repo => repo.DeleteAsync(It.IsAny<Url>()), Times.Never);
    }

    [Fact]
    public async Task RemoveAllUrlsAsync_DeletesAllUrls()
    {
        // Arrange

        // Act
        await _urlService.RemoveAllUrlsAsync();

        // Assert
        _repoMock.Verify(repo => repo.DeleteAllAsync(), Times.Once);
    }
}