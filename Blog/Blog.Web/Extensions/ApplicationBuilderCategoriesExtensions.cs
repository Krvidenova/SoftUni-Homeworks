namespace Blog.Web.Extensions
{
    using System.Linq;
    using Blog.Common.Infrastructure;
    using Blog.Data;
    using Blog.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderCategoriesExtensions
    {
        private static readonly Category[] Categories =
        {
            new Category()
                {
                    Order = 1,
                    Name = Constants.Business,
                    Title = "Business News This Week",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
                },
            new Category()
                {
                    Order = 2,
                    Name = Constants.Technology,
                    Title = "Get the Latest Technology News",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
                },
            new Category()
                {
                    Order = 3,
                    Name = Constants.Lifestyle,
                    Title = "Hot Topics from LifeStyle Section",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
                }
        };

        public static async void SeedDatabase(this IApplicationBuilder app)
        {
            var serviceFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceFactory.CreateScope();
            using (scope)
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
                if (!dbContext.Categories.Any())
                {
                    await dbContext.Categories.AddRangeAsync(Categories);
                    await dbContext.SaveChangesAsync();
                }                
            }
        }
    }
}
