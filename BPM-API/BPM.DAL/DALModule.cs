using BPM.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BPM.DAL
{
    public class DALModule
    {
        public static void Load(IServiceCollection services, IConfiguration configuration)
        {
            // DAL Services
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DBConnection")));
        }
    }
}