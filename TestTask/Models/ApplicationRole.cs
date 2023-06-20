using Microsoft.AspNetCore.Identity;
using TestTask.Enums;

namespace TestTask.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public Roles Role { get; set; }
}

