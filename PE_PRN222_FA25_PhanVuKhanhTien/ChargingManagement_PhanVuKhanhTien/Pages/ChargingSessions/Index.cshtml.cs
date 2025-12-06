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
    public class IndexModel : PageModel
    {
        private readonly ChargingSessionService _service;

        public IndexModel(ChargingSessionService service)
        {
            _service = service;
        }

        public IList<ChargingSession> ChargingSession { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ChargingSession = await _service.GetAllAsync();
        }
    }
}
