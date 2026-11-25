
using AbsenceManagementSystem.Data;
using AbsenceManagementSystem.Models;
using AbsenceManagementSystem.Services;
using AbsenceManagementSystem.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

///<summary>
///Create connection string and if not found throws an exception
///Add connection string to the DbContext
///</summary>

var connectionString =
    builder.Configuration.GetConnectionString(
        "DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found!");

// Existing SQL Server database
builder.Services.AddDbContext<AbsenceDbContext>(options =>
    options.UseSqlServer(connectionString));

// ASP.NET Core Identity
builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        // Development configuration without email sender.
        options.SignIn.RequireConfirmedAccount = false;

        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AbsenceDbContext>();

// MVC and Identity Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Existing application services
builder.Services.AddScoped<IAbsenceService, AbsenceService>();
builder.Services.AddScoped<IChildService, ChildService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();

var app = builder.Build();

// Database initialization
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider
        .GetRequiredService<AbsenceDbContext>();

    await DbSeeder.SeedAsync(context);
}

// Seed Identity roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    await IdentityDataSeeder.SeedRolesAsync(roleManager);

    if (app.Environment.IsDevelopment())
    {
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        await IdentityDataSeeder.SeedDirectorAsync(
            userManager,
            app.Configuration);
    }
}

// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication must be before Authorization
app.UseAuthentication();
app.UseAuthorization();

// Identity Razor Pages
app.MapRazorPages();

// Existing MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
