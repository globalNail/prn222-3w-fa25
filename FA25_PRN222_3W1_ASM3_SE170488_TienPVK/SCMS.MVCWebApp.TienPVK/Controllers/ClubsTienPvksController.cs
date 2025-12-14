using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCMS.Domain.TienPVK.Models;
using SCMS.Service.TienPVK.Implements;

namespace SCMS.MVCWebApp.TienPVK.Controllers
{
    public class ClubsTienPvksController : Controller
    {
        private readonly ClubsTienPvkService _mainService;
        private readonly ClubCategoriesTienPvkService _subService;

        public ClubsTienPvksController(
            ClubsTienPvkService mainService,
            ClubCategoriesTienPvkService subService)
        {
            _mainService = mainService;
            _subService = subService;
        }


        // GET: ClubTienPVKController
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var allItems = await _mainService.GetAllAsync();

            // Calculate pagination
            var totalItems = allItems.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Ensure page number is valid
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages > 0 ? totalPages : 1));

            var items = allItems
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Pass pagination data to view
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;

            return View(items);
        }

        // GET: ClubTienPVKController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var item = await _mainService.GetByIdAsync(id.Value);
            if (item == null) return NotFound();
            return View(item);
        }

        // GET: ClubTienPVKController/Create
        public async Task<IActionResult> Create()
        {
            await LoadClubCategory(null);
            return View();
        }

        // POST: ClubTienPVKController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClubsTienPvk model)
        {
            // Remove model state for navigation properties
            ModelState.Remove("Category");
            ModelState.Remove("ManagerUser");
            
            if (!ModelState.IsValid)
            {
                await LoadClubCategory(model);
                return View(model);
            }

            try
            {
                // Set default values
                model.CreatedAt = DateTime.Now;
                model.CreatedBy = User.Identity?.Name ?? "system";
                model.IsDeleted = false;
                
                // Clear navigation properties to avoid EF tracking issues
                model.Category = null;
                model.ManagerUser = null;
                // Initialize collections to avoid null reference issues
                model.Activities = new List<Activity>();
                model.AttendanceSessions = new List<AttendanceSession>();
                model.ClubFeePolicies = new List<ClubFeePolicy>();
                model.DisciplinaryCases = new List<DisciplinaryCase>();
                model.FeeInvoices = new List<FeeInvoice>();
                model.JoinRequests = new List<JoinRequest>();

                var newId = await _mainService.CreateAsync(model);
                if (newId <= 0)
                {
                    TempData["ErrorMessage"] = "Failed to create club. Please try again.";
                    await LoadClubCategory(model);
                    return View(model);
                }

                TempData["SuccessMessage"] = "Club created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Check for unique constraint violation
                var innerException = ex.InnerException?.Message ?? ex.Message;
                
                if (innerException.Contains("UNIQUE KEY") || innerException.Contains("duplicate key"))
                {
                    if (innerException.Contains(model.ClubCode))
                    {
                        TempData["ErrorMessage"] = $"Club code '{model.ClubCode}' already exists. Please use a different code.";
                        ModelState.AddModelError("ClubCode", "This club code already exists.");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "A club with this information already exists. Please check your input.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Database error occurred. Please try again.";
                }
                
                await LoadClubCategory(model);
                return View(model);
            }
            catch (Exception ex)
            {
                // Get the innermost exception message
                var innerException = ex;
                while (innerException.InnerException != null)
                {
                    innerException = innerException.InnerException;
                }
                
                TempData["ErrorMessage"] = $"Error: {innerException.Message}";
                await LoadClubCategory(model);
                return View(model);
            }
            finally
            {
                ModelState.Clear();
            }
        }

        // GET: ClubTienPVKController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var item = await _mainService.GetByIdAsync(id.Value);
            if (item == null) return NotFound();

            await LoadClubCategory(item);
            return View(item);
        }

        // POST: ClubTienPVKController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClubsTienPvk model)
        {
            if (id != model.ClubIdtienPvk) return BadRequest();

            // Remove model state for navigation properties
            ModelState.Remove("Category");
            ModelState.Remove("ManagerUser");
            
            if (!ModelState.IsValid)
            {
                await LoadClubCategory(model);
                return View(model);
            }

            try
            {
                // Set modified fields
                model.ModifiedAt = DateTime.Now;
                model.ModifiedBy = User.Identity?.Name ?? "system";
                
                // Clear navigation properties to avoid EF tracking issues
                model.Category = null;
                model.ManagerUser = null;

                var rows = await _mainService.UpdateAsync(model);
                if (rows <= 0)
                {
                    TempData["ErrorMessage"] = "Failed to update club. Please try again.";
                    await LoadClubCategory(model);
                    return View(model);
                }

                TempData["SuccessMessage"] = "Club updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                await LoadClubCategory(model);
                return View(model);
            }
        }

        // GET: ClubTienPVKController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var item = await _mainService.GetByIdAsync(id.Value);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: ClubTienPVKController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obj = await _mainService.GetByIdAsync(id);
            if (obj == null) return NotFound();

            var result = await _mainService.DeleteAsync(id);
            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to delete club. Please try again.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            TempData["SuccessMessage"] = "Club deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadClubCategory(ClubsTienPvk? clubCategory)
        {
            var categories = await _subService.GetAllAsync();

            ViewData["CategoryIdtienPvk"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                categories,
                "CategoryIdtienPvk",
                "CategoryName",
                clubCategory?.CategoryIdtienPvk);
        }
    }
}
