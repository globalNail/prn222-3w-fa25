using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SCMS.Domain.TienPVK.Models;

namespace SCMS.RazorWebApp.TienPVK.Pages.ClubsTienPvks
{
    public class EditModel : PageModel
    {
        private readonly SCMS.Repository.TienPVK.DBContext.FA25_PRN222_3W_PRN222_01_G5_SCMSDbContext _context;

        public EditModel(SCMS.Repository.TienPVK.DBContext.FA25_PRN222_3W_PRN222_01_G5_SCMSDbContext context)
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

            var clubstienpvk =  await _context.ClubsTienPvks.FirstOrDefaultAsync(m => m.ClubIdtienPvk == id);
            if (clubstienpvk == null)
            {
                return NotFound();
            }
            ClubsTienPvk = clubstienpvk;
           ViewData["CategoryIdtienPvk"] = new SelectList(_context.ClubCategoriesTienPvks, "CategoryIdtienPvk", "CategoryCode");
           ViewData["ManagerUserId"] = new SelectList(_context.SystemAccounts, "UserAccountId", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ClubsTienPvk).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

            return RedirectToPage("./Index");
        }

        private bool ClubsTienPvkExists(int id)
        {
            return _context.ClubsTienPvks.Any(e => e.ClubIdtienPvk == id);
        }
    }
}
