//this page is based on professor Mohamed Elliethy, and edited by George Hady

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Lab1.Models;

namespace Lab1.Data;

public static class DbInitializer
{
    public static AppSecrets appSecrets { get; set; }



    public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
    {
        // create the database if it doesn't exist
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Check if roles already exist and exit if there are
        if (roleManager.Roles.Count() > 0)
            return 1;  // should log an error message here

        // Seed roles
        int result = await SeedRoles(roleManager);
        if (result != 0)
            return 2;  // should log an error message here

        // Check if users already exist and exit if there are
        if (userManager.Users.Count() > 0)
            return 3;  // should log an error message here

        // Seed users
        result = await SeedUsers(userManager);
        if (result != 0)
            return 4;  // should log an error message here

        return 0;
    }

    private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        // Create Admin Role
        var result = await roleManager.CreateAsync(new IdentityRole("Manager"));
        if (!result.Succeeded)
            return 1;  // should log an error message here

        // Create Member Role
        result = await roleManager.CreateAsync(new IdentityRole("Employee"));
        if (!result.Succeeded)
            return 2;  // should log an error message here

        return 0;
    }




    private static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager)
    {
        // Create Manager User
        var adminUser = new ApplicationUser
        {
            UserName = "Manager@mohawkcollege.ca",
            Email = "Manager@mohawkcollege.ca",
            FirstName = "No Input",
            LastName = "No Input",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(adminUser, appSecrets.ManagerPassword);
        if (!result.Succeeded)
            return 1;  // should log an error message here

        // Assign user to Manager role
        result = await userManager.AddToRoleAsync(adminUser, "Manager");
        if (!result.Succeeded)
            return 2;  // should log an error message here





        // Create Employee User
        var memberUser = new ApplicationUser
        {
            UserName = "Employee@mohawkcollege.ca",
            Email = "Employee@mohawkcollege.ca",
            FirstName = "No Input",
            LastName = "No Input",
            EmailConfirmed = true
        };
        result = await userManager.CreateAsync(memberUser, appSecrets.EmployeePassword);
        if (!result.Succeeded)
            return 3;  // should log an error message here

        // Assign user to Employee role
        result = await userManager.AddToRoleAsync(memberUser, "Employee");
        if (!result.Succeeded)
            return 4;  // should log an error message here

        return 0;
    }
}
