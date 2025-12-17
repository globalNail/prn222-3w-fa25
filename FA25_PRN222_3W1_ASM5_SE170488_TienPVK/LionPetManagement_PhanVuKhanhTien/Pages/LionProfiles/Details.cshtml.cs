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
    [Authorize(Roles = "2,3")]
    public class DetailsModel : PageModel
    {
        private readonly LionProfileService _service;
        private readonly LionTypeService _subService;

        public DetailsModel(LionProfileService context, LionTypeService subService)
        {
            _service = context;
            _subService = subService;
        }

        public Entity.Models.LionProfile LionProfile { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lionprofile = await _service.GetByIdAsync(id.Value);
            if (lionprofile == null)
            {
                return NotFound();
            }
            else
            {
                LionProfile = lionprofile;
            }
            return Page();
        }
    }
}
