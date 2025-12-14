namespace SCMS.RazorWebApp.TienPVK.Pages.ClubsTienPvks
{
    public class DeleteModel : PageModel
    {
        private readonly ClubsTienPvkService _service;

        public DeleteModel(ClubsTienPvkService service)
        {
            _service = service;
        }

        [BindProperty]
        public ClubsTienPvk ClubsTienPvk { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubstienpvk = await _service.GetByIdAsync(id.Value);

            if (clubstienpvk == null)
            {
                return NotFound();
            }
            else
            {
                ClubsTienPvk = clubstienpvk;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Invalid club ID.";
                return NotFound();
            }

            try
            {
                var club = await _service.GetByIdAsync(id.Value);
                if (club == null)
                {
                    TempData["ErrorMessage"] = "Club not found.";
                    return NotFound();
                }

                var clubName = club.ClubName;
                var result = await _service.DeleteAsync(id.Value);

                if (result)
                {
                    TempData["SuccessMessage"] = $"Club '{clubName}' deleted successfully!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete club. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            return Page();
        }
    }
}
