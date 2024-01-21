using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using net8auth.auth.Data;
using net8auth.auth.Services;
using net8auth.model;

namespace net8auth.auth
{
    public static class AuthenticationServer
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<WellKnownService>();
            services.AddTransient<UsersService>();
        }

        public static void AddAuthenticationServer(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlite(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            // services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
            //     .AddEntityFrameworkStores<AuthDbContext>();
                        
            // services.AddAuthentication(IdentityConstants.ApplicationScheme)
            //     .AddIdentityCookies();
            
            services.AddAuthorizationBuilder();

            services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddRoleManager<RoleManager<ApplicationRole>>();
                
            // services.AddIdentityCore<ApplicationUser>()
            //     .AddEntityFrameworkStores<AuthDbContext>()
            //     .AddApiEndpoints();

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

            var authBuilder = services.AddAuthentication();

            authBuilder.AddOpenIdConnect("AzureAd", "Azure Active Directory", options =>
            {
                options.SignInScheme = "idsrv.external";
                options.SignOutScheme = "idsrv";

                options.Authority = $"https://login.microsoftonline.com/common/v2.0/";
                options.ClientId = "ffc92b44-2f67-45ab-a112-3e20ff6dcf78";
                options.ResponseType = "id_token";

                options.CallbackPath = "/signin-aad";
                options.SignedOutCallbackPath = "/signout-callback-aad";
                options.RemoteSignOutPath = "/signout-aad";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                    ValidateIssuer = false
                };
                options.Events.OnRedirectToIdentityProvider = context =>
                {
                    Console.WriteLine(context.ToString());

                    return Task.FromResult(0);
                };
                options.Events.OnRemoteFailure = context =>
                {
                    Console.WriteLine(context.Failure.Message);
                    return Task.FromResult(0);
                };
            });

        }
    }
}