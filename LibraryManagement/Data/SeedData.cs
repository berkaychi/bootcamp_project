using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging; // Added for ILogger
using System;
using System.Threading.Tasks;
using LibraryManagement.Data; // Assuming ApplicationUser is in this namespace

namespace LibraryManagement.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("LibraryManagement.Data.SeedData"); // Get logger with a category name

            string adminRoleName = "Admin";
            string adminUserName = "admin"; // This is the UserName, not the email
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin123!"; // Ensure this meets password requirements

            // 1. Create Admin Role if it doesn't exist
            if (await roleManager.FindByNameAsync(adminRoleName) == null)
            {
                logger.LogInformation("Role '{RoleName}' not found. Creating...", adminRoleName);
                IdentityResult roleResult = await roleManager.CreateAsync(new IdentityRole(adminRoleName));
                if (roleResult.Succeeded)
                {
                    logger.LogInformation("Role '{RoleName}' created successfully.", adminRoleName);
                }
                else
                {
                    foreach (var error in roleResult.Errors)
                    {
                        logger.LogError("Error creating role '{RoleName}': {Error}", adminRoleName, error.Description);
                    }
                }
            }
            else
            {
                logger.LogInformation("Role '{RoleName}' already exists.", adminRoleName);
            }

            // 2. Create Admin User if it doesn't exist
            if (await userManager.FindByNameAsync(adminUserName) == null) // Check by UserName
            {
                logger.LogInformation("User '{UserName}' not found. Creating...", adminUserName);
                ApplicationUser adminUser = new ApplicationUser
                {
                    UserName = adminUserName, // Use the defined adminUserName
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "Administrator"
                };
                IdentityResult userResult = await userManager.CreateAsync(adminUser, adminPassword);

                if (userResult.Succeeded)
                {
                    logger.LogInformation("User '{UserName}' created successfully with email '{Email}'.", adminUserName, adminEmail);
                    // 3. Assign Admin User to Admin Role
                    IdentityResult addToRoleResult = await userManager.AddToRoleAsync(adminUser, adminRoleName);
                    if (addToRoleResult.Succeeded)
                    {
                        logger.LogInformation("User '{UserName}' added to role '{RoleName}'.", adminUserName, adminRoleName);
                    }
                    else
                    {
                        foreach (var error in addToRoleResult.Errors)
                        {
                            logger.LogError("Error adding user '{UserName}' to role '{RoleName}': {Error}", adminUserName, adminRoleName, error.Description);
                        }
                    }
                }
                else
                {
                    logger.LogError("Error creating user '{UserName}'.", adminUserName);
                    foreach (var error in userResult.Errors)
                    {
                        logger.LogError("Creation Error for '{UserName}': {ErrorDescription}", adminUserName, error.Description);
                    }
                }
            }
            else
            {
                logger.LogInformation("User '{UserName}' (Email: '{Email}') already exists. Seeding skipped for user creation.", adminUserName, adminEmail);
            }
        }
    }
}