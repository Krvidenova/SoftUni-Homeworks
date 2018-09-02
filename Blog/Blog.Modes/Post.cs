namespace Blog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Post
    {
        public Post()
        {
            this.Comments = new List<Comment>();
            this.Tags = new List<PostTag>();
            this.CreationDate = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public string AuthorId { get; set; }

        public User Author { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int ViewCounter { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<PostTag> Tags { get; set; }
    }
}