/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     08-23-2022
# Copyright:   (c) BLS OPS LLC. 2022
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
using Newtonsoft.Json;

namespace Enter_The_Matrix.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly ILogger<AdminDashboardController> _logger;
        private readonly UsersService _usersService;
        private readonly KeyService _keyService;

        public AdminDashboardController(
            ILogger<AdminDashboardController> logger,
            UsersService usersService,
            KeyService keyService)
        {
            _logger = logger;
            _usersService = usersService;
            _keyService = keyService;
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
            List<Key> keys = await _keyService.GetKeys();

            return View((users, keys));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string username, string password, string displayName, string givenName)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin")) { return RedirectToAction("Logout", "Security"); }

            await _usersService.AddUser(username, password, givenName, displayName);

            return RedirectToAction("AdminDashboard", "AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> CreateKey(string keyName, 
            List<string> assessmentPrivileges, 
            List<string> scenarioPrivileges,
            List<string> eventPrivileges,
            List<string> templatePrivileges,
            List<string> metricsPrivileges)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin")) { return RedirectToAction("Logout", "Security"); }

            string apiKey = await _keyService.AddKey(keyName, assessmentPrivileges, scenarioPrivileges, eventPrivileges, templatePrivileges, metricsPrivileges);

            return Content(JsonConvert.SerializeObject(apiKey));
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string username, string password, string displayName, string givenName)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin")) { return RedirectToAction("Logout", "Security"); }

            await _usersService.UpdateUser(username, password, displayName, givenName);

            return RedirectToAction("AdminDashboard", "AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin")) { return RedirectToAction("Logout", "Security"); }

            await _usersService.DeleteUser(username);

            return RedirectToAction("AdminDashboard", "AdminDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteKey(string keyName)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin")) { return RedirectToAction("Logout", "Security"); }

            await _keyService.DeleteKey(keyName);

            return RedirectToAction("AdminDashboard", "AdminDashboard");
        }
    }
}
