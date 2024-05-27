using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieApp.DataAccess.Data;
using MovieApp.DataAccess.Repositories.Abstraction;
using MovieApp.DataAccess.Repositories.Implementation.EntityFrameworkImplementation;
using MovieApp.Domain.Models;
using MovieApp.Services.Abstraction;
using MovieApp.Services.Implementation;

namespace MovieApp.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectServices(this IServiceCollection services)
        {
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IUserService, UserService>();
        }

        public static void InjectRepositories(this IServiceCollection services)
        {
            // Entity Framework
            services.AddTransient<IRepository<Movie>, MovieRepositoryEntity>();
            services.AddTransient<IUserRepository, UserRepositoryEntity>();
        }

        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MovieAppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}