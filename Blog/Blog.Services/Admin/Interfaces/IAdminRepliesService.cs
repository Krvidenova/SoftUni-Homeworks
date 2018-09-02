namespace Blog.Services.Admin.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Admin.ViewModels;

    public interface IAdminRepliesService
    {
        Task<IEnumerable<ReplyViewModel>> GetRepliesForTodayDateAsync();

        Task<IEnumerable<ReplyViewModel>> GetRepliesByApprovalAsync(bool isApproved);

        Task<IEnumerable<ReplyViewModel>> GetRepliesForPeriodAsync(ReplySearchBindingModel model);

        Task<bool> CheckIfReplyExistsAsync(int id);

        Task<bool> ApproveReplyAsync(int id);

        Task<ReplyViewModel> GetReplyForEditAsync(int id);

        Task<ReplyViewModel> GetReplyForDisplayAsync(int id);

        Task<bool> ModerateReplyAsync(int id);

        Task<bool> DeleteReplyAsync(int id);

        Task CreateReplyAsync(string userId, int commentId, string content);
    }
}