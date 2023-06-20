using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TestTask.Data;
using TestTask.Helpers;
using TestTask.Interfaces;
using TestTask.Models;

namespace TestTask.Controllers.API;

[ApiController]
[Route("api/[controller]")]
public class UrlController : ControllerBase
{
    private readonly IRepository<Url> _repo;
    private readonly UserManager<User> _userManager;
    private readonly IUrlService _urlService;
    public UrlController(IRepository<Url> repo, UserManager<User> userManager, IUrlService urlService)
    {
        _repo = repo;
        _userManager = userManager;
        _urlService = urlService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Url>>> GetAllUrls()
    {
        var urls = await _repo.GetAllAsync();

        return Ok(urls);
    }


    [HttpGet("by-id/{id}")]
    public async Task<ActionResult<Url>> GetUrl(int id)
    {
        var url = await _repo.GetByIdAsync(id);

        if (url == null)
        {
            return NotFound();
        }

        return url;
    }

    [HttpGet("{shortUrl}")]
    public ActionResult<Url> GetUrlByShortUrl(string shortUrl)
    {

        var url = _repo.Find(u => u.ShortUrl == shortUrl).FirstOrDefault();

        if (url == null)
        {
            return NotFound();
        }

        return Ok(url);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveUrl(int id)
    {
        if ((_repo.GetByIdAsync(id).Result.CreatedByUserId == _userManager.GetUserAsync(User).Result.Id.ToString())
            && _userManager.GetUserAsync(User).Result.Role != Enums.Roles.Admin)
            return NoContent();

        await _urlService.RemoveUrlAsync(id);

        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveAllUrls()
    {
        await _urlService.RemoveAllUrlsAsync();

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Url>> AddUrl([FromBody] dynamic requestBody)
    {
        if (requestBody.TryGetProperty("fullUrl", out JsonElement fullUrlElement) && fullUrlElement.ValueKind == JsonValueKind.String)
        {
            string fullUrl = fullUrlElement.GetString();
            string createdByUserId = _userManager.GetUserId(User);

            var url = await _urlService.AddUrlAsync(fullUrl, createdByUserId);

            return Ok(url);
        }

        return BadRequest("Invalid request body");
    }

}
