namespace Blog.Web.Areas.Author.ViewComponents
{
    using System.Threading.Tasks;
    using Blog.Services.Admin.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsByPostId : ViewComponent
    {
        private readonly IAdminCommentsService commentsService;

        public CommentsByPostId(IAdminCommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int postId)
        {
            var comments = await this.commentsService.GetCommentsByPostIdAsync(postId);

            return this.View(comments);
        }
    }
}
