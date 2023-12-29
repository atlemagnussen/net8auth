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
                        
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }
    }
}