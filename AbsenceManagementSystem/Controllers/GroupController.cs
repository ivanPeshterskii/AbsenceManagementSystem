namespace AbsenceManagementSystem.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AbsenceManagementSystem.Models;
    using AbsenceManagementSystem.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using AbsenceManagementSystem.Common.ExceptionMessages;
    using AbsenceManagementSystem.DTOs.Groups;

    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            this._groupService = groupService;
        }

        /// <summary>
        /// GET request after that it shows all groups, their names, teachers and children
        /// </summary>
        /// <returns>View of groups (overview)</returns>

        [HttpGet("/Groups")]
        public async Task<IActionResult> Index()
        {
            var groups = await _groupService
                .GetAllAsync();

            return View(groups);
        }

        /// <summary>
        /// Gives more details about a selected group
        /// </summary>
        /// <param name="id">integer</param>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var details = await _groupService
                .GetByIdAsync(id);

            if (details == null)
            {
                return NotFound();
            }

            return View(details);
        }

        /// <summary>
        /// Creates a GET request to show the view, with the form
        /// </summary>
        /// <returns>Razor view</returns>

        [HttpGet("Group/Add")]
        public IActionResult Add()
        {
            return View(new CreateDto());
        }

        /// <summary>
        /// 1. Creates POST request to the server
        /// to create a new group after some validations;
        /// 2. Use 'AntiForgeryToken' to prevent from CSRF attack
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>

        [ValidateAntiForgeryToken]
        [HttpPost("Group/Add")]
        public async Task<IActionResult> AddGroup(CreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", dto);
            }

            try
            {
                await _groupService.AddAsync(dto);
            }
            catch (InvalidOperationException ex)
                when (ex.Message == ErrorMessage.GroupWithThisNameAlreadyExists)
            {
                ModelState.AddModelError(nameof(dto.Name), ex.Message);

                return View("Add", dto);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Creates GET request, redirects to update section 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("Group/Update/{id:int}")]
        public async Task<IActionResult> GetGroup(int id)
        {
            try
            {
                var group = await _groupService.GetByIdAsync(id);

                if (group == null)
                {
                    return NotFound();
                }

                var groupDto = new UpdateDto
                {
                    Id = group.Id,
                    Name = group.Name,
                    AgeGroup = group.AgeGroup
                };

                return View("Update", groupDto);

            }
            catch (InvalidOperationException ex)
                when (ex.Message == ErrorMessage.GroupDoesNotExist)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates POST request to update the selected group by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="group"></param>
        /// <returns></returns>

        [ValidateAntiForgeryToken]
        [HttpPost("Group/Update/{id:int}")]
        public async Task<IActionResult> UpdateGroup(
            int id,
            [Bind("Id,Name,AgeGroup")] UpdateDto group)
        {
            if (group.Id != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View("Update", group);
            }

            try
            {
                await _groupService.UpdateAsync(group);

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
                when (ex.Message == ErrorMessage.GroupWithThisNameAlreadyExists)
            {
                ModelState.AddModelError(nameof(group.Name), ex.Message);
                return View("Update", group);
            }
            catch(InvalidOperationException ex)
                when(ex.Message == ErrorMessage.GroupDoesNotExist)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates GET request, redirects to delete section 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("Group/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var group = await _groupService.GetByIdAsync(id);

                if (group == null)
                {
                    return NotFound();
                }

                return View(group);
            }
            catch (InvalidOperationException ex)
                when (ex.Message == ErrorMessage.GroupDoesNotExist)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Creates POST request to delete the selected group by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost("Group/Delete/{id:int}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _groupService.DeleteAsync(id);
            }
            catch (InvalidOperationException ex)
                when (ex.Message == ErrorMessage.GroupDoesNotExist)
            {
                return NotFound();
            }
            TempData["Success"] = "Group and related records deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}