namespace Blog.Services.Category.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Common.Author.ViewModels;
    using Blog.Common.Home.ViewModels;

    public interface ICategoryPostsService
    {
        Task<IEnumerable<PostConciseViewModel>> GetPostsAsync(string name);

        Task<bool> CheckIfPostExistsAsync(int postId, string slug);
        
        Task<PostDetailsViewModel> GetPostDetailsAsync(int postId);

        Task IncrementPostViewCountAsync(int postId);

        Task<IEnumerable<PostConciseViewModel>> GetPostsHomePageAsync(string name);

        Task<IEnumerable<SearchPostConciseViewModel>> GetPostsBySearchTermAsync(string searchTerm);
    }
}