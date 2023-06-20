using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TestTask.Enums;

namespace TestTask.Models;

public class User : IdentityUser<Guid>
{
    public Roles Role { get; set; } = Roles.Regular;
}
