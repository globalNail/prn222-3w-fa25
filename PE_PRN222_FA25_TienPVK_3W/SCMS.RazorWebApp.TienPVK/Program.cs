using SCMS.Repository.TienPVK.Implements;
using SCMS.Service.TienPVK.Implements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register concrete classes directly (no interfaces)
builder.Services.AddScoped<ClubsTienPvkRepository>();
builder.Services.AddScoped<ClubCategoriesTienPvkRepository>();
builder.Services.AddScoped<ClubsTienPvkService>();
builder.Services.AddScoped<ClubCategoriesTienPvkService>();


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

app.MapRazorPages();

app.Run();
