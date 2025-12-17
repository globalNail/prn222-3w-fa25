using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Entity.Models;
using Service;

namespace LionPetManagement_PhanVuKhanhTien.Pages.LionProfiles
{
    public class EditModel : PageModel
    {
        private readonly LionProfileService _service;
        private readonly LionTypeService _subService;

        public EditModel(LionProfileService context, LionTypeService subService)
        {
            _service = context;
            _subService = subService;
        }

        [BindProperty]
        public Entity.Models.LionProfile LionProfile { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lionprofile =  await _service.GetByIdAsync(id.Value);
            if (lionprofile == null)
            {
                return NotFound();
            }
            LionProfile = lionprofile;
           ViewData["LionTypeId"] = new SelectList(await _subService.GetAllAsync(), "LionTypeId", "LionTypeName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var updated = await _service.UpdateAsync(LionProfile);
            if (updated == null)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }

        private bool LionProfileExists(int id)
        {
            return _service.AnyAsync(id).Result;
        }
    }
}
