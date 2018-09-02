namespace Blog.Web.Areas.Admin.ViewComponents
{
    using System.Threading.Tasks;
    using Blog.Services.Admin.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsForTodayDate : ViewComponent
    {
        private readonly IAdminCommentsService commentsService;

        public CommentsForTodayDate(IAdminCommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var comments = await this.commentsService.GetCommentsForTodayDateAsync();

            return this.View(comments);
        }
    }
}
