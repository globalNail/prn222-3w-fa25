using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Entity.Models;
using Repository.DBContext;
using Service;

namespace ChargingManagement_PhanVuKhanhTien.Pages.ChargingSessions
{
    public class DeleteModel : PageModel
    {
        private readonly ChargingSessionService _service;

        public DeleteModel(ChargingSessionService service)
        {
            _service = service;
        }

        [BindProperty]
        public ChargingSession ChargingSession { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chargingsession = await _service.GetByIdAsync(id.Value);

            if (chargingsession == null)
            {
                return NotFound();
            }
            else
            {
                ChargingSession = chargingsession;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chargingsession = await _service.DeleteAsync(id.Value);

            if (chargingsession)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
