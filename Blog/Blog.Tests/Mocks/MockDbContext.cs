namespace Blog.Tests.Mocks
{
    using System;
    using Blog.Data;
    using Microsoft.EntityFrameworkCore;

    public static class MockDbContext
    {
        public static BlogDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BlogDbContext(options);
        }
    }
}
