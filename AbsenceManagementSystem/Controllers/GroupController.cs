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
        [HttpGet("/Group/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var details = await _groupService
                .GetByIdAsync(id);

            return View(details);
        }

        /// <summary>
        /// Creates a GET request to show the view, with the form
        /// </summary>
        /// <returns>Razor view</returns>
        [HttpGet("Group/Add")]
        public IActionResult Add()
        {
            return View();
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
        public async Task<IActionResult> AddGroup(Group group)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index", await _groupService.GetAllAsync());
                }

                await _groupService.AddAsync(group);
            }
            catch (InvalidOperationException ex)
                when(ex.Message == ErrorMessage.GroupWithThisNameAlreadyExists)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Group/Update/{id}")]
        public async Task<IActionResult> GetGroup(int id)
        {
            try
            {
                var group = await _groupService.GetByIdAsync(id);

                if(group == null)
                {
                    return NotFound();
                }

                return View("Update", group);

            }
            catch (InvalidOperationException ex)
                when (ex.Message == ErrorMessage.GroupDoesNotExist)
            {
                return NotFound();
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost("Group/Update/{id}")]
        public async Task<IActionResult> UpdateGroup(
            int id,
            [Bind("Id,Name,AgeGroup")] Group group)
        {
            if(group.Id != id)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return View("Update", group);
            }

            try
            {
                await _groupService.UpdateAsync(group);

                return RedirectToAction(nameof(Index));
            }

            catch (InvalidOperationException ex)
                when(ex.Message == ErrorMessage.GroupDoesNotExist)
            {
                return NotFound();
            }
        }
    }
}

