namespace Blog.Tests.IntegrationTests
{
    using System.Net;
    using System.Threading.Tasks;
    using Blog.Web;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HomeControllerIntegrationTests
    {
        [TestMethod]
        public async Task Index_ShouldReturnIndexView()
        {
            // Arrange
            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            // Act
            var result = await client.GetAsync("/");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            var resultContent = await result.Content.ReadAsStringAsync();
            Assert.IsTrue(resultContent.Contains("Welcome"));
        }
    }
}