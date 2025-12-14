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

    public async Task SendCreateClub(ClubsTienPvk club)
    {
        // Validate required fields
        if (string.IsNullOrEmpty(club.ClubCode) || string.IsNullOrEmpty(club.ClubName) || 
            club.CategoryIdtienPvk == 0 || string.IsNullOrEmpty(club.Status))
        {
            throw new HubException("Required fields are missing");
        }

        // Set system-generated values
        club.CreatedAt = DateTime.Now;
        club.IsDeleted = false;

        // Create the club in database
        var result = await _service.CreateAsync(club);

        if (result > 0)
        {
            // Broadcast to all connected clients about the new club
            await Clients.All.SendAsync("CreateClub", result.ToString());
        }
        else
        {
            throw new HubException("Failed to create club in database");
        }
    }

    public async Task SendUpdateClub(string clubId)
    {
        // Broadcast to all connected clients about the updated club
        await Clients.All.SendAsync("UpdateClub", clubId);
    }

}
