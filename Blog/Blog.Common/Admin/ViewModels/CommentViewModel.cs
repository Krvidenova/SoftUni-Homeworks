namespace Blog.Common.Admin.ViewModels
{
    using System.Collections.Generic;

    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string CreationDate { get; set; }

        public string LastUpdateDate { get; set; }

        public bool IsApproved { get; set; }

        public bool IsCensored { get; set; }

        public string PostId { get; set; }
        
        public string AuthorId { get; set; }

        public string Author { get; set; }

        public string AuthorAvatarUrl { get; set; }

        public ICollection<ReplyViewModel> Replies { get; set; }
    }
}
