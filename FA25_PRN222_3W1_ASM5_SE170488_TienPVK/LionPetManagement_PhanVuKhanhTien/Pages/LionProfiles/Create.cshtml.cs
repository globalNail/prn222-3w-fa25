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

namespace LionPetManagement_PhanVuKhanhTien.Pages.LionProfiles
{
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
            if (!ModelState.IsValid)
            {
                ViewData["LionTypeId"] = new SelectList(await _subService.GetAllAsync(), "LionTypeId", "LionTypeName");
                return Page();
            }

            await _service.CreateAsync(LionProfile);

            return RedirectToPage("./Index");
        }
    }
}
