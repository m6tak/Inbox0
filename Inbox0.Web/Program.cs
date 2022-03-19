using Inbox0.Core.Tools.General;
using Inbox0.Web.Services.EF;
using Inbox0.Web.Services.Mail;
using Inbox0.Web.Services.Mapping;
using Inbox0.Web.Services.Repositories.Base;
using Inbox0.Web.Services.Session;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection")));
builder.Services.AddScoped<ISessionManager, SessionManager>();

builder.Services.AddCoreDependencies();
builder.Services.AddRepositories();
builder.Services.AddAppMappers();

builder.Services.AddScoped<IMailService, MailService>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "Inbox0.Session";
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(6);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.None;
        options.LoginPath = "/Home/Login";
        options.LogoutPath = "/Home/Login";
        options.Cookie.Name = "CurrentSessionCookie";
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.Redirect("/Home/Login");
            return Task.CompletedTask;
        };
        options.Cookie.SameSite = SameSiteMode.None;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authorized", policy => policy.RequireClaim("Authorized"));
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.HttpOnly = HttpOnlyPolicy.None;
    options.Secure = CookieSecurePolicy.Always;
    options.CheckConsentNeeded = context => false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(6);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
app.UseCookiePolicy();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
