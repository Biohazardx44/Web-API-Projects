using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteApp.DataAccess.Data;
using NoteApp.DataAccess.Repositories.Abstraction;
using NoteApp.DataAccess.Repositories.Implementation.AdoNetImplementation;
using NoteApp.DataAccess.Repositories.Implementation.DapperImplementation;
using NoteApp.DataAccess.Repositories.Implementation.EntityFrameworkImplementation;
using NoteApp.Services.Abstraction;
using NoteApp.Services.Implementation;

namespace NoteApp.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void InjectServices(this IServiceCollection services)
        {
            services.AddTransient<INoteService, NoteService>();
            services.AddTransient<IUserService, UserService>();
        }

        public static void InjectRepositories(this IServiceCollection services)
        {
            // AdoNet Implementation
            //services.AddTransient<INoteRepository, NoteRepositoryAdoNet>();
            //services.AddTransient<IUserRepository, UserRepositoryAdoNet>();

            // Dapper Implementation
            //services.AddTransient<INoteRepository, NoteRepositoryDapper>();
            //services.AddTransient<IUserRepository, UserRepositoryDapper>();

            // Entity Framework
            services.AddTransient<INoteRepository, NoteRepositoryEntity>();
            services.AddTransient<IUserRepository, UserRepositoryEntity>();
        }

        public static void InjectDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<NoteAppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}