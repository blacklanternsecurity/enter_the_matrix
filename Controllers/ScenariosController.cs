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
using System.Linq;

namespace Enter_The_Matrix.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScenariosController : ControllerBase
    {
        private readonly ScenariosService _scenariosService;
        private readonly StepsService _stepsService;

        public ScenariosController(ScenariosService scenariosService, StepsService stepsService)
        {
            _scenariosService = scenariosService;
            _stepsService = stepsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scenarios>>> GetAll()
        {
            var scenarios = await _scenariosService.GetAllAsync();
            return Ok(scenarios);
        }

        [HttpGet]
        public async Task<ActionResult<Scenarios>> GetById(string id)
        {
            Scenarios scenario;
            try
            {
                scenario = await _scenariosService.GetByIdAsync(id);
            }
            catch
            {
                return BadRequest();
            }
            if (scenario == null)
            {
                return NotFound("Scenario provided does not exist.");
            }
            return Ok(scenario);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Scenarios scenario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            scenario.Id = null;
            scenario.Steps = new string[] { };
            try
            {
                await _scenariosService.CreateAsync(scenario);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(scenario);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string id, Scenarios scenario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Check if scenario exists
            var queriedScenario = await _scenariosService.GetByIdAsync(id);
            if (queriedScenario == null)
            {
                return NotFound("The scenario supplied does not exist.");
            }

            // Check that there are no duplicated events
            if (scenario.Steps.Length != scenario.Steps.Distinct().ToArray<string>().Length)
            {
                return BadRequest("An event was submitted more than once.");
            }

            // Check if all events exist
            foreach (string eventId in scenario.Steps)
            {
                if (await _stepsService.GetByIdAsync(eventId) == null)
                {
                    return NotFound("One of the events supplied does not exist.");
                }
            }

            // Check that all events are unique to this scenario
            foreach (Scenarios s in await _scenariosService.GetAllAsync())
            {
                foreach (string eventId in scenario.Steps)
                {
                    if (s.Id == id) { continue; }
                    if (s.Steps.Contains(eventId))
                    {
                        return BadRequest("One of the events supplied is associated with another scenario.");
                    }
                }
            }

            try
            {
                await _scenariosService.UpdateAsync(id, scenario);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(await _scenariosService.GetByIdAsync(id));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            // Check if scenario exists
            Scenarios scenario;
            try
            {
                scenario = await _scenariosService.GetByIdAsync(id);
            }
            catch
            {
                return BadRequest();
            }
            if (scenario == null)
            {
                return NotFound("The scenario supplied does not exist.");
            }

            // Clean up orphans
            foreach (string eventId in scenario.Steps)
            {
                try
                {
                    await _stepsService.DeleteAsync(eventId);
                }
                catch
                {
                    return BadRequest();
                }
            }

            try
            {
                await _scenariosService.DeleteAsync(id);
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}