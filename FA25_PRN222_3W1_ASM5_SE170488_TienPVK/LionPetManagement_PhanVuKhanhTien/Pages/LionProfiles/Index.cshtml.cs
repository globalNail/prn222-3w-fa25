using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Entity.Models;
using Service;
using Microsoft.AspNetCore.Authorization;

namespace LionPetManagement_PhanVuKhanhTien.Pages.LionProfiles
{
    [Authorize(Roles =("3,2"))]
    public class IndexModel : PageModel
    {
        private readonly LionProfileService _service;

        public IndexModel(LionProfileService service) => _service = service;

        public IList<Entity.Models.LionProfile> LionProfile { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public async Task OnGetAsync()
        {
            LionProfile = await _service.GetAllAsync();
        }

    }
}
