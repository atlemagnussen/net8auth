using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using net8auth.auth.Data;
using net8auth.model;

namespace net8auth.auth;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddServices();
        builder.Services.AddOptionsConfiguration(builder.Configuration);
        builder.Services.AddAuthenticationServer(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddRazorPages();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // app.UseMigrationsEndPoint();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // app.MapIdentityApi<ApplicationUser>();

        app.MapControllers();
        app.MapRazorPages();

        app.Run();
    }
}
