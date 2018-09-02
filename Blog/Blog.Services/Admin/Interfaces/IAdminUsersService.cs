namespace Blog.Services.Admin.Interfaces
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Admin.ViewModels;

    public interface IAdminUsersService
    {
        Task<IEnumerable<UserConciseViewModel>> GetUsersExceptLoggedInAsync(ClaimsPrincipal user);

        Task<bool> CheckIfUserExistsAsync(string id);

        Task<UserDetailsViewModel> GetUserDetailsAsync(string id);

        Task<string> GetUsernameByUserIdAsync(string id);

        Task<bool> MakeAuthorAsync(string id);

        Task<bool> BanAsync(string id);

        Task<bool> ChangePasswordAsync(UserChangePasswordBindingModel model);
    }
}