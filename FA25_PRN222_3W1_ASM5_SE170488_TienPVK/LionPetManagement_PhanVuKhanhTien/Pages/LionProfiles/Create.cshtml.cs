using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Entity.Models;
using Repository.DBContext;
using Service;
using Microsoft.AspNetCore.Authorization;

namespace LionPetManagement_PhanVuKhanhTien.Pages.LionProfiles
{
    [Authorize(Roles = "2")]
    public class CreateModel : PageModel
    {
        private readonly LionProfileService _service;
        private readonly LionTypeService _subService;

        public CreateModel(LionProfileService context, LionTypeService subService)
        {
            _service = context;
            _subService = subService;
        }

        public async Task<IActionResult> OnGet()
        {
            ViewData["LionTypeId"] = new SelectList(await _subService.GetAllAsync(), "LionTypeId", "LionTypeName");
            return Page();
        }

        [BindProperty]
        public LionProfile LionProfile { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Remove navigation properties from validation
            ModelState.Remove("LionProfile.ModifiedDate");
            ModelState.Remove("LionProfile.LionType");

            if (!ModelState.IsValid)
            {
                ViewData["LionTypeId"] = new SelectList(await _subService.GetAllAsync(), "LionTypeId", "LionTypeName");
                return Page();
            }

            LionProfile.ModifiedDate = DateTime.Now;

            var result = await _service.CreateAsync(LionProfile);
            if (result > 0)
            {
                return RedirectToPage("./Index");
            }

            ViewData["LionTypeId"] = new SelectList(await _subService.GetAllAsync(), "LionTypeId", "LionTypeName");
            return Page();
        }
    }
}
