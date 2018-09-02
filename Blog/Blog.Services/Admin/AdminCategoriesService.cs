namespace Blog.Services.Admin
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Admin.ViewModels;
    using Blog.Common.Infrastructure.Validation;
    using Blog.Data;
    using Blog.Services.Admin.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class AdminCategoriesService : BaseEfService, IAdminCategoriesService
    {
        public AdminCategoriesService(BlogDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<CategoryConciseViewModel>> GetCategoriesAsync()
        {
            var categories = await this.DbContext.Categories.ToListAsync();
            var model = this.Mapper.Map<IEnumerable<CategoryConciseViewModel>>(categories);
            return model;
        }   

        public async Task<CategoryDetailsViewModel> GetCategoryDetailsAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var category = await this.DbContext.Categories
                               .Include(c => c.Posts)
                               .FirstOrDefaultAsync(c => c.Id == id);
            var model = this.Mapper.Map<CategoryDetailsViewModel>(category);
            return model;
        }

        public async Task<CategoryEditBindingModel> GetCategoryForEditAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var category = await this.DbContext.Categories.FindAsync(id);
            var model = this.Mapper.Map<CategoryEditBindingModel>(category);
            return model;
        }

        public async Task<int> EditCategoryAsync(CategoryEditBindingModel model)
        {
            Validator.ThrowIfNull(model);
            var categoryFromDb = await this.DbContext.Categories.FindAsync(model.Id);         
            categoryFromDb.Name = model.Name;
            categoryFromDb.Order = model.Order;
            categoryFromDb.Title = model.Title;
            categoryFromDb.Description = model.Description;
            this.DbContext.Categories.Update(categoryFromDb);
            await this.DbContext.SaveChangesAsync();
            return categoryFromDb.Id;
        }

        public async Task<string> CategoryWithSameOrderAsync(int categoryId, int order)
        {
            Validator.ThrowIfZeroOrNegative(categoryId, nameof(categoryId));            
            var category = await this.DbContext.Categories
                .FirstOrDefaultAsync(c => c.Id != categoryId && c.Order == order);
            return category?.Name;
        }

        public async Task<bool> CheckIfCategoryExistsAsync(int id)
        {
            return await this.DbContext.Categories.AnyAsync(c => c.Id == id);
        }
    }
}
