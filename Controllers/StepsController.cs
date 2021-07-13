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
    public class StepsController : ControllerBase
    {
        private readonly StepsService _stepsService;

        public StepsController(StepsService service)
        {
            _stepsService = service;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Steps>>> GetAll()
        {
            if (!User.Identity.IsAuthenticated) { return Unauthorized(); }
            var steps = await _stepsService.GetAllAsync();
            return Ok(steps);
        }

        public async Task<ActionResult<Steps>> GetById(string id)
        {
            if (!User.Identity.IsAuthenticated) { return Unauthorized(); }
            var step = await _stepsService.GetByIdAsync(id);
            if (step == null)
            {
                return NotFound();
            }
            return Ok(step);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Steps step)
        {
            if (!User.Identity.IsAuthenticated) { return Unauthorized(); }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _stepsService.CreateAsync(step);
            return Ok(step);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string id, Steps step)
        {
            if (!User.Identity.IsAuthenticated) { return Unauthorized(); }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedStep = await _stepsService.GetByIdAsync(id);
            if (queriedStep == null)
            {
                return NotFound();
            }

            await _stepsService.UpdateAsync(id, step);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (!User.Identity.IsAuthenticated) { return Unauthorized(); }
            var step = await _stepsService.GetByIdAsync(id);
            if (step == null)
            {
                return NotFound();
            }
            await _stepsService.DeleteAsync(id);
            return NoContent();
        }
    }
}