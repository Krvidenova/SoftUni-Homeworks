namespace Blog.Web.Areas.Admin.ViewComponents
{
    using System.Threading.Tasks;
    using Blog.Services.Admin.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class RepliesForTodayDate : ViewComponent
    {
        private readonly IAdminRepliesService repliesService;

        public RepliesForTodayDate(IAdminRepliesService repliesService)
        {
            this.repliesService = repliesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var replies = await this.repliesService.GetRepliesForTodayDateAsync();
            return this.View(replies);
        }
    }
}
