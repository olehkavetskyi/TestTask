using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestTask.Enums;
using TestTask.Interfaces;
using TestTask.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TestTask.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<ConfigurableText> _repo;
    private readonly UserManager<User> _userManager;

    public HomeController(ILogger<HomeController> logger, IRepository<ConfigurableText> repo, UserManager<User> userManager)
    {
        _logger = logger;
        _repo = repo;
        _userManager = userManager;
    }

    public async Task<IActionResult> About()
    {
        var texts = await _repo.GetAllAsync();

        var latestText = texts.OrderBy(x => x.ModifiedDate).LastOrDefault();   

        if (latestText != null)
        {
            return View(latestText);
        }
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> About(string newText)
    {
        var text = new ConfigurableText()
        {
            Text = newText,
            ModifiedDate = DateTime.UtcNow,
            ModifiedBy = await _userManager.GetUserAsync(User)
        };

        await _repo.AddAsync(text);

        return RedirectToAction("About");
    }

    public IActionResult AngularEntryPoint()
    {
        return View();
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
