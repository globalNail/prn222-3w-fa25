using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK.Implements;
using SCMS.Service.TienPVK.Implements;
using SCMS.WorkerService.TienPVK;


var builder = Host.CreateApplicationBuilder(args);
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
