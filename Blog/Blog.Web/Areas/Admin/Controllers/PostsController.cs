namespace Blog.Web.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using Blog.Common.Infrastructure;
    using Blog.Models;
    using Blog.Services.Admin.Interfaces;
    using Blog.Web.Extensions;
    using Blog.Web.Helpers.Messages;
    using Microsoft.AspNetCore.Mvc;

    public class PostsController : AuthorController
    {
        private readonly IAdminPostsService postsService;

        public PostsController(IAdminPostsService postsService)
        {
            this.postsService = postsService;
        }      

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            bool postExists = await this.postsService.CheckIfPostExistsAsync(id);
            if (!postExists)
            {
                return this.ProcessNullEntity(nameof(Post));
            }

            var model = await this.postsService.GetPostForDeleteAsync(id);

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Destroy(int id)
        {
            bool postExists = await this.postsService.CheckIfPostExistsAsync(id);
            if (!postExists)
            {
                return this.ProcessNullEntity(nameof(Post));
            }

            bool succeeded = await this.postsService.DeletePostAsync(id);
            if (succeeded)
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Success,
                    Message = string.Format(Messages.EntityDeleteSuccess, nameof(Post), id)
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
                return this.RedirectToAction("Index", "Posts", new { Area = "Author" });
            }
            else
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = $"Unexpected error occurred deleting post with ID: {id}."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
                return this.RedirectToAction("Details", "Posts", new { id = id, Area = "Author" });
            }
        }
    }
}