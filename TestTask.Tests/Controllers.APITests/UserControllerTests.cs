using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using TestTask.Controllers.API;
using TestTask.Enums;
using TestTask.Models;

namespace TestTask.Tests.Controllers.APITests;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly Mock<UserManager<User>> _userManager;

    public UserControllerTests()
    {
        _userManager = MockUserManager<User>();
        _controller = new UserController(_userManager.Object);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            }
        };
    }


    [Fact]
    public async Task GetCurrentUser_WithInvalidUser_ReturnsNotFound()
    {
        // Arrange
        _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(null as User);

        // Act
        var result = await _controller.GetCurrentUser();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
    {
        var userStore = new Mock<IUserStore<TUser>>();
        return new Mock<UserManager<TUser>>(userStore.Object, null, null, null, null, null, null, null, null);
    }
}
