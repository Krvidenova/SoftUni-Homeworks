namespace Blog.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Admin.ViewModels;
    using Blog.Common.Infrastructure;
    using Blog.Models;
    using Blog.Services.Admin.Interfaces;
    using Blog.Web.Areas.Admin.ViewComponents;
    using Blog.Web.Extensions;
    using Blog.Web.Helpers.Messages;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsController : AuthorController
    {
        private readonly IAdminCommentsService commentsService;

        public CommentsController(IAdminCommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult LoadTodayComments()
        {
            return this.ViewComponent(nameof(CommentsForTodayDate), new { });
        }

        [HttpGet]
        public IActionResult LoadCommentsByApproval(bool isApproved)
        {
            return this.ViewComponent(nameof(CommentsByApproval), new { isApproved = isApproved });
        }

        [HttpPost]
        public async Task<IActionResult> Search(CommentSearchBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("_SearchResult", new List<CommentViewModel>());
            }

            model = TSelfExtensions.TrimStringProperties(model);
            var modelView = await this.commentsService.GetCommentsForPeriodAsync(model);

            return this.PartialView("_SearchResult", modelView);
        }

        [HttpGet]
        public async Task<IActionResult> Approve(int id)
        {
            bool commentExists = await this.commentsService.CheckIfCommentExistsAsync(id);
            if (!commentExists)
            {
                return this.ProcessNullEntity(nameof(Comment));
            }

            bool succeeded = await this.commentsService.ApproveCommentAsync(id);
            if (succeeded)
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Success,
                    Message = string.Format(Messages.EntityApproveSuccess, nameof(Comment), id)
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }
            else
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = $"Unexpected error occurred approving comment with ID: {id}."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }

            return this.RedirectToAction("Index", "Comments", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool commentExists = await this.commentsService.CheckIfCommentExistsAsync(id);
            if (!commentExists)
            {
                return this.ProcessNullEntity(nameof(Comment));
            }

            var model = await this.commentsService.GetCommentForEditAsync(id);

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Moderate(int id)
        {
            bool commentExists = await this.commentsService.CheckIfCommentExistsAsync(id);
            if (!commentExists)
            {
                return this.ProcessNullEntity(nameof(Comment));
            }

            bool succeeded = await this.commentsService.ModerateCommentAsync(id);
            if (succeeded)
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Success,
                    Message = string.Format(Messages.EntityModerateSuccess, nameof(Comment), id)
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }
            else
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = $"Unexpected error occurred moderating comment with ID: {id}."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }

            return this.RedirectToAction("Index", "Comments", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            bool commentExists = await this.commentsService.CheckIfCommentExistsAsync(id);
            if (!commentExists)
            {
                return this.ProcessNullEntity(nameof(Comment));
            }

            var model = await this.commentsService.GetCommentForDisplayAsync(id);

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Destroy(int id)
        {
            bool commentExists = await this.commentsService.CheckIfCommentExistsAsync(id);
            if (!commentExists)
            {
                return this.ProcessNullEntity(nameof(Comment));
            }

            bool succeeded = await this.commentsService.DeleteCommentAsync(id);
            if (succeeded)
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Success,
                    Message = string.Format(Messages.EntityDeleteSuccess, nameof(Comment), id)
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
                return this.RedirectToAction("Index", "Comments", new { Area = "Admin" });
            }
            else
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = $"Unexpected error occurred deleting comment with ID: {id}."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
                return this.RedirectToAction("Delete", "Comments", new { id = id, Area = "Admin" });
            }
        }
    }
}