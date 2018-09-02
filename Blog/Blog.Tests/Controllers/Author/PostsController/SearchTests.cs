namespace Blog.Tests.Controllers.Author.PostsController
{
    using Blog.Common.Author.BindingModels;
    using Blog.Services.Author.Interfaces;
    using Blog.Web.Areas.Author.Controllers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SearchTests
    {
        [TestMethod]
        public void Search_WithValidPostModel_ShouldCallService()
        {
            // Arrange
            bool serviceCalled = false;

            var mockRepository = new Mock<IAuthorPostsService>();
            mockRepository
                .Setup(repo => repo.GetPostsAsync(It.IsAny<PostSearchBindingModel>()))
                .Callback(() => serviceCalled = true);

            var controller = new PostsController(mockRepository.Object);

            // Act
            var result = controller.Search(new PostSearchBindingModel());

            // Assert
            Assert.IsTrue(serviceCalled);
        }
    }
}
