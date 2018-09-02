namespace Blog.Services.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Admin.ViewModels;
    using Blog.Common.Infrastructure;
    using Blog.Common.Infrastructure.Validation;
    using Blog.Data;
    using Blog.Models;
    using Blog.Services.Admin.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class AdminCommentsService : BaseEfService, IAdminCommentsService
    {
        public AdminCommentsService(BlogDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<CommentViewModel>> GetCommentsByPostIdAsync(int postId)
        {
            Validator.ThrowIfZeroOrNegative(postId, nameof(postId));
            var comments = await this.DbContext.Comments
                               .Include(c => c.Author)
                               .Include(c => c.Replies)
                               .ThenInclude(r => r.Author)
                               .Where(c => c.PostId == postId)
                               .OrderByDescending(c => c.CreationDate)
                               .ToListAsync();
            var model = this.Mapper.Map<IEnumerable<CommentViewModel>>(comments);
            return model;
        }

        public async Task<IEnumerable<CommentViewModel>> GetCommentsForTodayDateAsync()
        {
            var comments = await this.DbContext.Comments
                               .Include(c => c.Author)
                               .Include(c => c.Replies)
                               .ThenInclude(r => r.Author)
                               .Where(c => c.CreationDate.Date == DateTime.Today.Date)
                               .OrderByDescending(c => c.CreationDate)
                               .ToListAsync();
            var model = this.Mapper.Map<IEnumerable<CommentViewModel>>(comments);
            return model;
        }

        public async Task<IEnumerable<CommentViewModel>> GetCommentsForPeriodAsync(CommentSearchBindingModel model)
        {
            Validator.ThrowIfNull(model);
            var comments = await this.DbContext.Comments
                               .Where(
                               c => c.CreationDate.Date >= model.FromCreationDate.Date
                                    && c.CreationDate.Date <= model.ToCreationDate.Date)
                               .Include(c => c.Author)
                               .Include(c => c.Replies)
                               .ThenInclude(r => r.Author)
                               .OrderByDescending(c => c.CreationDate)
                               .ToListAsync();
            var viewModel = this.Mapper.Map<IEnumerable<CommentViewModel>>(comments);
            return viewModel;
        }

        public async Task<bool> CheckIfCommentExistsAsync(int commentId)
        {
            return await this.DbContext.Comments.AnyAsync(p => p.Id == commentId);
        }

        public async Task<CommentViewModel> GetCommentForEditAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var comment = await this.DbContext.Comments
                              .Include(c => c.Author)
                              .Include(c => c.Replies)
                              .ThenInclude(r => r.Author)
                              .FirstOrDefaultAsync(c => c.Id == id);
            var model = this.Mapper.Map<CommentViewModel>(comment);
            model.Content = Messages.CensoredComment;
            return model;
        }

        public async Task<CommentViewModel> GetCommentForDisplayAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var comment = await this.DbContext.Comments
                               .Include(c => c.Author)
                               .Include(c => c.Replies)
                               .ThenInclude(r => r.Author)
                               .FirstOrDefaultAsync(c => c.Id == id);
            var model = this.Mapper.Map<CommentViewModel>(comment);
            return model;
        }

        public async Task<IEnumerable<CommentViewModel>> GetCommentsByApprovalAsync(bool isApproved)
        {
            var comments = await this.DbContext.Comments
                               .Include(c => c.Author)
                               .Include(c => c.Replies)
                               .ThenInclude(r => r.Author)
                               .Where(c => c.IsApproved == isApproved)
                               .OrderByDescending(c => c.CreationDate)
                               .ToListAsync();
            var model = this.Mapper.Map<IEnumerable<CommentViewModel>>(comments);
            return model;
        }

        public async Task<bool> ApproveCommentAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var comment = await this.DbContext.Comments.FindAsync(id);
            if (comment == null)
            {
                return false;
            }

            comment.IsApproved = true;
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

        public async Task<bool> ModerateCommentAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var comment = await this.DbContext.Comments.FindAsync(id);
            if (comment == null)
            {
                return false;
            }

            var user = await this.DbContext.Users
                           .Where(u => u.Comments.Any(c => c.Id == id))
                           .FirstOrDefaultAsync();
            user.CensoredCommentsCount++;
            comment.Content = Messages.CensoredComment;
            comment.IsCensored = true;
            comment.IsApproved = true;
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

        public async Task<bool> DeleteCommentAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var comment = await this.DbContext.Comments.FindAsync(id);
            if (comment == null)
            {
                return false;
            }

            var user = await this.DbContext.Users
                           .Where(u => u.Comments.Any(c => c.Id == id))
                           .FirstOrDefaultAsync();
            user.DeletedCommentsCount++;

            try
            {
                this.DbContext.Comments.Remove(comment);
                await this.DbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task CreateCommentAsync(string authorId, int postId, string content)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(content, nameof(content));
            Validator.ThrowIfNullEmptyOrWhiteSpace(authorId, nameof(authorId));
            Validator.ThrowIfZeroOrNegative(postId, nameof(postId));
            var comment = new Comment()
            {
                AuthorId = authorId,
                PostId = postId,
                Content = content
            };
            await this.DbContext.Comments.AddAsync(comment);
            await this.DbContext.SaveChangesAsync();
        }
    }
}