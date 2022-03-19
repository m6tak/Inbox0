using System.Reflection;

namespace Inbox0.Web.Services.Repositories.Base
{
    public static class DependencyInjection
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            var allRepositoryTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.BaseType?.Name == typeof(GenericRepository<>).Name)
                .ToList();

            allRepositoryTypes.ForEach(repositoryType => services.AddScoped(repositoryType));
        }
    }
}
