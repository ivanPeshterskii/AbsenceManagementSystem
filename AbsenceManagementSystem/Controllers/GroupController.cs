using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbsenceManagementSystem.Models;
using AbsenceManagementSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AbsenceManagementSystem.Controllers
{
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

        [HttpGet("Group/Create")]
        public IActionResult Add() 
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost("Group/Add")]
        public async Task<IActionResult> AddGroup(Group group)
        {
            if(!ModelState.IsValid)
            {
                return View("Index", await _groupService.GetAllAsync());
            }

            await _groupService.AddAsync(group);

            return RedirectToAction(nameof(Index));
        }
    }
}

