namespace Blog.Services.User.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Common.Home.ViewModels;
    using Blog.Models;
    using Microsoft.AspNetCore.Http;

    public interface IUserProfileService
    {
        Task<User> GetUserByIdAsync(string userId);

        Task<string> GetUserIdAsync(string userName); 

        Task<bool> UploadFileInFileSystemAsync(IFormFile file, string userId);

        Task<bool> SetNameAsync(string userId, string name);

        Task<bool> SetFacebookProfileUrlAsync(string userId, string facebookProfileUrl);

        Task<bool> SetTwitterProfileUrlAsync(string userId, string twitterProfileUrl);

        Task<bool> SetInstagramProfileUrlAsync(string userId, string instagramProfileUrl);

        Task<IEnumerable<TeamMemberConciseViewModel>> GetTeamMembersAsync();
    }
}