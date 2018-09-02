namespace Blog.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Reply
    {
        public Reply()
        {
            this.CreationDate = DateTime.UtcNow;
            this.IsCensored = false;
            this.IsApproved = false;
        }

        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public bool IsCensored { get; set; }

        public bool IsApproved { get; set; }

        public string AuthorId { get; set; }

        public User Author { get; set; }

        public int CommentId { get; set; }

        public Comment Comment { get; set; }
    }
}