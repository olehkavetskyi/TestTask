using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using TestTask.Controllers;
using TestTask.Interfaces;
using TestTask.Models;

namespace TestTask.Tests.ControllersTests;

public class HomeControllerTests
{
    private Mock<ILogger<HomeController>> _mockLogger;
    private Mock<IRepository<ConfigurableText>> _mockRepo;
    private Mock<UserManager<User>> _mockUserManager;
    private HomeController _homeController;

    public HomeControllerTests()
    {
        _mockLogger = new Mock<ILogger<HomeController>>();
        _mockRepo = new Mock<IRepository<ConfigurableText>>();
        _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        _homeController = new HomeController(_mockLogger.Object, _mockRepo.Object, _mockUserManager.Object);
    }

    [Fact]
    public async Task About_WithLatestText_ReturnsViewWithLatestText()
    {
        // Arrange
        var latestText = new ConfigurableText { Text = "Latest text", ModifiedDate = DateTime.UtcNow };

        _mockRepo.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<ConfigurableText> { latestText });

        // Act
        var result = await _homeController.About() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(latestText, result.Model);
    }

    [Fact]
    public async Task About_WithNoText_ReturnsViewWithoutModel()
    {
        // Arrange
        _mockRepo.Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<ConfigurableText>());

        // Act
        var result = await _homeController.About() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Model);
    }

    [Fact]
    public async Task About_WithValidModel_RedirectsToAboutAction()
    {
        // Arrange
        var newText = "New text";

        var user = new User();
        _mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);

        _mockRepo.Setup(x => x.AddAsync(It.IsAny<ConfigurableText>()))
            .Returns(Task.FromResult((object)null));

        // Act
        var result = await _homeController.About(newText) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("About", result.ActionName);
        Assert.Null(result.ControllerName);
    }


    [Fact]
    public void AngularEntryPoint_ReturnsView()
    {
        // Act
        var result = _homeController.AngularEntryPoint() as ViewResult;

        // Assert
        Assert.NotNull(result);
    }
}