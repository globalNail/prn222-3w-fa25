using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK.DBContext;
using SCMS.Service.TienPVK.Implements;

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
                return NotFound();
            }

            var clubstienpvk = await _service.DeleteAsync(id.Value);

            if (clubstienpvk)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
