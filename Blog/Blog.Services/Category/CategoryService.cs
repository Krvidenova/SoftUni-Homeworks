namespace Blog.Services.Category
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Common.Home.ViewModels;
    using Blog.Data;
    using Blog.Services.Category.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : BaseEfService, ICategoryService
    {
        public CategoryService(BlogDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<CategoryHomeConciseViewModel>> GetCategoriesForHomePageAsync()
        {
            var categories = await this.DbContext.Categories.OrderBy(c => c.Order).ToListAsync();
            var model = this.Mapper.Map<IEnumerable<CategoryHomeConciseViewModel>>(categories);
            return model;
        }
    }
}