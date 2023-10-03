using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteApp.DataAccess.Data;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.DataAccess.Repositories.Implementation.EntityFrameworkImplementation;
using NoteApp.Services.Abstraction;
using NoteApp.Services.Implementation;

namespace NoteApp.Helpers
{
    public static class DependencyInjectionHelper
    {
        /// <summary>
        /// Injects the application's services into the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        public static void InjectServices(this IServiceCollection services)
        {
            services.AddTransient<INoteService, NoteService>();
            services.AddTransient<IUserService, UserService>();
        }

        /// <summary>
        /// Injects the application's repositories into the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the repositories to.</param>
        public static void InjectRepositories(this IServiceCollection services)
        {
            // AdoNet Implementation
            //services.AddTransient<INoteRepository, NoteRepositoryAdoNet>();
            //services.AddTransient<IUserRepository, UserRepositoryAdoNet>();

            // Dapper Implementation
            //services.AddTransient<INoteRepository, NoteRepositoryDapper>();
            //services.AddTransient<IUserRepository, UserRepositoryDapper>();

            // Entity Framework Implementation
            services.AddTransient<INoteRepository, NoteRepositoryEntity>();
            services.AddTransient<IUserRepository, UserRepositoryEntity>();
        }

        /// <summary>
        /// Injects the application's DbContext into the dependency injection container.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the DbContext to.</param>
        /// <param name="connectionString">The connection string to the database.</param>
        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<NoteAppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}