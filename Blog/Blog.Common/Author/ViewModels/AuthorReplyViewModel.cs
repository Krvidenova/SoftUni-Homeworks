namespace Blog.Common.Author.ViewModels
{
    public class AuthorReplyViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string CreationDate { get; set; }

        public string LastUpdateDate { get; set; }

        public bool IsApproved { get; set; }

        public bool IsCensored { get; set; }
        
        public string AuthorId { get; set; }

        public string Author { get; set; }

        public string AuthorAvatarUrl { get; set; }
    }
}
