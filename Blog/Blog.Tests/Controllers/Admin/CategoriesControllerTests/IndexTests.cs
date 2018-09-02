namespace Blog.Tests.Controllers.Admin.CategoriesControllerTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Common.Admin.ViewModels;
    using Blog.Services.Admin.Interfaces;
    using Blog.Web.Areas.Admin.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class IndexTests
    {
        [TestMethod]
        public async Task Index_ReturnsAllCourses()
        {
            // Arrange
            var categoryModel = new CategoryConciseViewModel() { Id = 1, Name = "Business", Order = 1};
            bool methodCalled = false;

            var mockRepository = new Mock<IAdminCategoriesService>();
            mockRepository
                .Setup(service => service.GetCategoriesAsync())
                .ReturnsAsync(new[] { categoryModel })
                .Callback(() => methodCalled = true);

            var controller = new CategoriesController(mockRepository.Object);

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var resultView = result as ViewResult;
            Assert.IsNotNull(resultView.Model);
            Assert.IsInstanceOfType(resultView.Model, typeof(IEnumerable<CategoryConciseViewModel>));
            Assert.IsTrue(methodCalled);
        }
    }
}