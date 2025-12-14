using Microsoft.AspNetCore.Mvc;
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
            if (!ModelState.IsValid)
            {
                await LoadClubCategory(model);
                return View(model);
            }

            var newId = await _mainService.CreateAsync(model);
            if (newId <= 0)
            {
                ModelState.AddModelError(string.Empty, "Không thể tạo phiên điểm danh. Vui lòng thử lại.");
                await LoadClubCategory(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
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

            if (!ModelState.IsValid)
            {
                await LoadClubCategory(model);
                return View(model);
            }

            // Set modified fields
            model.ModifiedAt = DateTime.Now;
            model.ModifiedBy = User.Identity?.Name ?? "system";

            var rows = await _mainService.UpdateAsync(model);
            if (rows <= 0)
            {
                ModelState.AddModelError(string.Empty, "Cập nhật không thành công. Vui lòng thử lại.");
                await LoadClubCategory(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var obj = await _mainService.GetByIdAsync(id);
            if (obj == null) return NotFound();

            var result = await _mainService.DeleteAsync(id);
            if (!result)
            {
                TempData["Error"] = "Xóa không thành công. Vui lòng thử lại.";
                return RedirectToAction(nameof(Delete), new { id });
            }

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
