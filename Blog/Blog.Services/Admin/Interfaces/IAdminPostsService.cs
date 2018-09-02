namespace Blog.Services.Admin.Interfaces
{
    using System.Threading.Tasks;
    using Blog.Common.Admin.ViewModels;

    public interface IAdminPostsService
    {
        Task<bool> CheckIfPostExistsAsync(int postId);

        Task<PostDeleteViewModel> GetPostForDeleteAsync(int id);

        Task<bool> DeletePostAsync(int id);
    }
}