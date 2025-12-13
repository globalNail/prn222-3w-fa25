using SCMS.Service.TienPVK;
using SCMS.WorkerService.TienPVK;
using SCMS.WorkerService.TienPVK.Services;


var builder = Host.CreateApplicationBuilder(args);

// Configure email settings from appsettings.json
var emailSettings = new EmailSettings();
builder.Configuration.GetSection("EmailSettings").Bind(emailSettings);

// Register services
builder.Services.AddSingleton(emailSettings);
builder.Services.AddSingleton<ExcelExportService>();
builder.Services.AddSingleton<EmailService>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Clubs_Worker Service";
});

builder.Services.AddSingleton<SystemAccountService>();
builder.Services.AddSingleton<ClubCategoriesTienPvkService>();
builder.Services.AddSingleton<ClubsTienPvkService>();

var host = builder.Build();
host.Run();
