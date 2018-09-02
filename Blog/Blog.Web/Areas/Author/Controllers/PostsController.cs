namespace Blog.Web.Areas.Author.Controllers
{
    using System.Threading.Tasks;
    using Blog.Common.Author.BindingModels;
    using Blog.Common.Infrastructure;
    using Blog.Models;
    using Blog.Services.Author.Interfaces;
    using Blog.Web.Areas.Author.ViewComponents;
    using Blog.Web.Extensions;
    using Blog.Web.Helpers.Messages;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : AuthorController
    {
        private readonly IAuthorPostsService postsService;

        public PostsController(IAuthorPostsService postsService)
        {
            this.postsService = postsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.postsService.PreparePostSearchFormAsync();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Search(PostSearchBindingModel model)
        {
            model = TSelfExtensions.TrimStringProperties(model);
            var modelView = await this.postsService.GetPostsAsync(model);

            return this.PartialView("_SearchResult", modelView);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await this.postsService.PreparePostCreateFormAsync();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostCreateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = await this.postsService.GenerateCategoriesSelectListAsync();
                return this.View(model);
            }

            model = TSelfExtensions.TrimStringProperties(model);
            int id = await this.postsService.CreatePostAsync(model, this.User.Identity.Name);
            var messageModel = new MessageModel()
            {
                Type = MessageType.Success,
                Message = string.Format(Messages.EntityCreateSuccess, nameof(Post), id)
            };
            TempDataExtensions.Put(this.TempData, Constants.TempDataKey, messageModel);

            return this.RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            bool postExists = await this.postsService.CheckIfPostExistsAsync(id);
            if (!postExists)
            {
                return this.ProcessNullEntity(nameof(Post));
            }

            var model = await this.postsService.GetPostDetailsAsync(id);

            return this.View(model);
        }

        [HttpGet]
        public IActionResult LoadComments(int postId)
        {
            return this.ViewComponent(nameof(CommentsByPostId), new { postId = postId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool postExists = await this.postsService.CheckIfPostExistsAsync(id);
            if (!postExists)
            {
                return this.ProcessNullEntity(nameof(Post));
            }

            bool canEditPost = await this.postsService.CheckIfIsAllowedToPerformAsync(this.User.Identity.Name, id);
            if (!canEditPost)
            {
                var messageModel = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = Messages.NotAllowedMsg
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, messageModel);
                return this.RedirectToAction("Index");
            }

            var model = await this.postsService.GetPostForEditAsync(id);           

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostEditBindingModel model)
        {
            bool canEditPost = await this.postsService.CheckIfIsAllowedToPerformAsync(this.User.Identity.Name, model.Id);
            if (!canEditPost)
            {
                var messageModel = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = Messages.NotAllowedMsg
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, messageModel);
                return this.RedirectToAction("Index");
            }

            if (!this.ModelState.IsValid)
            {
                model.Categories = await this.postsService.GenerateCategoriesSelectListAsync();
                return this.View(model);
            }

            model = TSelfExtensions.TrimStringProperties(model);
            int id = await this.postsService.EditPostAsync(model);
            var message = new MessageModel()
            {
                Type = MessageType.Success,
                Message = string.Format(Messages.EntityEditSuccess, nameof(Post), id)
            };
            TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);

            return this.RedirectToAction("Details", new { id = id });
        }
    }
}