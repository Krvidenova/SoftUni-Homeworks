namespace Blog.Web.ViewComponents
{
    using System.Threading.Tasks;
    using Blog.Services.User.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    public class TeamMembers : ViewComponent
    {
        private readonly IUserProfileService userProfileService;

        public TeamMembers(IUserProfileService userProfileService)
        {
            this.userProfileService = userProfileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var members = await this.userProfileService.GetTeamMembersAsync();
            return this.View(members);
        }
    }
}