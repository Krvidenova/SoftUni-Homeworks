namespace Blog.Web.Extensions
{
    using Blog.Common.Infrastructure;
    using Blog.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderAuthExtensions
    {
        private static readonly IdentityRole[] Roles =
        {
            new IdentityRole(Constants.Administrator),
            new IdentityRole(Constants.Author)
        };

        public static async void SeedDatabase(this IApplicationBuilder app)
        {
            var serviceFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceFactory.CreateScope();
            using (scope)
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                foreach (var role in Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        await roleManager.CreateAsync(role);
                    }
                }

                var admin = await userManager.FindByNameAsync("admin");
                if (admin == null)
                {
                    admin = new User()
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        AvatarUrl = Constants.DefaultAvatarPath
                    };

                    await userManager.CreateAsync(admin, Constants.DefaultAdminPassword);
                    await userManager.AddToRoleAsync(admin, Roles[0].Name);
                }

                var author = await userManager.FindByNameAsync("author");
                if (author == null)
                {
                    author = new User()
                    {
                        UserName = "author",
                        Email = "author@example.com",
                        AvatarUrl = Constants.DefaultAvatarPath
                    };

                    await userManager.CreateAsync(author, Constants.DefaultLecturerPassword);
                    await userManager.AddToRoleAsync(author, Roles[1].Name);
                }
            }
        }
    }
}
