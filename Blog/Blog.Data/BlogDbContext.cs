namespace Blog.Data
{
    using Blog.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class BlogDbContext : IdentityDbContext<User>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Post>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Reply>()
                .HasOne(r => r.Comment)
                .WithMany(c => c.Replies)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });

            builder
                .Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.Posts)
                .HasForeignKey(pt => pt.TagId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique(true);

            base.OnModelCreating(builder);
        }
    }
}
