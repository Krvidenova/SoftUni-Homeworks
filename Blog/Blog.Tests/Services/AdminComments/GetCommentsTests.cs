namespace Blog.Tests.Services.AdminComments
{
    using System.Linq;
    using System.Threading.Tasks;
    using Blog.Data;
    using Blog.Models;
    using Blog.Services.Admin;
    using Blog.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetCommentsTests
    {
        private BlogDbContext dbContext;
        private AdminCommentsService service;

        [TestInitialize]
        public void InitializeTests()
        {
            this.dbContext = MockDbContext.GetContext();
            this.service = new AdminCommentsService(this.dbContext, MockAutoMapper.GetAutoMapper());
        }

        [TestMethod]
        public async Task GetCommentsByPostIdAsync_WithFewComments_ShouldReturnAll()
        {
            // Arrange           
            this.dbContext.Comments.Add(new Comment() { Id = 1, Content = "First comment", AuthorId = "123", PostId = 1 });
            this.dbContext.Comments.Add(new Comment() { Id = 2, Content = "Second comment", AuthorId = "231", PostId = 1 });
            this.dbContext.Comments.Add(new Comment() { Id = 3, Content = "Third comment", AuthorId = "132", PostId = 1 });
            this.dbContext.SaveChanges();

            // Act
            var courses = await this.service.GetCommentsByPostIdAsync(1);

            // Assert
            Assert.IsNotNull(courses);
            Assert.AreEqual(3, courses.Count());            
        }

        [TestMethod]
        public async Task GetCommentsForTodayDateAsync_WithZeroComments_ShouldReturnNone()
        {
            // Act
            var courses = await this.service.GetCommentsForTodayDateAsync();

            // Assert
            Assert.IsNotNull(courses);
            Assert.AreEqual(0, courses.Count());
        }
    }
}
