using AutoMapper;

namespace Inbox0.Web.Services.Mapping
{
    public static class DependencyInjection
    {
        public static void AddAppMappers(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
