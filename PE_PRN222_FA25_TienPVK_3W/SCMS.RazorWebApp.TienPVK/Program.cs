using Microsoft.AspNetCore.Authentication.Cookies;
using SCMS.RazorWebApp.TienPVK.Hubs;
using SCMS.Repository.TienPVK.Implements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// Register concrete classes directly (no interfaces)
builder.Services.AddScoped<ClubsTienPvkRepository>();
builder.Services.AddScoped<ClubCategoriesTienPvkRepository>();
builder.Services.AddScoped<SystemAccountService>();
builder.Services.AddScoped<ClubsTienPvkService>();
builder.Services.AddScoped<ClubCategoriesTienPvkService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Forbidden";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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

app.MapRazorPages().RequireAuthorization();

// Allow anonymous access to login/logout pages
app.MapRazorPages().AllowAnonymous();

app.MapHub<ClubHub>("/clubHub");

// Redirect root to login if not authenticated
app.MapGet("/", context =>
{
    if (!context.User.Identity?.IsAuthenticated ?? true)
    {
        context.Response.Redirect("/Account/Login");
    }
    else
    {
        context.Response.Redirect("/ClubsTienPvks/Index");
    }
    return Task.CompletedTask;
});

app.Run();
