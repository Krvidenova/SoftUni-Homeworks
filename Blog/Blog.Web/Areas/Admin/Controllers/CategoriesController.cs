namespace Blog.Web.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Infrastructure;
    using Blog.Models;
    using Blog.Services.Admin.Interfaces;
    using Blog.Web.Extensions;
    using Blog.Web.Helpers.Messages;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : AuthorController
    {
        private readonly IAdminCategoriesService categoriesService;

        public CategoriesController(IAdminCategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await this.categoriesService.GetCategoriesAsync();
            return this.View(categories);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await this.categoriesService.GetCategoryDetailsAsync(id);
            if (model == null)
            {
                return this.ProcessNullEntity(nameof(Category));
            }

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await this.categoriesService.GetCategoryForEditAsync(id);
            if (model == null)
            {
                return this.ProcessNullEntity(nameof(Category));
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditBindingModel model)
        {
            bool categoryExists = await this.categoriesService.CheckIfCategoryExistsAsync(model.Id);
            if (!categoryExists)
            {
                return this.ProcessNullEntity(nameof(Category));
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            model = TSelfExtensions.TrimStringProperties(model);
            var categoryNameWithSameOrder = await this.categoriesService
                                                .CategoryWithSameOrderAsync(model.Id, model.Order);
            if (categoryNameWithSameOrder != null)
            {
                this.ModelState.AddModelError(
                    nameof(model.Order),
                    $"{nameof(Category)} \"{categoryNameWithSameOrder}\" already has order \"{model.Order}\".");
                return this.View(model);
            }

            int id = await this.categoriesService.EditCategoryAsync(model);
            var messageModel = new MessageModel()
            {
                Type = MessageType.Success,
                Message = string.Format(Messages.EntityEditSuccess, nameof(Category), id)
            };
            TempDataExtensions.Put(this.TempData, Constants.TempDataKey, messageModel);

            return this.RedirectToAction("Details", "Categories", new { id = id, Area = "Admin" });
        }
    }
}