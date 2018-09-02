namespace Blog.Web.Areas.Admin.Controllers
{
    using Blog.Common.Infrastructure;
    using Blog.Web.Extensions;
    using Blog.Web.Helpers.Messages;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    [Authorize(Roles = Constants.Administrator)]
    public abstract class AuthorController : Controller
    {
        protected virtual IActionResult ProcessNullEntity(string entityName)
        {
            var messageModel = new MessageModel()
            {
                Type = MessageType.Danger,
                Message = string.Format(Messages.EntityDoesNotExist, entityName)
            };

            TempDataExtensions.Put(this.TempData, Constants.TempDataKey, messageModel);
            return this.RedirectToAction("Index");
        }
    }
}