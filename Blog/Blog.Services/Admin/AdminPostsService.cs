namespace Blog.Services.Admin
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Common.Admin.ViewModels;
    using Blog.Common.Infrastructure;
    using Blog.Common.Infrastructure.Validation;
    using Blog.Data;
    using Blog.Services.Admin.Interfaces;
    using Blog.Services.Author.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class AdminPostsService : BaseEfService, IAdminPostsService
    {
        private readonly IAuthorPostsService authorPostsService;
        public AdminPostsService(BlogDbContext dbContext, IMapper mapper, IAuthorPostsService authorPostsService)
            : base(dbContext, mapper)
        {
            this.authorPostsService = authorPostsService;
        }       

        public async Task<bool> CheckIfPostExistsAsync(int postId)
        {
            return await this.DbContext.Posts.AnyAsync(p => p.Id == postId);
        }        

        public async Task<PostDeleteViewModel> GetPostForDeleteAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var post = await this.DbContext.Posts
                           .Include(p => p.Author)
                           .Include(p => p.Category)
                           .Include(p => p.Tags)
                           .ThenInclude(t => t.Tag)
                           .Include(p => p.Comments)
                           .ThenInclude(c => c.Replies)
                           .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return null;
            }

            var model = this.Mapper.Map<PostDeleteViewModel>(post);
            model.Content = this.authorPostsService.PreparePostContentForDisplay(model.Content);
            return model;
        }   

        public async Task<bool> DeletePostAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var post = await this.DbContext.Posts
                           .Include(p => p.Tags)
                           .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return false;
            }

            var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{Constants.StaticFilesFolder}/{post.ImageUrl}");
            if (File.Exists(fullFilePath))
            {
                try
                {
                    File.Delete(fullFilePath);
                }
                catch
                {
                    return false;
                }
            }

            var tags = post.Tags.ToList();
            try
            {
                foreach (var tag in tags)
                {
                    post.Tags.Remove(tag);
                }

                this.DbContext.Posts.Remove(post);
                await this.DbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }     
    }
}
