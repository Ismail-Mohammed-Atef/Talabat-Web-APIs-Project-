﻿using AdminDashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;

namespace AdminDashboard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(u => new UserViewModel()
            {
                Id = u.Id,
                UserName = u.UserName,
                DisplayName = u.DisplayName,
                Email = u.Email,
                Roles = _userManager.GetRolesAsync(u).Result,
            }).ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRoleViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                if (!userRoles.Any(r => r == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
