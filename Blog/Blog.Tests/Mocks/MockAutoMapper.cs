namespace Blog.Tests.Mocks
{
    using AutoMapper;
    using Blog.Web.Mapping;

    public static class MockAutoMapper
    {
        static MockAutoMapper()
        {
            Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
        }

        public static IMapper GetAutoMapper() => Mapper.Instance;
    }
}
