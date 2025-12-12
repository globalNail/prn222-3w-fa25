using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SCMS.Domain.TienPVK.Models;

namespace SCMS.RazorWebApp.TienPVK.Pages.ClubsTienPvks
{
    public class EditModel : PageModel
    {
        private readonly ClubsTienPvkService _service;
        private readonly ClubCategoriesTienPvkService _categoryService;

        public EditModel(ClubsTienPvkService service,
            ClubCategoriesTienPvkService categoryService
        ){
            _service = service;
            _categoryService = categoryService;
        }

        [BindProperty]
        public ClubsTienPvk ClubsTienPvk { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubstienpvk =  await _service.GetByIdAsync(id.Value);
            if (clubstienpvk == null)
            {
                return NotFound();
            }
            ClubsTienPvk = clubstienpvk;
            var categoryList = await _categoryService.GetAllAsync();
            ViewData["CategoryIdtienPvk"] = new SelectList(categoryList, "CategoryIdtienPvk", "CategoryCode");
           //ViewData["ManagerUserId"] = new SelectList(_service.SystemAccounts, "UserAccountId", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ClubsTienPvk.ModifiedAt = DateTime.Now;
            ModelState.Remove("ClubsTienPvk.Category");
            ModelState.Remove("ClubsTienPvk.ManagerUser");
            ModelState.Remove("ClubsTienPvk.Activities");
            ModelState.Remove("ClubsTienPvk.AttendanceSessions");
            ModelState.Remove("ClubsTienPvk.ClubFeePolicies");
            ModelState.Remove("ClubsTienPvk.DisciplinaryCases");
            ModelState.Remove("ClubsTienPvk.FeeInvoices");
            ModelState.Remove("ClubsTienPvk.JoinRequests");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _service.UpdateAsync(ClubsTienPvk);

                if (result > 0)
                {
                    return RedirectToPage("./Index");

                }

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubsTienPvkExists(ClubsTienPvk.ClubIdtienPvk))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Page();
        }

        private bool ClubsTienPvkExists(int id)
        {
            return _service.AnyAsync(id).Result;
        }
    }
}
