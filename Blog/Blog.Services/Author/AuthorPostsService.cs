namespace Blog.Services.Author
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Common.Author.BindingModels;
    using Blog.Common.Author.ViewModels;
    using Blog.Common.Infrastructure;
    using Blog.Common.Infrastructure.Validation;
    using Blog.Data;
    using Blog.Models;
    using Blog.Services.Admin.Interfaces;
    using Blog.Services.Author.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class AuthorPostsService : BaseEfService, IAuthorPostsService
    {
        private readonly IAdminTagsService tagService;

        public AuthorPostsService(BlogDbContext dbContext, IMapper mapper, IAdminTagsService tagsService)
            : base(dbContext, mapper)
        {
            this.tagService = tagsService;
        }

        public async Task<PostSearchBindingModel> PreparePostSearchFormAsync()
        {
            var authors = await this.GenerateAuthorsSelectListAsync();
            var categories = await this.GenerateCategoriesSelectListAsync();
            var model = new PostSearchBindingModel()
            {
                Categories = categories.OrderBy(c => c.Value),
                Authors = authors.OrderBy(a => a.Value)
            };
            return model;
        }

        public async Task<PostCreateBindingModel> PreparePostCreateFormAsync()
        {
            var categories = await this.GenerateCategoriesSelectListAsync();
            var model = new PostCreateBindingModel()
            {
                Categories = categories.OrderBy(c => c.Value)
            };
            return model;
        }

        public async Task<int> CreatePostAsync(PostCreateBindingModel model, string userName)
        {
            Validator.ThrowIfNull(model);
            Validator.ThrowIfNullEmptyOrWhiteSpace(userName, nameof(userName));
            var post = this.Mapper.Map<Post>(model);
            post.AuthorId = this.DbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName).Result.Id;
            await this.DbContext.Posts.AddAsync(post);
            await this.DbContext.SaveChangesAsync();
            if (model.Image != null)
            {
                bool uploadImageSucceeded = await this.UploadFileInFileSystemAsync(model.Image, post.Id);
                if (!uploadImageSucceeded)
                {
                    throw new InvalidOperationException(string.Format(Messages.UnexpectedError, nameof(model.Image), nameof(Post), post.Id));
                }
            }

            if (model.Tags != null)
            {
                bool addTagsToPostSucceeded = await this.AddTagsToPostAsync(model.Tags, post.Id);
                if (!addTagsToPostSucceeded)
                {
                    throw new InvalidOperationException(string.Format(Messages.UnexpectedError, nameof(model.Tags), nameof(Post), post.Id));
                }
            }

            return post.Id;
        }

        public async Task<bool> AddTagsToPostAsync(string tags, int postId)
        {
            Validator.ThrowIfZeroOrNegative(postId, nameof(postId));
            Validator.ThrowIfNullEmptyOrWhiteSpace(tags, nameof(tags));
            var tagNames = tags.Split(", ", StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .Select(t => t.ToLower())
                .Select(t => t.Trim())
                .ToList();
            var tagsFromDb = await this.DbContext.Tags.ToListAsync();
            var tagIds = new List<int>();
            foreach (var tagName in tagNames)
            {
                var tagId = tagsFromDb.FirstOrDefault(t => t.Name == string.Concat(Constants.HashTag, tagName))?.Id;
                if (tagId == null)
                {
                    tagId = await this.tagService.CreateTagAsync(tagName);
                    tagIds.Add(tagId.Value);
                }
                else
                {
                    tagIds.Add(tagId.Value);
                }
            }

            var post = await this.DbContext.Posts.FindAsync(postId);
            foreach (var tagId in tagIds)
            {
                post.Tags.Add(new PostTag()
                {
                    PostId = postId,
                    TagId = tagId
                });
            }

            return await this.DbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> CheckIfPostExistsAsync(int postId)
        {
            return await this.DbContext.Posts.AnyAsync(p => p.Id == postId);
        }

        public async Task<bool> UploadFileInFileSystemAsync(IFormFile file, int postId)
        {
            Validator.ThrowIfZeroOrNegative(postId, nameof(postId));
            Validator.ThrowIfNull(file);
            var fileName = postId + Constants.Jpg;
            var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{Constants.StaticFilesFolder}/{Constants.PostsImgFolder}", fileName);
            var fileStream = new FileStream(fullFilePath, FileMode.Create);
            using (fileStream)
            {
                await file.CopyToAsync(fileStream);
            }

            var post = await this.DbContext.Posts.FindAsync(postId);
            post.ImageUrl = $"/{Constants.PostsImgFolder}/{fileName}";
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
            model.Content = this.PreparePostContentForDisplay(model.Content);
            return model;
        }

        public async Task<IEnumerable<PostConciseViewModel>> GetPostsAsync(PostSearchBindingModel model)
        {
            Validator.ThrowIfNull(model);
            var posts = await this.DbContext.Posts
                            .Where(p =>
                                   (model.AuthorId == null || p.AuthorId == model.AuthorId)
                                && (model.CategoryId == null || p.CategoryId == model.CategoryId.Value)
                                && (model.PostId == null || p.Id == model.PostId.Value)
                                && (model.Title == null || p.Title == model.Title)
                                && (model.FromCreationDate == null || p.CreationDate.Date >= model.FromCreationDate.Value)
                                && (model.ToCreationDate == null || p.CreationDate.Date <= model.ToCreationDate.Value))
                            .Include(p => p.Comments)
                            .ThenInclude(c => c.Replies)
                            .OrderByDescending(p => p.CreationDate)
                            .ToListAsync();

            var viewModel = this.Mapper.Map<IEnumerable<PostConciseViewModel>>(posts);
            return viewModel;
        }

        public async Task<PostEditBindingModel> GetPostForEditAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var post = await this.DbContext.Posts
                           .Include(p => p.Tags)
                           .ThenInclude(t => t.Tag)
                           .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return null;
            }

            var model = this.Mapper.Map<PostEditBindingModel>(post);
            model.Categories = await this.GenerateCategoriesSelectListAsync();
            return model;
        }

        public async Task<bool> CheckIfIsAllowedToPerformAsync(string userName, int postId)
        {
            var user = await this.DbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            var userId = user.Id;
            var post = await this.DbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            var postAuthorId = post.AuthorId;
            return userId == postAuthorId;
        }

        public async Task<int> EditPostAsync(PostEditBindingModel model)
        {
            Validator.ThrowIfNull(model);
            var postFromDb = await this.DbContext.Posts
                                 .Include(p => p.Tags)
                                 .ThenInclude(t => t.Tag)
                                 .FirstOrDefaultAsync(p => p.Id == model.Id);
            postFromDb.Title = model.Headline;
            postFromDb.Slug = model.Slug;
            postFromDb.Content = model.Content;
            postFromDb.LastUpdateDate = model.LastUpdateDate;
            postFromDb.CategoryId = model.CategoryId;
            if (model.Tags != null)
            {
                var postTagsNames = postFromDb.Tags.Select(t => t.Tag.Name.Replace(Constants.HashTag, string.Empty)).ToList();
                var modelTagsNames = model.Tags
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
                    .Select(t => t.ToLower())
                    .Select(t => t.Trim())
                    .ToList();
                var tagsToAdd = modelTagsNames.Where(t => !postTagsNames.Contains(t)).ToList();
                var tagsToRemove = postFromDb.Tags
                    .Where(t => !modelTagsNames.Contains(t.Tag.Name.Replace(Constants.HashTag, string.Empty))).ToList();
                foreach (var tag in tagsToRemove)
                {
                    postFromDb.Tags.Remove(tag);
                }

                await this.DbContext.SaveChangesAsync();
                if (tagsToAdd.Count != 0)
                {
                    var tagsToAddAsString = string.Join(", ", tagsToAdd);
                    bool addTagsToPostSucceeded = await this.AddTagsToPostAsync(tagsToAddAsString, postFromDb.Id);
                    if (!addTagsToPostSucceeded)
                    {
                        throw new InvalidOperationException(string.Format(Messages.UnexpectedError, nameof(model.Tags), nameof(Post), postFromDb.Id));
                    }
                }
            }

            if (model.Image != null)
            {
                bool uploadImageSucceeded = await this.UploadFileInFileSystemAsync(model.Image, postFromDb.Id);
                if (!uploadImageSucceeded)
                {
                    throw new InvalidOperationException(string.Format(Messages.UnexpectedError, nameof(model.Image), nameof(Post), postFromDb.Id));
                }
            }

            return postFromDb.Id;
        }

    public async Task<List<SelectListItem>> GenerateAuthorsSelectListAsync()
        {
            var authorRoleId = this.DbContext.Roles
                            .Where(r => r.Name == Constants.Author || r.Name == Constants.Administrator)
                            .Select(r => r.Id)
                            .ToList();

            var userIdsWithAuthorRole = await this.DbContext.UserRoles
                                      .Where(ur => authorRoleId.Any(id => id == ur.RoleId))
                                      .Select(ur => ur.UserId)
                                      .ToListAsync();

            var authors = await this.DbContext.Users
                              .Where(u => userIdsWithAuthorRole.Contains(u.Id))
                              .Select(u => new SelectListItem() { Text = u.FullName ?? u.UserName, Value = u.Id.ToString() })
                              .ToListAsync();
            authors.Add(new SelectListItem() { Text = string.Empty, Value = string.Empty });
            return authors;
        }

        public async Task<List<SelectListItem>> GenerateCategoriesSelectListAsync()
        {
            var categories = await this.DbContext.Categories
                                 .Select(c => new SelectListItem() { Text = c.Name, Value = c.Id.ToString() })
                                 .ToListAsync();
            categories.Add(new SelectListItem() { Text = string.Empty, Value = string.Empty });
            return categories;
        }

        public string PreparePostContentForDisplay(string content)
        {
            var resultContent = new StringBuilder();
            content = content.Replace("STARTQUOTE", "<blockquote>");
            content = content.Replace("ENDQUOTE", "</blockquote>");
            var contentArr = content.Split('\n').ToArray();
            for (var i = 0; i < contentArr.Length; i++)
            {
                if (contentArr[i].Contains("<blockquote>"))
                {
                    if (contentArr[i].Contains("</blockquote>"))
                    {
                        resultContent.Append($"<p>{contentArr[i]}</p>");
                    }
                    else
                    {
                        var startQuoteIndex = i;
                        var endQuoteIndex = 0;
                        for (var n = i + 1; n < contentArr.Length; n++)
                        {
                            if (contentArr[n].Contains("</blockquote>"))
                            {
                                endQuoteIndex = n;
                            }
                        }

                        var quote = string.Empty;
                        for (var z = startQuoteIndex; z <= endQuoteIndex; z++)
                        {
                            quote = quote + contentArr[z];
                        }

                        resultContent.Append($"<p>{quote}</p>");
                        i = endQuoteIndex;
                    }                   
                }
                else
                {
                    resultContent.Append($"<p>{contentArr[i]}</p>");
                }
            }

            return resultContent.ToString();
        }
    }
}
