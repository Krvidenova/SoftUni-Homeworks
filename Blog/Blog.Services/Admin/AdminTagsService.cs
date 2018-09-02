namespace Blog.Services.Admin
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Common.Infrastructure;
    using Blog.Common.Infrastructure.Validation;
    using Blog.Data;
    using Blog.Models;
    using Blog.Services.Admin.Interfaces;

    public class AdminTagsService : BaseEfService, IAdminTagsService
    {
        public AdminTagsService(BlogDbContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        public async Task<int> CreateTagAsync(string tagName)
        {
            Validator.ThrowIfNullEmptyOrWhiteSpace(tagName, nameof(tagName));
            var tag = new Tag()
            {
                Name = string.Concat(Constants.HashTag, tagName.ToLower())
            };
            await this.DbContext.Tags.AddAsync(tag);
            await this.DbContext.SaveChangesAsync();
            return tag.Id;
        }
    }
}
