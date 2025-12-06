using Microsoft.AspNetCore.Authentication.Cookies;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//  ===========
//||  SignalR  ||
//  ===========
builder.Services.AddSignalR();

//  =============================
//||  Application Service Inject ||
//  =============================
builder.Services.AddScoped<ChargingSessionService>();
builder.Services.AddScoped<ChargingStationService>();
builder.Services.AddScoped<SystemUserService>();

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

app.UseAuthorization();

// RequireAuthorization(); ~~> Global Authorization Filter, add [AllowAnonymous] attribute for public pages
//  ==========================
//||  Require  Authorization  ||
//  ==========================
app.MapRazorPages().RequireAuthorization();

app.MapRazorPages();

app.Run();
