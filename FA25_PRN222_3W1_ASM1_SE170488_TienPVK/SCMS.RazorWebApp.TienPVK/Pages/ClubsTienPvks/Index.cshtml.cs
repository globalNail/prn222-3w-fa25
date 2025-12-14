namespace SCMS.RazorWebApp.TienPVK.Pages.ClubsTienPvks
{
    public class IndexModel : PageModel
    {
        private readonly ClubsTienPvkService _service;
        private const int PageSize = 9;

        public IndexModel(ClubsTienPvkService service) => _service = service;

        public IList<ClubsTienPvk> ClubsTienPvk { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public async Task OnGetAsync()
        {
            var allClubs = await _service.GetAllAsync();
            TotalItems = allClubs.Count;
            TotalPages = (TotalItems + PageSize - 1) / PageSize;

            // Ensure PageNumber is valid
            if (PageNumber < 1)
                PageNumber = 1;
            if (PageNumber > TotalPages && TotalPages > 0)
                PageNumber = TotalPages;

            // Apply paging
            ClubsTienPvk = allClubs
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public async Task<IActionResult> OnGetGetClubCardAsync(int clubId)
        {
            var club = await _service.GetByIdAsync(clubId);
            if (club == null)
                return Content("");

            var statusClass = GetStatusClass(club.Status);
            var html = $@"
                <div class='col'>
                    <div class='card h-100 shadow-sm club-card' id='{club.ClubIdtienPvk}'>
                        <div class='card-body d-flex flex-column gap-3'>
                            <div class='d-flex justify-content-between align-items-start gap-3'>
                                <div>
                                    <span class='badge bg-primary-subtle text-primary-emphasis mb-2'>{club.Category?.CategoryCode ?? "General"}</span>
                                    <h2 class='h5 mb-1'>{club.ClubName}</h2>
                                    <p class='text-muted mb-0'>{club.ClubCode}</p>
                                </div>
                                <span class='status-pill {statusClass}'>{club.Status}</span>
                            </div>
                            <p class='text-body-secondary mb-0 club-description'>{club.Description}</p>
                            <div class='d-flex flex-column gap-2 small text-body-secondary'>
                                <div class='d-flex justify-content-between'>
                                    <span>Founded</span>
                                    <span>{club.FoundedDate.ToString("dd MMM yyyy")}</span>
                                </div>
                                <div class='d-flex justify-content-between'>
                                    <span>Member limit</span>
                                    <span>{club.MemberLimit}</span>
                                </div>
                                <div class='d-flex justify-content-between'>
                                    <span>Manager</span>
                                    <span>{club.ManagerUser?.Email ?? "—"}</span>
                                </div>
                                <div class='d-flex justify-content-between'>
                                    <span>Contact</span>
                                    <span>{club.Email}</span>
                                </div>
                            </div>
                            <div class='d-flex flex-wrap gap-2'>
                                {(club.IsOpenToJoin ? "<span class='badge rounded-pill bg-success-subtle text-success-emphasis'>Open to join</span>" : "<span class='badge rounded-pill bg-secondary-subtle text-secondary-emphasis'>Closed</span>")}
                                {(club.RequiresApproval ? "<span class='badge rounded-pill bg-warning-subtle text-warning-emphasis'>Approval required</span>" : "<span class='badge rounded-pill bg-info-subtle text-info-emphasis'>Auto enrollment</span>")}
                            </div>
                        </div>
                        <div class='card-footer bg-transparent border-0 pt-0'>
                            <div class='btn-group w-100' role='group'>
                                <a class='btn btn-outline-primary' href='/ClubsTienPvks/Details?id={club.ClubIdtienPvk}'>Details</a>
                                <a class='btn btn-outline-secondary' href='/ClubsTienPvks/Edit?id={club.ClubIdtienPvk}'>Edit</a>
                                <a class='btn btn-outline-danger' href='/ClubsTienPvks/Delete?id={club.ClubIdtienPvk}'>Delete</a>
                            </div>
                        </div>
                    </div>
                </div>";

            return Content(html, "text/html");
        }

        private static string GetStatusClass(string? status)
        {
            return status?.ToLower() switch
            {
                "active" => "status-pill-success",
                "inactive" => "status-pill-muted",
                "suspended" => "status-pill-warning",
                _ => "status-pill-muted"
            };
        }
    }
}