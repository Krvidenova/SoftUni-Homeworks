namespace Blog.Web.Mapping
{
    using System.Linq;
    using AutoMapper;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Admin.ViewModels;
    using Blog.Common.Author.BindingModels;
    using Blog.Common.Author.ViewModels;
    using Blog.Common.Home.ViewModels;
    using Blog.Common.Infrastructure;
    using Blog.Models;
    using Blog.Web.Extensions;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<Category, CategoryConciseViewModel>();

            this.CreateMap<Category, CategoryDetailsViewModel>()
                .ForMember(
                    viewModel => viewModel.PostsCount,
                    opt => opt.MapFrom(category => category.Posts.Count));

            this.CreateMap<CategoryEditBindingModel, Category>().ReverseMap();

            this.CreateMap<PostCreateBindingModel, Post>()
                .ForMember(
                    post => post.Title,
                    opt => opt.MapFrom(model => model.Headline))
                .ForMember(
                    post => post.ImageUrl,
                    opt => opt.Ignore())
                .ForMember(
                    post => post.Tags,
                    opt => opt.Ignore());

            this.CreateMap<Reply, ReplyViewModel>()
                .ForMember(
                    viewModel => viewModel.Author,
                    opt => opt.MapFrom(reply => reply.Author.FullName ?? reply.Author.UserName))
                .ForMember(
                    viewModel => viewModel.AuthorAvatarUrl,
                    opt => opt.MapFrom(reply => reply.Author.AvatarUrl))
                .ForMember(
                    viewModel => viewModel.CreationDate,
                    opt => opt.MapFrom(reply => string.Format("{0:dd MMM ,yyyy hh:mm tt}", reply.CreationDate)))
                .ForMember(
                    viewModel => viewModel.LastUpdateDate,
                    opt => opt.MapFrom(reply => string.Format("{0:dd MMM ,yyyy hh:mm tt}", reply.LastUpdateDate)));

            this.CreateMap<Comment, CommentViewModel>()
                .ForMember(
                    viewModel => viewModel.Author,
                    opt => opt.MapFrom(comment => comment.Author.FullName ?? comment.Author.UserName))
                .ForMember(
                    viewModel => viewModel.AuthorAvatarUrl,
                    opt => opt.MapFrom(reply => reply.Author.AvatarUrl))
                .ForMember(
                    viewModel => viewModel.CreationDate,
                    opt => opt.MapFrom(comment => string.Format("{0:dd MMM ,yyyy hh:mm tt}", comment.CreationDate)))
                .ForMember(
                    viewModel => viewModel.LastUpdateDate,
                    opt => opt.MapFrom(comment => string.Format("{0:dd MMM ,yyyy hh:mm tt}", comment.LastUpdateDate)));

            this.CreateMap<Post, PostDetailsViewModel>()
                .ForMember(
                    viewModel => viewModel.Author,
                    opt => opt.MapFrom(post => post.Author.FullName ?? post.Author.UserName))
                .ForMember(
                    viewModel => viewModel.Category,
                    opt => opt.MapFrom(post => post.Category.Name))
                .ForMember(
                    viewModel => viewModel.AuthorAvatarUrl,
                    opt => opt.MapFrom(post => post.Author.AvatarUrl))
                .ForMember(
                    viewModel => viewModel.CommentsCount,
                    opt => opt.MapFrom(post => (post.Comments.Count + post.Comments.Sum(c => c.Replies.Count))))
                .ForMember(
                    viewModel => viewModel.Tags,
                    opt => opt.MapFrom(post => post.Tags.Select(t => t.Tag.Name)))
                .ForMember(
                    viewModel => viewModel.CreationDate,
                    opt => opt.MapFrom(post => string.Format("{0:dd MMM ,yyyy hh:mm tt}", post.CreationDate)))
                .ForMember(
                    viewModel => viewModel.LastUpdateDate,
                    opt => opt.MapFrom(post => string.Format("{0:dd MMM ,yyyy hh:mm tt}", post.LastUpdateDate)));

            this.CreateMap<Post, PostConciseViewModel>()
                .ForMember(
                    viewModel => viewModel.CommentsCount,
                    opt => opt.MapFrom(post => (post.Comments.Count + post.Comments.Sum(c => c.Replies.Count))))
                .ForMember(
                    viewModel => viewModel.CreationDate,
                    opt => opt.MapFrom(post => string.Format("{0:dd MMM}", post.CreationDate)))
                .ForMember(
                    viewModel => viewModel.CreationYear,
                    opt => opt.MapFrom(post => post.CreationDate.Year.ToString()))
                .ForMember(
                    viewModel => viewModel.Content,
                    opt => opt.MapFrom(post => StringExtensions.TruncateAtWord(post.Content, 120)))
                .ForMember(
                    viewModel => viewModel.CategoryName,
                    opt => opt.MapFrom(post => post.Category.Name));

            this.CreateMap<Post, PostEditBindingModel>()
                .ForMember(
                    model => model.Headline,
                    opt => opt.MapFrom(post => post.Title))
                .ForMember(
                    model => model.Tags,
                    opt => opt.MapFrom(post => string.Join(", ", post.Tags.Select(t => t.Tag.Name)).Replace(Constants.HashTag, string.Empty)))
                .ForMember(
                    model => model.CreationDate,
                    opt => opt.MapFrom(post => string.Format("{0:dd MMM ,yyyy hh:mm tt}", post.CreationDate)))
                .ForMember(
                    model => model.LastUpdateDate,
                    opt => opt.Ignore());

            this.CreateMap<PostEditBindingModel, Post>()
                .ForMember(
                    post => post.Title,
                    opt => opt.MapFrom(model => model.Headline))
                .ForMember(
                    post => post.ImageUrl,
                    opt => opt.Ignore())
                .ForMember(
                    post => post.Tags,
                    opt => opt.Ignore());

            this.CreateMap<Post, PostDeleteViewModel>()
                .ForMember(
                    viewModel => viewModel.Author,
                    opt => opt.MapFrom(post => post.Author.FullName ?? post.Author.UserName))
                .ForMember(
                    viewModel => viewModel.Category,
                    opt => opt.MapFrom(post => post.Category.Name))
                .ForMember(
                    viewModel => viewModel.AuthorAvatarUrl,
                    opt => opt.MapFrom(post => post.Author.AvatarUrl))
                .ForMember(
                    viewModel => viewModel.CommentsCount,
                    opt => opt.MapFrom(post => (post.Comments.Count + post.Comments.Sum(c => c.Replies.Count))))
                .ForMember(
                    viewModel => viewModel.Tags,
                    opt => opt.MapFrom(post => post.Tags.Select(t => t.Tag.Name)))
                .ForMember(
                    viewModel => viewModel.CreationDate,
                    opt => opt.MapFrom(post => string.Format("{0:dd MMM ,yyyy hh:mm tt}", post.CreationDate)))
                .ForMember(
                    viewModel => viewModel.LastUpdateDate,
                    opt => opt.MapFrom(post => string.Format("{0:dd MMM ,yyyy hh:mm tt}", post.LastUpdateDate)));

            this.CreateMap<User, UserConciseViewModel>()
                .ForMember(
                    viewModel => viewModel.IsAdmin,
                    opt => opt.Ignore())
                .ForMember(
                    viewModel => viewModel.IsAuthor,
                    opt => opt.Ignore());

            this.CreateMap<User, UserDetailsViewModel>()
                .ForMember(
                    viewModel => viewModel.CommentsCount,
                    opt => opt.MapFrom(u => u.Comments.Count))
                .ForMember(
                    viewModel => viewModel.RepliesCount,
                    opt => opt.MapFrom(u => u.Replies.Count))
                .ForMember(
                    viewModel => viewModel.PostsCount,
                    opt => opt.MapFrom(u => u.Posts.Count))
                .ForMember(
                    viewModel => viewModel.Roles,
                    opt => opt.Ignore());

            this.CreateMap<Post, SearchPostConciseViewModel>()
                .ForMember(
                    viewModel => viewModel.CommentsCount,
                    opt => opt.MapFrom(post => (post.Comments.Count + post.Comments.Sum(c => c.Replies.Count))))
                .ForMember(
                    viewModel => viewModel.CreationDate,
                    opt => opt.MapFrom(post => string.Format("{0:dd MMM}", post.CreationDate)))
                .ForMember(
                    viewModel => viewModel.CreationYear,
                    opt => opt.MapFrom(post => post.CreationDate.Year.ToString()))
                .ForMember(
                    viewModel => viewModel.Content,
                    opt => opt.MapFrom(post => StringExtensions.TruncateAtWord(post.Content, 120)))
                .ForMember(
                    viewModel => viewModel.CategoryName,
                    opt => opt.MapFrom(post => post.Category.Name));

            this.CreateMap<Category, CategoryHomeConciseViewModel>();

            this.CreateMap<User, TeamMemberConciseViewModel>()
                .ForMember(
                    viewModel => viewModel.Position,
                    opt => opt.Ignore());
        }
    }
}
