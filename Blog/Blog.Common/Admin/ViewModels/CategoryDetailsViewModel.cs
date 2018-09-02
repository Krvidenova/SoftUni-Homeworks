namespace Blog.Common.Admin.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Posts Count")]
        public int PostsCount { get; set; }
    }
}