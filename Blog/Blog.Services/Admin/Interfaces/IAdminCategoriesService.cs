namespace Blog.Services.Admin.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Admin.ViewModels;

    public interface IAdminCategoriesService
    {
        Task<IEnumerable<CategoryConciseViewModel>> GetCategoriesAsync();       

        Task<CategoryDetailsViewModel> GetCategoryDetailsAsync(int id);

        Task<CategoryEditBindingModel> GetCategoryForEditAsync(int id);

        Task<int> EditCategoryAsync(CategoryEditBindingModel model);

        Task<string> CategoryWithSameOrderAsync(int categoryId, int order);

        Task<bool> CheckIfCategoryExistsAsync(int id);
    }
}