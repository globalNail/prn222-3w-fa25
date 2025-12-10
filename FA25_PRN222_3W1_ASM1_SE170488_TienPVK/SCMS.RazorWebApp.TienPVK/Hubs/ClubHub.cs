using Microsoft.AspNetCore.SignalR;
using SCMS.Service.TienPVK.Implements;

namespace SCMS.RazorWebApp.TienPVK.Hubs;

public class ClubHub : Hub
{
    private readonly ClubsTienPvkService _service;

    public ClubHub(ClubsTienPvkService service)
    {
        _service = service;
    }

    public async Task SendDeleteClub(string clubId)
    {
        // Delete the club from database
        var result = await _service.DeleteAsync(Int32.Parse(clubId));

        if (result)
        {
            // Broadcast to all connected clients after successful deletion
            await Clients.All.SendAsync("DeleteClub", clubId);
        }
    }

    public async Task SendCreateClub(string clubId)
    {
        // Delete the club from database
        var result = await _service.DeleteAsync(Int32.Parse(clubId));

        if (result)
        {
            // Broadcast to all connected clients after successful deletion
            await Clients.All.SendAsync("CreateClub", clubId);
        }
    }

    public async Task SendUpdateClub(string clubId)
    {
        // Delete the club from database
        var result = await _service.DeleteAsync(Int32.Parse(clubId));

        if (result)
        {
            // Broadcast to all connected clients after successful deletion
            await Clients.All.SendAsync("UpdateClub", clubId);
        }
    }

}
