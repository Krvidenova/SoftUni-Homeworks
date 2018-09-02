namespace Blog.Tests.Controllers.HomeControllerTests
{
    using Blog.Services.Category.Interfaces;
    using Blog.Web.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SearchTests
    {
        [TestMethod]
        public void Search_ReturnsTheProperView()
        {
            // Arrange
            var mockCategoryPostsService = new Mock<ICategoryPostsService>();
            var mockCategoryService = new Mock<ICategoryService>();
            var controller = new HomeController(mockCategoryPostsService.Object, mockCategoryService.Object);

            // Act
            var result = controller.Search(string.Empty);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}