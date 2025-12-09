using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SCMS.Domain.TienPVK.Models;
using SCMS.Repository.TienPVK.DBContext;
using SCMS.Service.TienPVK.Implements;

namespace SCMS.RazorWebApp.TienPVK.Pages.ClubsTienPvks
{
    public class CreateModel : PageModel
    {
        private readonly ClubsTienPvkService _clubService;
        private readonly ClubCategoriesTienPvkService _categoryService;

        public CreateModel(ClubsTienPvkService clubService, ClubCategoriesTienPvkService categoryService)
        {
            _clubService = clubService;
            _categoryService = categoryService;
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
            ModelState.Remove("ClubsTienPvk.CategoryIdtienPvkNavigation");
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
                return Page();
            }

            var result = await _clubService.CreateAsync(ClubsTienPvk);
            if (result > 0)
            {
                return RedirectToPage("./Index");
            }

            var categoryList = await _categoryService.GetAllAsync();
            ViewData["CategoryIdtienPvk"] = new SelectList(categoryList, "CategoryIdtienPvk", "CategoryCode", ClubsTienPvk.CategoryIdtienPvk);

            return Page();
        }
    }
}
