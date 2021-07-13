/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     10-15-2020
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Enter_The_Matrix.Models;
using Enter_The_Matrix.Services;
using System.ServiceModel.Security;

namespace Enter_The_Matrix.Controllers
{
    public class SecurityController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly UsersService _usersService;

        public SecurityController(IAuthenticationService ldapService, UsersService usersService)
        {
            _authService = ldapService;
            _usersService = usersService;
        }

        [HttpPost]
        public async Task<IActionResult> Admin(string username, string password)
        {
            User user = new User();

            user = _usersService.AdminLogin(username, password);

            if (user != null)
            {
                // create login token
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim("Operator", user.UserName),
                    new Claim("DisplayName", user.DisplayName),
                    new Claim("GivenName", user.GivenName)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("AdminDashboard", "AdminDashboard");
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl = "")
        {
            var model = new Account { returnUrl = ReturnUrl };

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Main", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(Account model, string method)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            User user = new User();

            if (method == "ldap")
            {
                user = _authService.Login(model.userName, model.password);
            }
            else if (method == "admin")
            {
                return await Admin(model.userName, model.password);
                //return RedirectToAction("Admin", "Security", new { username = model.userName, password = model.password});
            }
            else
            {
                user = _usersService.AuthUser(model.userName, model.password).Result;
            }

            if (null != user)
            {
                // create login token
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Guid.NewGuid().ToString()),
                    new Claim("Operator", model.userName),
                    new Claim("DisplayName", user.DisplayName),
                    new Claim("GivenName", user.GivenName)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Main", "Home");
            }
            else
            {
                return Unauthorized();
            }
        }

        public async Task<IActionResult> Logout()
        {
            var authProperties = new AuthenticationProperties();
            
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                authProperties);

            return RedirectToAction("Login", "Security");
        }
    }
}