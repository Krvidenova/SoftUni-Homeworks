namespace Blog.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        public Tag()
        {
            this.Posts = new List<PostTag>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<PostTag> Posts { get; set; }
    }
}
