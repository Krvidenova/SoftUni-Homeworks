namespace Blog.Common.Home.ViewModels
{
    public class SearchPostConciseViewModel
    {
        public int Id { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string CreationDate { get; set; }

        public string CreationYear { get; set; }

        public int ViewCounter { get; set; }

        public int CommentsCount { get; set; }

        public string CategoryName { get; set; }
    }
}
