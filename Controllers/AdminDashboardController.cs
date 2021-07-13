/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     10-15-2020
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Enter_The_Matrix.Services;
using Enter_The_Matrix.Models;

namespace Enter_The_Matrix.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly ILogger<AdminDashboardController> _logger;
        private readonly UsersService _usersService;

        public AdminDashboardController(
            ILogger<AdminDashboardController> logger,
            UsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> AdminDashboard()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Logout", "Security");
            }

            // Display the dashboard
            List<User> users = await _usersService.GetUsers();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string username, string password, string displayName, string givenName)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin")) { return Unauthorized(); }

            await _usersService.AddUser(username, password, givenName, displayName);

            return RedirectToAction("AdminDashboard", "AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string username, string password, string displayName, string givenName)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin")) { return Unauthorized(); }

            await _usersService.UpdateUser(username, password, displayName, givenName);

            return RedirectToAction("AdminDashboard", "AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin")) { return Unauthorized(); }

            await _usersService.DeleteUser(username);

            return RedirectToAction("AdminDashboard", "AdminDashboard");
        }
    }
}
