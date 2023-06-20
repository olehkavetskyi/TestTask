using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using TestTask.Dtos;
using TestTask.Enums;
using TestTask.Helpers;
using TestTask.Interfaces;
using TestTask.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TestTask.Data; 

public static class UrlShortenerDbContextSeed
{
    public static async Task SeedUsers(UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
    {
        if (userManager.Users.Any())
        {
            return;
        }

        var roles = new List<string> { "Admin", "Regular" };
        foreach (var roleName in roles)
        {
            try
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new ApplicationRole { Name = roleName };
                    try
                    {
                        await roleManager.CreateAsync(role);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while creating the user: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while creating the user: " + ex.Message);
            }
            
        }

        var jsonData = File.ReadAllText("Data/SeedData/users.json");
        var users = JsonConvert.DeserializeObject<List<UserSeedDto>>(jsonData);

        foreach (var userSeedData in users)
        {
            var existingUser = await userManager.FindByEmailAsync(userSeedData.Email);

            if (existingUser == null)
            {
                var user = new User
                {
                    UserName = userSeedData.Email,
                    Email = userSeedData.Email,
                    Role = userSeedData.Role
                };
                IdentityResult result = null!;
                try
                {
                    result = await userManager.CreateAsync(user, userSeedData.Password);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while creating the user: " + ex.Message);
                }

                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync(userSeedData.Role.ToString()))
                    {
                        await roleManager.CreateAsync(new ApplicationRole { Name = userSeedData.Role.ToString() });
                    }

                    await userManager.AddToRoleAsync(user, userSeedData.Role.ToString());
                }
            }
        }
    }

    public static async Task SeedUrls(IServiceProvider serviceProvider, int keyLength)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IRepository<Url>>();


        if (dbContext.GetAllAsync().Result.Any())
        {
            return;
        }

        var jsonData = File.ReadAllText("Data/SeedData/urls.json");
        
        var urlSeedDataList = JsonConvert.DeserializeObject<List<UrlSeedDto>>(jsonData);

        foreach (var urlSeedData in urlSeedDataList)
        {
            Url url = null!;
            try
            {
                url = new Url
                {
                    ShortUrl = UrlShortener.GenerateShortUrl(keyLength),
                    FullUrl = urlSeedData.FullUrl,
                    CreatedByUserId = urlSeedData.CreatedByUserId,
                    CreatedAt = urlSeedData.CreatedAt
                };await dbContext.AddAsync(url);
            }
            catch { }

            
        }
    }

    public static async Task SeedText(IRepository<ConfigurableText> repo, UserManager<User> userManager)
    {
        var adminRole = Roles.Admin.ToString();
        var adminUsers = await userManager.GetUsersInRoleAsync(adminRole);

        if (!adminUsers.Any())
        {
            Console.WriteLine("Admin user not found.");
            return;
        }
        var adminUser = adminUsers.First();
        
        if (!repo.GetAllAsync().Result.Any())
        {
            var text = new ConfigurableText
            {
                Text = "The algorithm for the GenerateShortUrl method in the UrlShortener class is " +
                "as follows:\r\n\r\nDefine a constant string Characters that represents the set of " +
                "characters from which the short URL will be generated. It includes lowercase and " +
                "uppercase letters of the English alphabet (a-z, A-Z) and digits (0-9).\r\n\r\nThe " +
                "method takes an integer parameter ShortUrlLength, which specifies the desired length" +
                " of the generated short URL.\r\n\r\nCreate a StringBuilder named key to store the " +
                "generated short URL.\r\n\r\nInstantiate a Random object named random to generate " +
                "random indices for selecting characters from the Characters string.\r\n\r\nEnter " +
                "a loop that iterates ShortUrlLength times, generating one character for each " +
                "position in the short URL.\r\n\r\nInside the loop, generate a random number " +
                "between 0 and the length of the Characters string (exclusive upper bound) using " +
                "random.Next(0, Characters.Length). This number represents the index of the character" +
                " to be selected.\r\n\r\nRetrieve the character at the generated index from the " +
                "Characters string and append it to the key StringBuilder using key.Append(Characters[index])." +
                "\r\n\r\nAfter completing the loop, convert the key StringBuilder to a string using " +
                "key.ToString().\r\n\r\nReturn the generated short URL string.",

                ModifiedDate = DateTime.UtcNow,
                ModifiedBy = adminUser
            };

            try
            {
                await repo.AddAsync(text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }


}
