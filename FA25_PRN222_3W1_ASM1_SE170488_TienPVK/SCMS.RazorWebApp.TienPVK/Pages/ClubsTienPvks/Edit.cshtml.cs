using Microsoft.AspNetCore.Mvc.Rendering;

namespace SCMS.RazorWebApp.TienPVK.Pages.ClubsTienPvks
{
    public class EditModel : PageModel
    {
        private readonly ClubsTienPvkService _service;
        private readonly ClubCategoriesTienPvkService _categoryService;
        private readonly IHubContext<ClubHub> _hubContext;

        public EditModel(ClubsTienPvkService service,
            ClubCategoriesTienPvkService categoryService,
            IHubContext<ClubHub> hubContext
        ){
            _service = service;
            _categoryService = categoryService;
            _hubContext = hubContext;
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
                var categoryList = await _categoryService.GetAllAsync();
                ViewData["CategoryIdtienPvk"] = new SelectList(categoryList, "CategoryIdtienPvk", "CategoryCode");
                TempData["ErrorMessage"] = "Please fix the validation errors before submitting.";
                return Page();
            }

            try
            {
                var result = await _service.UpdateAsync(ClubsTienPvk);

                if (result > 0)
                {
                    // Broadcast update event to all connected clients
                    await _hubContext.Clients.All.SendAsync("UpdateClub", ClubsTienPvk.ClubIdtienPvk.ToString());
                    TempData["SuccessMessage"] = $"Club '{ClubsTienPvk.ClubName}' updated successfully!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update club. Please try again.";
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubsTienPvkExists(ClubsTienPvk.ClubIdtienPvk))
                {
                    TempData["ErrorMessage"] = "Club not found. It may have been deleted.";
                    return NotFound();
                }
                else
                {
                    TempData["ErrorMessage"] = "A concurrency error occurred. Please try again.";
                    throw;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            var categories = await _categoryService.GetAllAsync();
            ViewData["CategoryIdtienPvk"] = new SelectList(categories, "CategoryIdtienPvk", "CategoryCode");
            return Page();
        }

        private bool ClubsTienPvkExists(int id)
        {
            return _service.AnyAsync(id).Result;
        }
    }
}
