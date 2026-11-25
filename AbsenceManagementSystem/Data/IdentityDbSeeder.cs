using AbsenceManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace AbsenceManagementSystem.Data
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedRolesAsync(
            RoleManager<IdentityRole> roleManager)
        {
            string[] roles =
            {
                "User",
                "Administrator",
                "Parent",
                "Teacher",
                "Director"
            };

            foreach (var role in roles)
            {
                if (await roleManager.RoleExistsAsync(role))
                {
                    continue;
                }

                var result = await roleManager.CreateAsync(
                    new IdentityRole(role));

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ",
                            result.Errors.Select(e => e.Description)));
                }
            }
        }

        public static async Task SeedDirectorAsync(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            var email = configuration["SeedDirector:Email"];
            var password = configuration["SeedDirector:Password"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var director = await userManager
                .FindByEmailAsync(email);

            if (director == null)
            {
                director = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var creationResult = await userManager
                    .CreateAsync(director, password);

                if (!creationResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ",
                            creationResult.Errors
                                .Select(e => e.Description)));
                }
            }

            string[] directorRoles =
            {
                "User",
                "Director",
                "Administrator"
            };

            foreach (var role in directorRoles)
            {
                if (await userManager
                    .IsInRoleAsync(director, role))
                {
                    continue;
                }

                var result = await userManager
                    .AddToRoleAsync(director, role);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ",
                            result.Errors
                                .Select(e => e.Description)));
                }
            }
        }
    }
}