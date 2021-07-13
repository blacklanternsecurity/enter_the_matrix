/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     10-15-2020
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Enter_The_Matrix.Services;
using Enter_The_Matrix.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Enter_The_Matrix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScenariosController : ControllerBase
    {
        private readonly ScenariosService _scenariosService;

        public ScenariosController(ScenariosService service)
        {
            _scenariosService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scenarios>>> GetAll()
        {
            if (!User.Identity.IsAuthenticated) { return Unauthorized(); }
            var scenarios = await _scenariosService.GetAllAsync();
            return Ok(scenarios);
        }

        public async Task<ActionResult<Scenarios>> GetById(string id)
        {
            if (!User.Identity.IsAuthenticated) { return Unauthorized(); }
            var scenario = await _scenariosService.GetByIdAsync(id);
            if (scenario == null)
            {
                return NotFound();
            }
            return Ok(scenario);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Scenarios scenario)
        {
            if (!User.Identity.IsAuthenticated) { return Unauthorized(); }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _scenariosService.CreateAsync(scenario);
            return Ok(scenario);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string id, Scenarios scenario)
        {
            if (!User.Identity.IsAuthenticated) { return Unauthorized(); }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedScenario = await _scenariosService.GetByIdAsync(id);
            if (queriedScenario == null)
            {
                return NotFound();
            }

            await _scenariosService.UpdateAsync(id, scenario);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (!User.Identity.IsAuthenticated) { return Unauthorized(); }
            var scenario = await _scenariosService.GetByIdAsync(id);
            if (scenario == null)
            {
                return NotFound();
            }
            await _scenariosService.DeleteAsync(id);
            return NoContent();
        }
    }
}