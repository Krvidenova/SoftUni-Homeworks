namespace Blog.Services.Category
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Common.Author.ViewModels;
    using Blog.Common.Home.ViewModels;
    using Blog.Common.Infrastructure.Validation;
    using Blog.Data;
    using Blog.Services.Author.Interfaces;
    using Blog.Services.Category.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class CategoryPostsService : BaseEfService, ICategoryPostsService
    {
        private readonly IAuthorPostsService authorPostsService;

        public CategoryPostsService(BlogDbContext dbContext, IMapper mapper, IAuthorPostsService authorPostsService)
            : base(dbContext, mapper)
        {
            this.authorPostsService = authorPostsService;
        }

        public async Task<IEnumerable<PostConciseViewModel>> GetPostsAsync(string name)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(name, nameof(name));
            var category = await this.DbContext.Categories
                               .FirstOrDefaultAsync(c => string.Equals(c.Name, name, StringComparison.InvariantCultureIgnoreCase));
            var categoryId = category.Id;
            var posts = await this.DbContext.Posts
                            .Where(p => p.CategoryId == categoryId)
                            .Include(p => p.Category)
                            .Include(p => p.Comments)
                            .ThenInclude(c => c.Replies)
                            .OrderByDescending(p => p.CreationDate)
                            .ToListAsync();

            var viewModel = this.Mapper.Map<IEnumerable<PostConciseViewModel>>(posts);
            return viewModel;
        }

        public async Task<bool> CheckIfPostExistsAsync(int postId, string slug)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(slug, nameof(slug));
            return await this.DbContext.Posts
                       .AnyAsync(p => p.Id == postId && p.Slug.ToLowerInvariant() == slug.ToLowerInvariant());
        }

        public async Task<PostDetailsViewModel> GetPostDetailsAsync(int id)
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

            var model = this.Mapper.Map<PostDetailsViewModel>(post);
            model.Content = this.authorPostsService.PreparePostContentForDisplay(model.Content);
            return model;
        }

        public async Task IncrementPostViewCountAsync(int postId)
        {
            Validator.ThrowIfZeroOrNegative(postId, nameof(postId));
            var post = await this.DbContext.Posts.FindAsync(postId);
            post.ViewCounter++;
            await this.DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<PostConciseViewModel>> GetPostsHomePageAsync(string name)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(name, nameof(name));
            var category = await this.DbContext.Categories
                               .FirstOrDefaultAsync(c => string.Equals(c.Name, name, StringComparison.InvariantCultureIgnoreCase));
            var categoryId = category.Id;
            var posts = await this.DbContext.Posts
                            .Where(p => p.CategoryId == categoryId)
                            .Include(p => p.Category)
                            .Include(p => p.Comments)
                            .ThenInclude(c => c.Replies)
                            .OrderByDescending(p => p.CreationDate)
                            .Take(4)
                            .ToListAsync();

            var viewModel = this.Mapper.Map<IEnumerable<PostConciseViewModel>>(posts);
            return viewModel;
        }

        public async Task<IEnumerable<SearchPostConciseViewModel>> GetPostsBySearchTermAsync(string searchTerm)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(searchTerm, nameof(searchTerm));
            var posts = await this.DbContext.Posts
                            .Where(p => p.Title.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant())
                                        || p.Content.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()))
                            .Include(p => p.Category)
                            .Include(p => p.Comments)
                            .ThenInclude(c => c.Replies)
                            .OrderByDescending(p => p.CreationDate)
                            .ToListAsync();
            foreach (var post in posts)
            {
                if (post.Content.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()))
                {
                    var index = post.Content.IndexOf(searchTerm, StringComparison.InvariantCultureIgnoreCase);
                    if (index == 0)
                    {
                       continue; 
                    }
                    else
                    {
                        post.Content = string.Concat("...", post.Content.Substring(index));
                    }
                }
            }

            var viewModel = this.Mapper.Map<IEnumerable<SearchPostConciseViewModel>>(posts).ToList();
            foreach (var model in viewModel)
            {               
                model.Content = Regex.Replace(
                    model.Content,
                    $"({Regex.Escape(searchTerm)})",
                    match => $@"<strong class=""text-danger"">{match}</strong>",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }

            return viewModel; 
        }
    }
}
