using Microsoft.AspNetCore.Mvc.RazorPages;
using SCMS.Domain.TienPVK.Models;
using SCMS.Service.TienPVK.Implements;

namespace SCMS.RazorWebApp.TienPVK.Pages.ClubsTienPvks
{
    public class IndexModel : PageModel
    {
        private readonly ClubsTienPvkService _service;

        public IndexModel(ClubsTienPvkService service) => _service = service;

        public IList<ClubsTienPvk> ClubsTienPvk { get; set; } = default!;

        public async Task OnGetAsync()
        {
            ClubsTienPvk = await _service.GetAllAsync();
        }
    }
}
