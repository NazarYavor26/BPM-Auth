using BPM_Auth.DAL.DbContexts;
using BPM_Auth.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BPM_Auth.DAL
{
    public static class DALModule
    {
        public static void InitializeDAL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DBConnection")));

            services.AddTransient<IUserRepository, UserRepository>();/*
            services.AddTransient<ITeamMembershipRepository, TeamMembershipRepository>();
            services.AddTransient<ITeamRepository, TeamRepository>();*/
        }
    }
}