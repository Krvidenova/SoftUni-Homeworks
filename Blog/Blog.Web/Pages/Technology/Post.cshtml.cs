namespace Blog.Web.Pages.Technology
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Blog.Common.Author.ViewModels;
    using Blog.Common.Infrastructure;
    using Blog.Models;
    using Blog.Services.Admin.Interfaces;
    using Blog.Services.Category.Interfaces;
    using Blog.Services.User.Interfaces;
    using Blog.Web.Extensions;
    using Blog.Web.Helpers.Messages;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class PostModel : PageModel
    {
        private readonly ICategoryPostsService postsService;
        private readonly IAdminCommentsService commentsService;
        private readonly IAdminRepliesService repliesService;
        private readonly IUserProfileService userService;
        private readonly UserManager<User> userManager;

        public PostModel(
            ICategoryPostsService postsService,
            IAdminCommentsService commentsService,
            IAdminRepliesService repliesService,
            IUserProfileService userService,
            UserManager<User> userManager)
        {
            this.postsService = postsService;
            this.commentsService = commentsService;
            this.repliesService = repliesService;
            this.userService = userService;
            this.userManager = userManager;
        }

        [BindProperty]
        [Required]
        [StringLength(1000, MinimumLength = 2)]
        public string Message { get; set; }

        [BindProperty]
        public int PostId { get; set; }

        [BindProperty]
        public int CommentId { get; set; }

        [BindProperty]
        public string Slug { get; set; }

        public PostDetailsViewModel Post { get; set; }

        [HttpGet("/Technology/{id}/{slug}")]
        public async Task<IActionResult> OnGet(int id, string slug)
        {
            bool postExists = await this.postsService.CheckIfPostExistsAsync(id, slug);
            if (!postExists)
            {
                return this.NotFound();
            }

            this.Post = await this.postsService.GetPostDetailsAsync(id);
            await this.postsService.IncrementPostViewCountAsync(id);
            return this.Page();
        }

        public async Task<IActionResult> OnPost()
        {
            bool postExists = await this.postsService.CheckIfPostExistsAsync(this.PostId, this.Slug);
            if (!postExists)
            {
                return this.NotFound();
            }

            if (this.Message == null)
            {
                var msgModel = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = Messages.FillFormMsg
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, msgModel);
                return this.Redirect($"/{Constants.Technology}/{this.PostId}/{this.Slug}");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (user.IsBanned)
            {
                var msgModel = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = Messages.NotAllowedToComment
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, msgModel);
                return this.Redirect($"/{Constants.Technology}/{this.PostId}/{this.Slug}");
            }

            string userId = await this.userService.GetUserIdAsync(this.User.Identity.Name);
            bool commentExists = await this.commentsService.CheckIfCommentExistsAsync(this.CommentId);
            if (commentExists)
            {
                await this.repliesService.CreateReplyAsync(userId, this.CommentId, this.Message);
            }
            else
            {
                await this.commentsService.CreateCommentAsync(userId, this.PostId, this.Message);
            }

            var message = new MessageModel()
            {
                Type = MessageType.Success,
                Message = Messages.CommentSuccess
            };
            TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            return this.Redirect($"/{Constants.Technology}/{this.PostId}/{this.Slug}");
        }
    }
}