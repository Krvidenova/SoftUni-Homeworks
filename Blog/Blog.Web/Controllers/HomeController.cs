namespace Blog.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Blog.Common.Home.ViewModels;
    using Blog.Common.Infrastructure;
    using Blog.Services.Category.Interfaces;
    using Blog.Web.Extensions;
    using Blog.Web.Helpers.Messages;
    using Blog.Web.Models;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly ICategoryPostsService postsService;
        private readonly ICategoryService categoryService;

        public HomeController(ICategoryPostsService postsService, ICategoryService categoryService)
        {
            this.postsService = postsService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetCategoriesForHomePageAsync();
            return this.View(categories);
        }

        public IActionResult About()
        {
            return this.View();
        }

        public IActionResult Contact()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult Search(string value)
        {
            return this.View(model: value);
        }

        [HttpPost]
        public async Task<IActionResult> PerformSearch(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = "Please enter a text to search."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);

                return this.RedirectToAction("Search");
            }

            var posts = await this.postsService.GetPostsBySearchTermAsync(searchTerm);
            var model = new SearchResultViewModel() { Posts = posts, SearchTerm = searchTerm };

            return this.PartialView("_SearchResult", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
