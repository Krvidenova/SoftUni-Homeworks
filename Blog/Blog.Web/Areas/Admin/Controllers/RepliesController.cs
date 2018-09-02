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

    public class RepliesController : AuthorController
    {
        private readonly IAdminRepliesService repliesService;

        public RepliesController(IAdminRepliesService repliesService)
        {
            this.repliesService = repliesService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public IActionResult LoadTodayReplies()
        {
            return this.ViewComponent(nameof(RepliesForTodayDate), new { });
        }

        [HttpGet]
        public IActionResult LoadRepliesByApproval(bool isApproved)
        {
            return this.ViewComponent(nameof(RepliesByApproval), new { isApproved = isApproved });
        }

        [HttpPost]
        public async Task<IActionResult> Search(ReplySearchBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("_SearchResult", new List<ReplyViewModel>());
            }

            model = TSelfExtensions.TrimStringProperties(model);
            var modelView = await this.repliesService.GetRepliesForPeriodAsync(model);

            return this.PartialView("_SearchResult", modelView);
        }

        [HttpGet]
        public async Task<IActionResult> Approve(int id)
        {
            bool replyExists = await this.repliesService.CheckIfReplyExistsAsync(id);
            if (!replyExists)
            {
                return this.ProcessNullEntity(nameof(Reply));
            }

            bool succeeded = await this.repliesService.ApproveReplyAsync(id);
            if (succeeded)
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Success,
                    Message = string.Format(Messages.EntityApproveSuccess, nameof(Reply), id)
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }
            else
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = $"Unexpected error occurred approving reply with ID: {id}."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }

            return this.RedirectToAction("Index", "Replies", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool replyExists = await this.repliesService.CheckIfReplyExistsAsync(id);
            if (!replyExists)
            {
                return this.ProcessNullEntity(nameof(Reply));
            }

            var model = await this.repliesService.GetReplyForEditAsync(id);

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Moderate(int id)
        {
            bool replyExists = await this.repliesService.CheckIfReplyExistsAsync(id);
            if (!replyExists)
            {
                return this.ProcessNullEntity(nameof(Reply));
            }

            bool succeeded = await this.repliesService.ModerateReplyAsync(id);
            if (succeeded)
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Success,
                    Message = string.Format(Messages.EntityModerateSuccess, nameof(Reply), id)
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }
            else
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = $"Unexpected error occurred moderating reply with ID: {id}."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }

            return this.RedirectToAction("Index", "Replies", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            bool replyExists = await this.repliesService.CheckIfReplyExistsAsync(id);
            if (!replyExists)
            {
                return this.ProcessNullEntity(nameof(Reply));
            }

            var model = await this.repliesService.GetReplyForDisplayAsync(id);

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Destroy(int id)
        {
            bool replyExists = await this.repliesService.CheckIfReplyExistsAsync(id);
            if (!replyExists)
            {
                return this.ProcessNullEntity(nameof(Reply));
            }

            bool succeeded = await this.repliesService.DeleteReplyAsync(id);
            if (succeeded)
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Success,
                    Message = string.Format(Messages.EntityDeleteSuccess, nameof(Reply), id)
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
                return this.RedirectToAction("Index", "Replies", new { Area = "Admin" });
            }
            else
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = $"Unexpected error occurred deleting reply with ID: {id}."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
                return this.RedirectToAction("Delete", "Replies", new { id = id,  Area = "Admin" });
            }
        }
    }
}