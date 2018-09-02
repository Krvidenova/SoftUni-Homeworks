namespace Blog.Web.Areas.Admin.ViewComponents
{
    using System.Threading.Tasks;
    using Blog.Services.Admin.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class RepliesByApproval : ViewComponent
    {
        private readonly IAdminRepliesService repliesService;

        public RepliesByApproval(IAdminRepliesService repliesService)
        {
            this.repliesService = repliesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool isApproved)
        {
            var replies = await this.repliesService.GetRepliesByApprovalAsync(isApproved);

            return this.View(replies);
        }
    }
}