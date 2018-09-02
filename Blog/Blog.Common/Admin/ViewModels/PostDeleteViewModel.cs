namespace Blog.Common.Admin.ViewModels
{
    using System.Collections.Generic;

    public class PostDeleteViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string CreationDate { get; set; }

        public string LastUpdateDate { get; set; }
        
        public string AuthorId { get; set; }

        public string Author { get; set; }

        public string AuthorAvatarUrl { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }

        public int ViewCounter { get; set; }

        public int CommentsCount { get; set; }

        public ICollection<string> Tags { get; set; }
    }
}
