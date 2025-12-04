using Microsoft.EntityFrameworkCore;
using MyPass.Core.Patterns.Builder;
using MyPass.Core.Patterns.Observer;
using MyPass.Core.Services.Account;
using MyPass.Core.Services.Vault;
using MyPass.Core.Session;
using MyPass.Infrastructure.Data;
using MyPass.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);
Environment.SetEnvironmentVariable("ASPNETCORE_BROWSERLINK_ENABLED", "false");

Console.WriteLine(">>> RUNNING FROM DIRECTORY: " + Directory.GetCurrentDirectory());

// MVC
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<MyPassDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVaultRepository, VaultRepository>();

// Business Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IVaultService, VaultService>();

// Session
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<WebSessionManager>();

// Builder
builder.Services.AddTransient<IPasswordBuilder, PasswordBuilder>();
builder.Services.AddTransient<PasswordGeneratorService>();

// Observer (WEAK PASSWORD ONLY)
builder.Services.AddSingleton<NotificationCenter>();
builder.Services.AddSingleton<UserNotificationObserver>();
builder.Services.AddSingleton<PasswordStrengthMonitor>();

// Chain of Responsibility
builder.Services.AddScoped<PasswordRecoveryService>();

var app = builder.Build();

// Attach observer
var notifier = app.Services.GetRequiredService<NotificationCenter>();
var weakPasswordObserver = app.Services.GetRequiredService<UserNotificationObserver>();
notifier.Attach(weakPasswordObserver);

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();


