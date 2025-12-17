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

namespace LionPetManagement_PhanVuKhanhTien.Pages.LionProfiles
{
    public class DeleteModel : PageModel
    {
        private readonly LionProfileService _service;

        public DeleteModel(LionProfileService context)
        {
            _service = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var obj = await _service.GetByIdAsync(id.Value);
            if (obj != null)
            {
                LionProfile = obj;
                await _service.DeleteAsync(id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}
