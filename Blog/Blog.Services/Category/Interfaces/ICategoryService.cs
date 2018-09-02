namespace Blog.Services.Category.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Common.Home.ViewModels;

    public interface ICategoryService
    {
        Task<IEnumerable<CategoryHomeConciseViewModel>> GetCategoriesForHomePageAsync();
    }
}