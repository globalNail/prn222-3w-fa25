using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Entity.Models;
using Repository.DBContext;
using Service;

namespace ChargingManagement_PhanVuKhanhTien.Pages.ChargingSessions
{
    public class CreateModel : PageModel
    {
        private readonly ChargingSessionService _mainService;
        private readonly ChargingStationService _subService;
        private readonly SystemUserService _userService;

        public CreateModel(ChargingSessionService mainService
            , ChargingStationService subService
            , SystemUserService userService
            )
        {
            _mainService = mainService;
            _subService = subService;
            _userService = userService;
        }

        public async Task<IActionResult> OnGet()
        {
            var subList = await _subService.GetAllAsync();
            var userList = await _userService.GetAllAsync();
            ViewData["DriverId"] = new SelectList(userList, "UserId", "Username");
            ViewData["StationId"] = new SelectList(subList, "StationId", "StationName");
            return Page();
        }

        [BindProperty]
        public ChargingSession ChargingSession { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var subList = await _subService.GetAllAsync();
                ViewData["StationId"] = new SelectList(subList, "StationId", "StationName");
                return Page();
            }

            var result = await _mainService.CreateAsync(ChargingSession);
            if (result > 0)
            {
                return RedirectToPage("./Index");
            }

            var categoryList = await _mainService.GetAllAsync();
            ViewData["CategoryIdtienPvk"] = new SelectList(categoryList, "StationId", "StationName", ChargingSession.StationId);

            return Page();
        }
    }
}
