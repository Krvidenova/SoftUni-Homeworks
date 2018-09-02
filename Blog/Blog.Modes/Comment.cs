namespace Blog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        public Comment()
        {
            this.Replies = new List<Reply>();
            this.CreationDate = DateTime.UtcNow;
            this.IsCensored = false;
            this.IsApproved = false;
        }

        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public bool IsApproved { get; set; }

        public bool IsCensored { get; set; }

        public string AuthorId { get; set; }

        public User Author { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

        public ICollection<Reply> Replies { get; set; }
    }
}