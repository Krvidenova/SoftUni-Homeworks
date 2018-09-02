namespace Blog.Services.Admin.Interfaces
{
    using System.Threading.Tasks;

    public interface IAdminTagsService
    {
        Task<int> CreateTagAsync(string tagName);
    }
}