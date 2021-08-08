namespace ComicTracker.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using static ComicTracker.Common.GlobalConstants;
    using static ComicTracker.Common.UserConstants;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ComicTrackerDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var administratorRole = await roleManager.FindByNameAsync(AdministratorRoleName);

            await SeedUserAsync(userManager, "Admin1", administratorRole);
            await SeedUserAsync(userManager, "User1");
        }

        private static async Task SeedUserAsync(
            UserManager<ApplicationUser> userManager,
            string userName,
            ApplicationRole role = null)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName.ToLower() + "@nonmail.com",
                };

                var result = await userManager.CreateAsync(user, DefaultPassword);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }

                if (role != null)
                {
                    await userManager.AddToRoleAsync(user, AdministratorRoleName);
                }
            }
        }
    }
}
