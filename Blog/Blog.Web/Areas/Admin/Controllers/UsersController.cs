namespace Blog.Web.Areas.Admin.Controllers
{
    using System.Threading.Tasks;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Infrastructure;
    using Blog.Services.Admin.Interfaces;
    using Blog.Services.User.Interfaces;
    using Blog.Web.Extensions;
    using Blog.Web.Helpers.Messages;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : AuthorController
    {
        private readonly IAdminUsersService usersService;
        private readonly IUserProfileService userProfileService;

        public UsersController(IAdminUsersService usersService, IUserProfileService userProfileService)
        {
            this.usersService = usersService;
            this.userProfileService = userProfileService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await this.usersService.GetUsersExceptLoggedInAsync(this.User);
            return this.View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            bool userExists = await this.usersService.CheckIfUserExistsAsync(id);
            if (!userExists)
            {
                return this.ProcessNullEntity(nameof(Blog.Models.User));
            }

            var currentUserId = await this.userProfileService.GetUserIdAsync(this.User.Identity.Name);
            if (id == currentUserId)
            {
                var messageModel = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = Messages.NotAllowedMsg
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, messageModel);
                return this.RedirectToAction("Index", "Users", new { Area = "Admin" });
            }

            var model = await this.usersService.GetUserDetailsAsync(id);
            return this.View(model);
        }

        public async Task<IActionResult> MakeAuthor(string id)
        {
            bool userExists = await this.usersService.CheckIfUserExistsAsync(id);
            if (!userExists)
            {
                return this.ProcessNullEntity(nameof(Blog.Models.User));
            }

            var currentUserId = await this.userProfileService.GetUserIdAsync(this.User.Identity.Name);
            if (id == currentUserId)
            {
                var messageModel = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = Messages.NotAllowedMsg
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, messageModel);
                return this.RedirectToAction("Index", "Users", new { Area = "Admin" });
            }

            bool succeeded = await this.usersService.MakeAuthorAsync(id);
            var username = await this.usersService.GetUsernameByUserIdAsync(id);
            if (succeeded)
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Success,
                    Message = $"The user with Username: {username} was assigned with role \"{Constants.Author}\" successfully."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }
            else
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = $"Unexpected error occurred assigning role \"{Constants.Author}\" to User with Username: {username}."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }

            return this.RedirectToAction("Details", "Users", new { id = id, Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            bool userExists = await this.usersService.CheckIfUserExistsAsync(id);
            if (!userExists)
            {
                return this.ProcessNullEntity(nameof(Blog.Models.User));
            }

            var currentUserId = await this.userProfileService.GetUserIdAsync(this.User.Identity.Name);
            if (id == currentUserId)
            {
                var messageModel = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = Messages.NotAllowedMsg
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, messageModel);
                return this.RedirectToAction("Index", "Users", new { Area = "Admin" });
            }

            var username = await this.usersService.GetUsernameByUserIdAsync(id);
            var model = new UserChangePasswordBindingModel() { Id = id, Username = username };
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserChangePasswordBindingModel model)
        {
            bool userExists = await this.usersService.CheckIfUserExistsAsync(model.Id);
            if (!userExists)
            {
                return this.ProcessNullEntity(nameof(Blog.Models.User));
            }

            var currentUserId = await this.userProfileService.GetUserIdAsync(this.User.Identity.Name);
            if (model.Id == currentUserId)
            {
                var messageModel = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = Messages.NotAllowedMsg
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, messageModel);
                return this.RedirectToAction("Index", "Users", new { Area = "Admin" });
            }

            if (!this.ModelState.IsValid)
            {
                var viewModel = new UserChangePasswordBindingModel() { Id = model.Id, Username = model.Username };
                return this.View(viewModel);
            }

            model = TSelfExtensions.TrimStringProperties(model);
            bool succeeded = await this.usersService.ChangePasswordAsync(model);
            if (succeeded)
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Success,
                    Message = $"The password of user with Username: {model.Username} has been changed successfully."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }
            else
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = $"Unexpected error occurred changing password of User with Username: {model.Username}."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }

            return this.RedirectToAction("Details", "Users", new { id = model.Id, Area = "Admin" });
        }

        public async Task<IActionResult> Ban(string id)
        {
            bool userExists = await this.usersService.CheckIfUserExistsAsync(id);
            if (!userExists)
            {
                return this.ProcessNullEntity(nameof(Blog.Models.User));
            }

            var currentUserId = await this.userProfileService.GetUserIdAsync(this.User.Identity.Name);
            if (id == currentUserId)
            {
                var messageModel = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = Messages.NotAllowedMsg
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, messageModel);
                return this.RedirectToAction("Index", "Users", new { Area = "Admin" });
            }

            var username = await this.usersService.GetUsernameByUserIdAsync(id);
            bool succeeded = await this.usersService.BanAsync(id);
            if (succeeded)
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Success,
                    Message = $"The user with Username: {username} was banned successfully."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }
            else
            {
                var message = new MessageModel()
                {
                    Type = MessageType.Warning,
                    Message = $"Unexpected error occurred banning User with Username: {username}."
                };
                TempDataExtensions.Put(this.TempData, Constants.TempDataKey, message);
            }

            return this.RedirectToAction("Details", "Users", new { id = id, Area = "Admin" });
        }
    }
}