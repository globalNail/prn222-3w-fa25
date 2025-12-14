using Microsoft.AspNetCore.Authentication.Cookies;
using SCMS.RazorWebApp.TienPVK.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

//  ===========
//||  SignalR  ||
//  ===========
builder.Services.AddSignalR();

// Register concrete classes directly (no interfaces)
builder.Services.AddScoped<SystemAccountService>();
builder.Services.AddScoped<ClubsTienPvkService>();
builder.Services.AddScoped<ClubCategoriesTienPvkService>();

//  ==================================
//||  Authentication & Authorization  ||
//  ==================================
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

// RequireAuthorization(); ~~> Global Authorization Filter, add [AllowAnonymous] attribute for public pages
//  ==========================
//||  Require  Authorization  ||
//  ==========================
app.MapRazorPages().RequireAuthorization();

app.MapHub<ClubHub>("/clubHub");

app.Run();
