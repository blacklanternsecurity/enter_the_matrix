/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Updated:     08-23-2022
# Copyright:   (c) BLS OPS LLC. 2022
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
    [Route("api/Templates/[action]")]
    [ApiController]
    public class SteplatesController : ControllerBase
    {
        private readonly SteplatesService _steplatesService;

        public SteplatesController(SteplatesService service)
        {
            _steplatesService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Steplates>>> GetAll()
        {
            var steplates = await _steplatesService.GetAllAsync();
            return Ok(steplates);
        }

        [HttpGet]
        public async Task<ActionResult<Steplates>> GetById(string id)
        {
            var steplate = await _steplatesService.GetByIdAsync(id);
            if (steplate == null)
            {
                return NotFound();
            }
            return Ok(steplate);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Steplates steplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _steplatesService.CreateAsync(steplate);
            return Ok(steplate);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string id, Steplates steplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedSteplate = await _steplatesService.GetByIdAsync(id);
            if (queriedSteplate == null)
            {
                return NotFound();
            }

            await _steplatesService.UpdateAsync(id, steplate);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var steplate = await _steplatesService.GetByIdAsync(id);
            if (steplate == null)
            {
                return NotFound();
            }
            await _steplatesService.DeleteAsync(id);
            return NoContent();
        }
    }
}