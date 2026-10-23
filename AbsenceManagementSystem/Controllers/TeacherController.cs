namespace AbsenceManagementSystem.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AbsenceManagementSystem.DTOs.Teachers;
    using AbsenceManagementSystem.Models;
    using AbsenceManagementSystem.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class TeacherController : Controller
    {
        private readonly ITeacherService _service;

        private readonly IGroupService _groupService;

        public TeacherController(ITeacherService service, IGroupService groupService)
        {
            this._service = service;
            this._groupService = groupService;
        }

        /// <summary>
        /// Creates a request to show all available teachers.
        /// </summary>
        /// <returns></returns>

        [HttpGet("Teachers")]
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        /// <summary>
        /// Get details about a group by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("Teacher/Details{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);

                if (item == null)
                {
                    return NotFound();
                }

                return View(item);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates GET request to show the add-form.
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>

        [HttpGet("Teacher/Add/{id:int?}")]
        public async Task<IActionResult> Add(int? groupId)
        {
            await LoadGroups();

            return View("Form", new TeacherDto { GroupId = groupId ?? 0 });
        }

        /// <summary>
        /// Creates POST request to add a teacher.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        [ValidateAntiForgeryToken]
        [HttpPost("Teacher/Add/{id:int}")]
        public async Task<IActionResult> AddTeacher(TeacherDto dto)
        {
            dto.Id = 0;
            if (ModelState.IsValid)
            {
                await _service.AddAsync(new Teacher
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    GroupId = dto.GroupId
                });

                TempData["Success"] = "Teacher added successfully!";
                return RedirectToAction(nameof(Index));
            }

            await LoadGroups();
            return View("Form", dto);
        }

        /// <summary>
        /// Creates GET request to show form for editing.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("Teacher/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);

                if (item == null)
                {
                    return NotFound();
                }

                await LoadGroups();

                return View("Form", new TeacherDto
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    GroupId = item.GroupId
                });
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates POST request to update the selected teacher
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>

        [ValidateAntiForgeryToken]
        [HttpPost("Teacher/Edit/{id:int}")]
        public async Task<IActionResult> EditTeacher([FromRoute] int id, TeacherDto dto)
        {
            if (id != dto.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (await _service.GetByIdAsync(id) == null) return NotFound();
                    await _service.UpdateAsync(new Teacher
                    {
                        Id = id,
                        FirstName = dto.FirstName.Trim(),
                        LastName = dto.LastName.Trim(),
                        GroupId = dto.GroupId
                    });
                    TempData["Success"] = "Teacher updated successfully.";
                    return RedirectToAction(nameof(Details), new { id });
                }
                catch (InvalidOperationException ex) { ModelState.AddModelError(string.Empty, ex.Message); }
            }
            await LoadGroups();
            return View("Form", dto);
        }

        /// <summary>
        /// Creates GET request to show form for deleting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("Teacher/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var item = await _service.GetByIdAsync(id);

                if (item == null)
                    return NotFound();

                return View(item);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates POST request that will delete the teacher by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            TempData["Success"] = "Teacher deleted.";

            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Loading groups async
        /// </summary>
        /// <returns></returns>
        private async Task LoadGroups()
        {
            ViewBag.Groups = new SelectList(await _groupService.GetAllAsync(), "Id", "Name");
        }
    }
}