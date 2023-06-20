using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TestTask.Models;

namespace TestTask.Data;

public class UrlShortenerDbContext : IdentityDbContext<User, ApplicationRole, Guid>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Url> Urls { get; set; }
    public DbSet<ConfigurableText> Texts { get; set; }

    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}
