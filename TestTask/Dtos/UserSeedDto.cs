using TestTask.Enums;

namespace TestTask.Dtos;

public class UserSeedDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public Roles Role { get; set; }
}
