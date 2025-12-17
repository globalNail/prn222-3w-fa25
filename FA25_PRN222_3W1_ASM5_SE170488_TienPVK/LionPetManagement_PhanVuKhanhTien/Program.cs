using LionPetManagement_PhanVuKhanhTien.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//  ===========
//||  SignalR  ||
//  ===========
builder.Services.AddSignalR();

//  ================
//||  Core Service  ||
//  ================
builder.Services.AddScoped<LionAccountService>();
builder.Services.AddScoped<LionProfileService>();
builder.Services.AddScoped<LionTypeService>();

//  ==================================
//||  Authentication & Authorization  ||
//  ==================================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Forbidden";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
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
app.MapHub<LionHub>("/LionHub");
app.Run();
