namespace Blog.Services.Admin.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Admin.ViewModels;

    public interface IAdminCommentsService
    {
        Task<IEnumerable<CommentViewModel>> GetCommentsByPostIdAsync(int postId);

        Task<IEnumerable<CommentViewModel>> GetCommentsForTodayDateAsync();

        Task<IEnumerable<CommentViewModel>> GetCommentsForPeriodAsync(CommentSearchBindingModel model);

        Task<bool> CheckIfCommentExistsAsync(int commentId);

        Task<CommentViewModel> GetCommentForEditAsync(int id);

        Task<CommentViewModel> GetCommentForDisplayAsync(int id);

        Task<IEnumerable<CommentViewModel>> GetCommentsByApprovalAsync(bool isApproved);

        Task<bool> ApproveCommentAsync(int id);

        Task<bool> ModerateCommentAsync(int id);

        Task<bool> DeleteCommentAsync(int id);

        Task CreateCommentAsync(string userId, int postId, string content);
    }
}