using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Service;

namespace LionPetManagement_PhanVuKhanhTien.Pages.LionProfiles
{
    [Authorize(Roles = ("3,2"))]
    public class SearchModel : PageModel
    {
        private readonly LionProfileService _service;

        public SearchModel(LionProfileService service) => _service = service;

        public IList<Entity.Models.LionProfile> LionProfile { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public double? weight { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? lionTypeName { get; set; }
        public async Task OnGetAsync()
        {
            LionProfile = await _service.Search(lionTypeName ?? "", (weight ?? 0));
        }
    }
}
