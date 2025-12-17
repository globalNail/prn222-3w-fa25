using Microsoft.AspNetCore.SignalR;
using Service;
using System.ComponentModel.DataAnnotations;

namespace LionPetManagement_PhanVuKhanhTien.Hubs
{
    public class LionHub : Hub
    {
        private readonly LionProfileService _service;

        public LionHub(LionProfileService service)
        {
            _service = service;
        }

        public async Task HubDelete(string objId)
        {
            var result = await _service.DeleteAsync(Int32.Parse(objId));

            if (result)
            {
                // Broadcast to all connected clients after successful deletion
                await Clients.All.SendAsync("Receiver_Delete", objId);
            }
            else
            {
                // Then notify all clients
                await Clients.Caller.SendAsync("Receiver_DeleteFailed", new List<string> { "Delete operation failed." });
            }
        }
    }
}
