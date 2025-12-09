using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SCMS.Domain.TienPVK.Models;
using SCMS.Service.TienPVK.Implements;

namespace SCMS.RazorWebApp.TienPVK.Pages.ClubsTienPvks
{
    public class DetailsModel : PageModel
    {
        private readonly ClubsTienPvkService _service;

        public DetailsModel(ClubsTienPvkService service)
        {
            _service = service;
        }

        public ClubsTienPvk ClubsTienPvk { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubstienpvk = await _service.GetByIdAsync(id.Value);
            if (clubstienpvk == null)
            {
                return NotFound();
            }
            else
            {
                ClubsTienPvk = clubstienpvk;
            }
            return Page();
        }
    }
}
