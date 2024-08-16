using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieApp.DataAccess.Data;
using MovieApp.DataAccess.Repositories.Abstraction;
using MovieApp.DataAccess.Repositories.Implementation.EntityFrameworkImplementation;
using MovieApp.Services.Abstraction;
using MovieApp.Services.Implementation;

namespace MovieApp.Helpers
{
    public static class DependencyInjectionHelper
    {
        /// <summary>
        /// Injects the application's services into the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        public static void InjectServices(this IServiceCollection services)
        {
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IUserService, UserService>();
        }

        /// <summary>
        /// Injects the application's repositories into the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the repositories to.</param>
        public static void InjectRepositories(this IServiceCollection services)
        {
            // Entity Framework Implementation
            services.AddTransient<IMovieRepository, MovieRepositoryEntity>();
            services.AddTransient<IUserRepository, UserRepositoryEntity>();
        }

        /// <summary>
        /// Injects the application's DbContext into the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the DbContext to.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MovieAppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}