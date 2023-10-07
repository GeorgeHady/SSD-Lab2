using Lab1.Data;
using Lab1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace Lab1;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Get Passwords from Secrets
        var conficuration = app.Services.GetService<IConfiguration>();
        var hosting = app.Services.GetService<IWebHostEnvironment>();
        if (hosting.IsDevelopment())
        {
            //names must match in Secrets-json and AppSecrets-class
            var secrets = conficuration.GetSection("Secrets").Get<AppSecrets>();

            //send the secrets to initializer class
            DbInitializer.appSecrets = secrets;
        }

        // database startup seeding users and thier roles
        using (var scopre = app.Services.CreateScope())
        {
            DbInitializer.SeedUsersAndRoles(scopre.ServiceProvider).Wait();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}

