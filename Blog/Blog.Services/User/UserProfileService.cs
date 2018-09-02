namespace Blog.Services.User
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Common.Home.ViewModels;
    using Blog.Common.Infrastructure;
    using Blog.Common.Infrastructure.Validation;
    using Blog.Data;
    using Blog.Models;
    using Blog.Services.User.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserProfileService : BaseEfService, IUserProfileService
    {
        private readonly UserManager<User> userManager;

        public UserProfileService(BlogDbContext dbContext, IMapper mapper,  UserManager<User> userManager)
            : base(dbContext, mapper)
        {
            this.userManager = userManager;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(userId, nameof(userId));
            return await this.DbContext.Users.FindAsync(userId);
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(userName, nameof(userName));
            var user = await this.DbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            return user.Id;
        }

        public async Task<bool> UploadFileInFileSystemAsync(IFormFile file, string userId)
        {
            Validator.ThrowIfNull(file);
            Validator.ThrowIfNullEmptyOrWhiteSpace(userId, nameof(userId));

            var fileName = "avatar-" + userId + Constants.Jpg;
            var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{Constants.StaticFilesFolder}/{Constants.UsersImgFolder}", fileName);
            var fileStream = new FileStream(fullFilePath, FileMode.Create);
            using (fileStream)
            {
                await file.CopyToAsync(fileStream);
            }

            var user = await this.DbContext.Users.FindAsync(userId);
            user.AvatarUrl = $"/{Constants.UsersImgFolder}/" + fileName;
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

        public async Task<bool> SetNameAsync(string userId, string name)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(userId, nameof(userId));
            Validator.ThrowIfNullEmptyOrWhiteSpace(name, nameof(name));
            var user = await this.DbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.FullName = name;
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

        public async Task<bool> SetFacebookProfileUrlAsync(string userId, string facebookProfileUrl)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(userId, nameof(userId));
            Validator.ThrowIfNullEmptyOrWhiteSpace(facebookProfileUrl, nameof(facebookProfileUrl));
            var user = await this.DbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.FacebookProfileUrl = facebookProfileUrl;
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

        public async Task<bool> SetTwitterProfileUrlAsync(string userId, string twitterProfileUrl)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(userId, nameof(userId));
            Validator.ThrowIfNullEmptyOrWhiteSpace(twitterProfileUrl, nameof(twitterProfileUrl));
            var user = await this.DbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.TwitterProfileUrl = twitterProfileUrl;
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

        public async Task<bool> SetInstagramProfileUrlAsync(string userId, string instagramProfileUrl)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(userId, nameof(userId));
            Validator.ThrowIfNullEmptyOrWhiteSpace(instagramProfileUrl, nameof(instagramProfileUrl));
            var user = await this.DbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.InstagramProfileUrl = instagramProfileUrl;
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

        public async Task<IEnumerable<TeamMemberConciseViewModel>> GetTeamMembersAsync()
        {
            var adminsFromDb = await this.userManager.GetUsersInRoleAsync(Constants.Administrator);
            var admins = this.Mapper.Map<IEnumerable<TeamMemberConciseViewModel>>(adminsFromDb).ToList();
            foreach (var admin in admins)
            {
                admin.Position = Constants.Administrator + "and" + Constants.Author;
            }

            var authorsFromDb = await this.userManager.GetUsersInRoleAsync(Constants.Author);
            var authors = this.Mapper.Map<IEnumerable<TeamMemberConciseViewModel>>(authorsFromDb).ToList();
            foreach (var author in authors)
            {
                author.Position = Constants.Author;
            }

            var teamMembers = Enumerable.Concat(admins, authors).ToList();
            return teamMembers;
        }
    }
}
