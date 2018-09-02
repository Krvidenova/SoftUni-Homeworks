namespace Blog.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User()
        {
            this.Comments = new List<Comment>();
            this.Replies = new List<Reply>();
            this.Posts = new List<Post>();
            this.IsBanned = false;
        }

        public string FullName { get; set; }

        public string AvatarUrl { get; set; }

        public string FacebookProfileUrl { get; set; }

        public string TwitterProfileUrl { get; set; }

        public string InstagramProfileUrl { get; set; }

        public bool IsBanned { get; set; }

        public int CensoredCommentsCount { get; set; }

        public int DeletedCommentsCount { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Reply> Replies { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
