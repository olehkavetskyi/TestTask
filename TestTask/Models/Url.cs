namespace TestTask.Models;

public class Url
{
    public int Id { get; set; }
    public string ShortUrl { get; set; } = null!;
    public string FullUrl { get; set; } = null!;
    public string CreatedByUserId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
