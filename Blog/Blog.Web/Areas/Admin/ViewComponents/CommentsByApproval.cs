namespace Blog.Web.Areas.Admin.ViewComponents
{
    using System.Threading.Tasks;
    using Blog.Services.Admin.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsByApproval : ViewComponent
    {
        private readonly IAdminCommentsService commentsService;

        public CommentsByApproval(IAdminCommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool isApproved)
        {
            var comments = await this.commentsService.GetCommentsByApprovalAsync(isApproved);

            return this.View(comments);
        }
    }
}
