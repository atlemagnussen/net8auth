using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using net8auth.auth.Data;
using net8auth.model;

namespace net8auth.auth
{
    public static class AuthenticationServer
    {
        public static void AddAuthenticationServer(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlite(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<AuthUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AuthDbContext>();
            
            
        }
    }
}