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

        // Delete LION
        public async Task HubDelete(string objId)
        {
            if (!int.TryParse(objId, out int id))
            {
                await Clients.Caller.SendAsync("Receiver_DeleteFailed", new List<string> { "Invalid ID." });
                return;
            }

            bool isDeleted = await _service.DeleteAsync(id);

            if (isDeleted)
            {
                // Delete from database first
                await Clients.All.SendAsync("Receiver_Delete", objId);
            }
            else
            {
                // Then notify all clients
                await Clients.Caller.SendAsync("Receiver_DeleteFailed", new List<string> { "Delete operation failed." });
            }
        }

        private bool TryValidate(object model, out List<string> errors)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, context, results, true);

            errors = results.Select(e => e.ErrorMessage).ToList();
            return isValid;
        }
    }
}
