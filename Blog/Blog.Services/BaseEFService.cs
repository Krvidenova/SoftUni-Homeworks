namespace Blog.Services
{
    using AutoMapper;
    using Blog.Data;

    public abstract class BaseEfService
    {
        protected BaseEfService(BlogDbContext dbContext, IMapper mapper)
        {
            this.DbContext = dbContext;
            this.Mapper = mapper;
        }

        protected BlogDbContext DbContext { get; private set; }

        protected IMapper Mapper { get; private set; }
    }
}
