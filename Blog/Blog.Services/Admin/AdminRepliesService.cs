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

    public class AdminRepliesService : BaseEfService, IAdminRepliesService
    {
        public AdminRepliesService(BlogDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<IEnumerable<ReplyViewModel>> GetRepliesForTodayDateAsync()
        {
            var replies = await this.DbContext.Replies
                               .Include(r => r.Author)
                               .Where(r => r.CreationDate.Date == DateTime.Today.Date)
                               .OrderByDescending(r => r.CreationDate)
                               .ToListAsync();
            var model = this.Mapper.Map<IEnumerable<ReplyViewModel>>(replies);
            return model;
        }

        public async Task<IEnumerable<ReplyViewModel>> GetRepliesByApprovalAsync(bool isApproved)
        {
            var replies = await this.DbContext.Replies
                               .Include(r => r.Author)
                               .Where(r => r.IsApproved == isApproved)
                               .OrderByDescending(r => r.CreationDate)
                               .ToListAsync();
            var model = this.Mapper.Map<IEnumerable<ReplyViewModel>>(replies);
            return model;
        }

        public async Task<IEnumerable<ReplyViewModel>> GetRepliesForPeriodAsync(ReplySearchBindingModel model)
        {
            Validator.ThrowIfNull(model);
            var replies = await this.DbContext.Replies
                               .Where(
                                   r => r.CreationDate.Date >= model.FromCreationDate.Date
                                        && r.CreationDate.Date <= model.ToCreationDate.Date)
                               .Include(r => r.Author)
                               .OrderByDescending(r => r.CreationDate)
                               .ToListAsync();
            var viewModel = this.Mapper.Map<IEnumerable<ReplyViewModel>>(replies);
            return viewModel;
        }

        public async Task<bool> CheckIfReplyExistsAsync(int id)
        {
            return await this.DbContext.Replies.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> ApproveReplyAsync(int id)
        {
            var reply = await this.DbContext.Replies.FindAsync(id);
            if (reply == null)
            {
                return false;
            }

            reply.IsApproved = true;
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

        public async Task<ReplyViewModel> GetReplyForEditAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var reply = await this.DbContext.Replies
                              .Include(r => r.Author)
                              .FirstOrDefaultAsync(r => r.Id == id);
            var model = this.Mapper.Map<ReplyViewModel>(reply);
            model.Content = Messages.CensoredComment;
            return model;
        }

        public async Task<bool> ModerateReplyAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var reply = await this.DbContext.Replies.FindAsync(id);
            if (reply == null)
            {
                return false;
            }

            var user = await this.DbContext.Users
                           .Where(u => u.Replies.Any(r => r.Id == id))
                           .FirstOrDefaultAsync();
            user.CensoredCommentsCount++;
            reply.Content = Messages.CensoredComment;
            reply.IsCensored = true;
            reply.IsApproved = true;
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

        public async Task<ReplyViewModel> GetReplyForDisplayAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var reply = await this.DbContext.Replies
                              .Include(r => r.Author)
                              .FirstOrDefaultAsync(r => r.Id == id);
            var model = this.Mapper.Map<ReplyViewModel>(reply);
            return model;
        }

        public async Task<bool> DeleteReplyAsync(int id)
        {
            Validator.ThrowIfZeroOrNegative(id, nameof(id));
            var reply = await this.DbContext.Replies.FindAsync(id);
            if (reply == null)
            {
                return false;
            }

            var user = await this.DbContext.Users
                           .Where(u => u.Replies.Any(r => r.Id == id))
                           .FirstOrDefaultAsync();
            user.DeletedCommentsCount++;

            try
            {
                this.DbContext.Replies.Remove(reply);
                await this.DbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task CreateReplyAsync(string userId, int commentId, string content)
        {
            Validator.ThrowIfZeroOrNegative(commentId, nameof(commentId));
            Validator.ThrowIfNullEmptyOrWhiteSpace(content, nameof(content));
            Validator.ThrowIfNullEmptyOrWhiteSpace(userId, nameof(userId));
            var reply = new Reply()
            {
                AuthorId = userId,
                CommentId = commentId,
                Content = content
            };
            await this.DbContext.Replies.AddAsync(reply);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
