namespace AbsenceManagementSystem.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AbsenceManagementSystem.DTOs.Children;
    using AbsenceManagementSystem.Models;
    using AbsenceManagementSystem.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ChildController : Controller
    {
        private readonly IChildService _childService;
        private readonly IGroupService _groupService;

        public ChildController(IChildService childService, IGroupService groupService)
        {
            this._childService = childService;
            this._groupService = groupService;
        }

        /// <summary>
        /// Creates GET request to visualise all children in the kindergarden.
        /// </summary>
        /// <returns></returns>

        [HttpGet("Children")]
        public async Task<IActionResult> Index()
        {
            return View(await _childService.GetAllAsync());
        }

        /// <summary>
        /// Get details about selected child
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("Child/Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var child = await _childService.GetByIdAsync(id);

                if (child == null)
                {
                    return NotFound();
                }

                return View(child);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets child with its data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>


        [HttpGet("Child/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var child = await _childService.GetByIdAsync(id);

                if (child == null)
                {
                    return NotFound();
                }

                var dto = new ChildDto
                {
                    Id = child.Id,
                    FirstName = child.FirstName,
                    LastName = child.LastName,
                    GroupId = child.GroupId,
                    Groups = await _childService.GetGroupsAsync()
                };

                return View("Form", dto);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Edits a child with new values.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>

        [ValidateAntiForgeryToken]
        [HttpPost("Child/Edit/{id:int}")]
        public async Task<IActionResult> Edit([FromRoute] int id, ChildDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _childService.GetByIdAsync(id) == null)
                    {
                        return NotFound();
                    }


                    await _childService.UpdateAsync(new Child
                    {
                        Id = id,
                        FirstName = dto.FirstName.Trim(),
                        LastName = dto.LastName.Trim(),
                        GroupId = dto.GroupId
                    });


                    TempData["Success"] = "Child updated successfully.";
                    return RedirectToAction(nameof(Details), new { id });
                }
                catch (InvalidOperationException)
                {
                    return NotFound();
                }
            }

            dto.Groups = await _childService.GetGroupsAsync();
            return View("Form", dto);
        }

        /// <summary>
        /// Opens a form for adding a child.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("Child/Add")]
        public async Task<IActionResult> Add(int? groupId)
        {
            return View("Form", new ChildDto
            {
                GroupId = groupId ?? 0,
                Groups = await _childService.GetGroupsAsync()
            });
        }

        /// <summary>
        /// Adds a child to a group.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        [ValidateAntiForgeryToken]
        [HttpPost("Child/Add")]
        public async Task<IActionResult> Add(ChildDto dto)
        {
            dto.Id = 0;
            if (ModelState.IsValid)
            {
                await _childService.AddAsync(new Child
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    GroupId = dto.GroupId
                });

                TempData["Success"] = "Child added successfully!";
                return RedirectToAction(nameof(Index));
            }

            dto.Groups = await _childService.GetGroupsAsync();
            return View("Form", dto);
        }

        /// <summary>
        /// Creates GET request to show form for deleting
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("Child/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return View(await _childService.GetByIdAsync(id));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates POST request that will delete the child by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _childService.DeleteAsync(id);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            TempData["Success"] = "Child deleted.";

            return RedirectToAction(nameof(Index));
        }
    }
}

