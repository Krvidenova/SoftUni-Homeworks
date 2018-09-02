namespace Blog.Web.Areas.Identity.Pages.Account
{
    using Blog.Web.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [AllowAnonymous]
    [ForbidAuthorized]
    public class ResetPasswordConfirmationModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
