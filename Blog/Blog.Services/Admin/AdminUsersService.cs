namespace Blog.Services.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Admin.ViewModels;
    using Blog.Common.Infrastructure;
    using Blog.Common.Infrastructure.Validation;
    using Blog.Data;
    using Blog.Models;
    using Blog.Services.Admin.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class AdminUsersService : BaseEfService, IAdminUsersService
    {
        private readonly UserManager<User> userManager;

        public AdminUsersService(BlogDbContext dbContext, IMapper mapper, UserManager<User> userManager)
            : base(dbContext, mapper)
        {
            this.userManager = userManager;
        }

        public async Task<IEnumerable<UserConciseViewModel>> GetUsersExceptLoggedInAsync(ClaimsPrincipal user)
        {
            Validator.ThrowIfNull(user, nameof(user));
            var currentUser = await this.userManager.GetUserAsync(user);
            var users = await this.DbContext.Users
                            .Where(u => u.Id != currentUser.Id)
                            .ToListAsync();
            var model = users.Select(u => new UserConciseViewModel()
            {
                Id = u.Id,
                Username = u.UserName, 
                Email = u.Email,
                IsBanned = u.IsBanned,
                IsAdmin = this.userManager.GetRolesAsync(u).Result.Contains(Constants.Administrator),
                IsAuthor = this.userManager.GetRolesAsync(u).Result.Contains(Constants.Author)
            }).ToList();

            return model;
        }

        public async Task<bool> CheckIfUserExistsAsync(string id)
        {         
            return await this.DbContext.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<UserDetailsViewModel> GetUserDetailsAsync(string id)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(id, nameof(id));
            var user = await this.DbContext.Users
                           .Include(u => u.Comments)
                           .Include(u => u.Replies)
                           .Include(u => u.Posts)
                           .FirstOrDefaultAsync(u => u.Id == id);

            var model = this.Mapper.Map<UserDetailsViewModel>(user);
            var roles = await this.userManager.GetRolesAsync(user);
            model.Roles = roles;
            return model;
        }

        public async Task<string> GetUsernameByUserIdAsync(string id)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(id, nameof(id));
            var user = await this.DbContext.Users.FindAsync(id);
            return user.UserName;
        }

        public async Task<bool> MakeAuthorAsync(string id)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(id, nameof(id));
            var user = await this.DbContext.Users.FindAsync(id);
            var result = await this.userManager.AddToRoleAsync(user, Constants.Author);
            return result.Succeeded;
        }

        public async Task<bool> BanAsync(string id)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(id, nameof(id));
            var user = await this.DbContext.Users.FindAsync(id);
            user.IsBanned = true;
            try
            {
                await this.DbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(UserChangePasswordBindingModel model)
        {
            Validator.ThrowIfNull(model);
            var user = await this.DbContext.Users.FindAsync(model.Id);
            var changePasswordResult = await this.userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return changePasswordResult.Succeeded;
        }
    }
}
