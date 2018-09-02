namespace Blog.Web.ViewComponents
{
    using System.Threading.Tasks;
    using Blog.Services.Category.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class PostsByCategory : ViewComponent
    {
        private readonly ICategoryPostsService postsService;

        public PostsByCategory(ICategoryPostsService postsService)
        {
            this.postsService = postsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryName)
        {
            var posts = await this.postsService.GetPostsHomePageAsync(categoryName);

            return this.View(posts);
        }
    }
}
