using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Entity.Models;
using Service;
using Microsoft.AspNetCore.Authorization;

namespace LionPetManagement_PhanVuKhanhTien.Pages.LionProfiles
{
    [Authorize(Roles = "2")]
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
            ModelState.Remove("LionProfile.ModifiedDate");
            ModelState.Remove("LionProfile.LionType");

            if (!ModelState.IsValid)
            {
                ViewData["LionTypeId"] = new SelectList(await _subService.GetAllAsync(), "LionTypeId", "LionTypeName");
                return Page();
            }

            LionProfile.ModifiedDate = DateTime.Now;

            var updated = await _service.UpdateAsync(LionProfile);
            if (updated > 0)
            {
                return RedirectToPage("./Index");
            }


            ViewData["LionTypeId"] = new SelectList(await _subService.GetAllAsync(), "LionTypeId", "LionTypeName");
            return Page();
        }

        private bool LionProfileExists(int id)
        {
            return _service.AnyAsync(id).Result;
        }
    }
}
