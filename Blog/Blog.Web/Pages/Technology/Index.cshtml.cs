namespace Blog.Web.Pages.Technology
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Common.Author.ViewModels;
    using Blog.Common.Infrastructure;
    using Blog.Services.Category.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class IndexModel : PageModel
    {
        private readonly ICategoryPostsService postsService;

        public IndexModel(ICategoryPostsService postsService)
        {
            this.postsService = postsService;
            this.Posts = new List<PostConciseViewModel>();
            this.Tags = new List<string>();
        }

        public string Name { get; set; }

        public IEnumerable<PostConciseViewModel> Posts { get; set; }

        public IEnumerable<string> Tags { get; set; }
        
        public async Task<IActionResult> OnGet()
        {
            this.Name = Constants.Technology;
            this.Posts = await this.postsService.GetPostsAsync(Constants.Technology);
            return this.Page();
        }
    }
}