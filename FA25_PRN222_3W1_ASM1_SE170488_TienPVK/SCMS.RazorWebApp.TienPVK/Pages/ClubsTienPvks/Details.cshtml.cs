namespace SCMS.RazorWebApp.TienPVK.Pages.ClubsTienPvks
{
    public class DetailsModel : PageModel
    {
        private readonly ClubsTienPvkService _service;

        public DetailsModel(ClubsTienPvkService service)
        {
            _service = service;
        }

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

        public async Task<IActionResult> OnGetGetClubDataAsync(int id)
        {
            var club = await _service.GetByIdAsync(id);
            if (club == null)
                return NotFound();

            var data = new
            {
                clubId = club.ClubIdtienPvk,
                clubCode = club.ClubCode,
                clubName = club.ClubName,
                description = club.Description,
                email = club.Email,
                phone = club.Phone,
                address = club.Address,
                memberLimit = club.MemberLimit,
                status = club.Status,
                isOpenToJoin = club.IsOpenToJoin,
                requiresApproval = club.RequiresApproval,
                foundedDate = club.FoundedDate.ToString("dd MMM yyyy"),
                categoryCode = club.Category?.CategoryCode,
                managerEmail = club.ManagerUser?.Email
            };

            return new JsonResult(data);
        }
    }
}
