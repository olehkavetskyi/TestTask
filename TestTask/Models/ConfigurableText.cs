namespace TestTask.Models;

public class ConfigurableText
{
    public int Id { get; set; }
    public string Text { get; set; }
    public User ModifiedBy { get; set; }
    public DateTime ModifiedDate { get; set; }
}