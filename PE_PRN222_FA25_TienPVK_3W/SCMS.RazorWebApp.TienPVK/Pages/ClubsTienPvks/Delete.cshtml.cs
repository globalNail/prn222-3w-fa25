using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK.DBContext;

namespace SCMS.RazorWebApp.TienPVK.Pages.ClubsTienPvks
{
    public class DeleteModel : PageModel
    {
        private readonly SCMS.Repository.TienPVK.DBContext.FA25_PRN222_3W_PRN222_01_G5_SCMSDbContext _context;

        public DeleteModel(SCMS.Repository.TienPVK.DBContext.FA25_PRN222_3W_PRN222_01_G5_SCMSDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClubsTienPvk ClubsTienPvk { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubstienpvk = await _context.ClubsTienPvks.FirstOrDefaultAsync(m => m.ClubIdtienPvk == id);

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

            var clubstienpvk = await _context.ClubsTienPvks.FindAsync(id);
            if (clubstienpvk != null)
            {
                ClubsTienPvk = clubstienpvk;
                _context.ClubsTienPvks.Remove(ClubsTienPvk);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
