using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Entity.Models;
using Repository.DBContext;
using Service;

namespace ChargingManagement_PhanVuKhanhTien.Pages.ChargingSessions
{
    public class EditModel : PageModel
    {
        private readonly ChargingSessionService _mainService;
        private readonly ChargingStationService _subService;
        private readonly SystemUserService _userService;

        public EditModel(ChargingSessionService mainService
            ,ChargingStationService subService
            ,SystemUserService userService
            )
        {
            _mainService = mainService;
            _subService = subService;
            _userService = userService;
        }

        [BindProperty]
        public ChargingSession ChargingSession { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chargingsession = await _mainService.GetByIdAsync(id.Value);
            if (chargingsession == null)
            {
                return NotFound();
            }
            ChargingSession = chargingsession;
            var subList = await _subService.GetAllAsync();
            var userList = await _userService.GetAllAsync();
            ViewData["DriverId"] = new SelectList(userList, "UserId", "Username");
            ViewData["StationId"] = new SelectList(subList, "StationId", "StationName");
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

            try
            {
                var result = await _mainService.UpdateAsync(ChargingSession);

                if (result > 0)
                {
                    return RedirectToPage("./Index");

                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChargingSessionExists(ChargingSession.SessionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Page();
        }

        private bool ChargingSessionExists(int id)
        {
            return _mainService.AnyAsync(id).Result;
        }
    }
}
