namespace Blog.Services.Author.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Common.Author.BindingModels;
    using Blog.Common.Author.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IAuthorPostsService
    {
        Task<PostSearchBindingModel> PreparePostSearchFormAsync();

        Task<PostCreateBindingModel> PreparePostCreateFormAsync();

        Task<List<SelectListItem>> GenerateCategoriesSelectListAsync();

        Task<List<SelectListItem>> GenerateAuthorsSelectListAsync();

        Task<int> CreatePostAsync(PostCreateBindingModel model, string userName);

        Task<bool> UploadFileInFileSystemAsync(IFormFile file, int postId);

        Task<bool> AddTagsToPostAsync(string tags, int postId);

        Task<bool> CheckIfPostExistsAsync(int postId);
        
        Task<PostDetailsViewModel> GetPostDetailsAsync(int postId);

        string PreparePostContentForDisplay(string content);

        Task<IEnumerable<PostConciseViewModel>> GetPostsAsync(PostSearchBindingModel model);

        Task<PostEditBindingModel> GetPostForEditAsync(int id);

        Task<bool> CheckIfIsAllowedToPerformAsync(string userName, int postId);

        Task<int> EditPostAsync(PostEditBindingModel model);
    }
}