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
    public class DetailsModel : PageModel
    {
        private readonly ChargingSessionService _service;

        public DetailsModel(ChargingSessionService context)
        {
            _service = context;
        }

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
    }
}
