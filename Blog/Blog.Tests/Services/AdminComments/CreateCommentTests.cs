namespace Blog.Tests.Services.AdminComments
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Blog.Common.Admin.BindingModels;
    using Blog.Common.Infrastructure.Validation;
    using Blog.Data;
    using Blog.Models;
    using Blog.Services.Admin;
    using Blog.Tests.Mocks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CreateCommentTests
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
        public async Task CreateCommentAsync_WithProperCommentModel_ShouldAddCorrectly()
        {
            // Arrange
            string authorId = "123";
            int postId = 1;
            string content = "comment content";

            // Act
            await this.service.CreateCommentAsync(authorId, postId, content);

            // Assert
            Assert.AreEqual(1, this.dbContext.Comments.Count());
            var comment = this.dbContext.Comments.First();
            Assert.AreEqual(comment.Content, content);
            Assert.AreEqual(comment.AuthorId, authorId);
            Assert.AreEqual(comment.PostId, postId);
        }

        [TestMethod]
        public async Task CreateCommentAsync_WithNullContentModel_ShouldThrowExc()
        {
            // Arrange
            string authorId = "123";
            int postId = 1;
            string content = null;

            // Act
            Func<Task> addComment = () => this.service.CreateCommentAsync(authorId, postId, content);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(addComment);
            var expectedMessage = string.Format(ValidationConstants.ValueNullEmptyWhiteSpaceMsg, nameof(content));
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [TestMethod]
        public async Task AddCommentAsync_WithNullAuthorId_ShouldThrowExc()
        {
            // Arrange
            string authorId = null;
            int postId = 1;
            string content = "comment content";

            // Act
            Func<Task> addComment = () => this.service.CreateCommentAsync(authorId, postId, content);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(addComment);
            var expectedMessage = string
                .Format(ValidationConstants.ValueNullEmptyWhiteSpaceMsg, nameof(authorId));
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [TestMethod]
        public async Task AddCommentAsync_WithZeroPostIdSlug_ShouldThrowExc()
        {
            // Arrange
            string authorId = "123";
            int postId = 0;
            string content = "comment content";

            // Act
            Func<Task> addComment = () => this.service.CreateCommentAsync(authorId, postId, content);

            // Assert
            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(addComment);
            var expectedMessage = string
                .Format(ValidationConstants.ValueZeroOrNegativeMsg, nameof(postId));
            Assert.AreEqual(expectedMessage, exception.Message);
        }
    }
}
