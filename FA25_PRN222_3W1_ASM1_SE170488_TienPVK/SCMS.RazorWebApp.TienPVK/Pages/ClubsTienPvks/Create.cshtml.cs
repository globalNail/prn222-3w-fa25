using Microsoft.AspNetCore.Mvc.Rendering;

namespace SCMS.RazorWebApp.TienPVK.Pages.ClubsTienPvks
{
    public class CreateModel : PageModel
    {
        private readonly ClubsTienPvkService _clubService;
        private readonly ClubCategoriesTienPvkService _categoryService;
        private readonly IHubContext<ClubHub> _hubContext;

        public CreateModel(ClubsTienPvkService clubService, ClubCategoriesTienPvkService categoryService, IHubContext<ClubHub> hubContext)
        {
            _clubService = clubService;
            _categoryService = categoryService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGet()
        {
            var category = await _categoryService.GetAllAsync();
            ViewData["CategoryIdtienPvk"] = new SelectList(category, "CategoryIdtienPvk", "CategoryCode");
            //var account = await _accountService.GetAllSync();
            //ViewData["ManagerUserId"] = new SelectList(_clubService.SystemAccounts, "UserAccountId", "Email");

            //Set default value
            if (ClubsTienPvk == null)
            {
                ClubsTienPvk = new ClubsTienPvk()
                {

                    ClubName = "",
                    Description = "",
                    Status = "Active",
                };
            }

            return Page();
        }

        [BindProperty]
        public ClubsTienPvk ClubsTienPvk { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Set system-generated values
            ClubsTienPvk.CreatedAt = DateTime.Now;
            ClubsTienPvk.IsDeleted = false;

            // Remove navigation property validation errors
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
                var category = await _categoryService.GetAllAsync();
                ViewData["CategoryIdtienPvk"] = new SelectList(category, "CategoryIdtienPvk", "CategoryCode");
                TempData["ErrorMessage"] = "Please fix the validation errors before submitting.";
                return Page();
            }

            try
            {
                var result = await _clubService.CreateAsync(ClubsTienPvk);
                if (result > 0)
                {
                    // Broadcast create event to all connected clients
                    await _hubContext.Clients.All.SendAsync("CreateClub", result.ToString());
                    TempData["SuccessMessage"] = $"Club '{ClubsTienPvk.ClubName}' created successfully!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create club. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            var categoryList = await _categoryService.GetAllAsync();
            ViewData["CategoryIdtienPvk"] = new SelectList(categoryList, "CategoryIdtienPvk", "CategoryCode", ClubsTienPvk.CategoryIdtienPvk);

            return Page();
        }
    }
}
