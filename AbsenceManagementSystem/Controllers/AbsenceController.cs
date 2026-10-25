namespace AbsenceManagementSystem.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AbsenceManagementSystem.DTOs.Absences;
    using AbsenceManagementSystem.Models;
    using AbsenceManagementSystem.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AbsenceController : Controller
    {
        private readonly IAbsenceService _service;
        private readonly IChildService _childService;

        public AbsenceController(IAbsenceService service, IChildService childService)
        {
            _service = service;
            _childService = childService;
        }

        public async Task<IActionResult> Index(int? childId)
        {
            var items = childId.HasValue ? await _service.GetByChildIdAsync(childId.Value) : await _service.GetAllAsync();
            ViewBag.ChildId = childId;
            return View(items);
        }

        public async Task<IActionResult> Add(int? childId)
        {
            await LoadChildren();
            return View("Form", new AbsenceDto { ChildId = childId ?? 0 });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            await LoadChildren();
            return View("Form", new AbsenceDto { Id = id, ChildId = item.ChildId, Date = item.Date, Type = item.Type, Note = item.Note });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AbsenceDto dto)
        {
            dto.Id = 0;
            return await Save(dto, false);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, AbsenceDto dto)
        {
            if (id != dto.Id) return BadRequest();
            if (await _service.GetByIdAsync(id) == null) return NotFound();
            return await Save(dto, true);
        }

        private async Task<IActionResult> Save(AbsenceDto dto, bool update)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var item = new Absence { Id = dto.Id, ChildId = dto.ChildId, Date = dto.Date!.Value, Type = dto.Type!.Value, Note = dto.Note?.Trim() };
                    if (update) await _service.UpdateAsync(item);
                    else await _service.AddAsync(item);
                    TempData["Success"] = "Absence saved successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex) { ModelState.AddModelError(string.Empty, ex.Message); }
                catch (ArgumentException ex) { ModelState.AddModelError(string.Empty, ex.Message); }
                catch (KeyNotFoundException) { return NotFound(); }
            }
            await LoadChildren();
            return View("Form", dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _service.DeleteAsync(id)) return NotFound();
            TempData["Success"] = "Absence deleted.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadChildren()
        {
            var children = await _childService.GetAllAsync();
            ViewBag.Children = new SelectList(children.Select(c => new { c.Id, Name = c.FirstName + " " + c.LastName + " — " + c.Group.Name }), "Id", "Name");
        }
    }
}

