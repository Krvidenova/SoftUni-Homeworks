namespace Blog.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category
    {
        public Category()
        {
            this.Posts = new List<Post>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}