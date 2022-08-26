﻿/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Updated:     08-23-2022
# Copyright:   (c) BLS OPS LLC. 2022
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Microsoft.AspNetCore.Mvc;
using Enter_The_Matrix.Services;
using Enter_The_Matrix.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace Enter_The_Matrix.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AssessmentsController : ControllerBase
    {
        private readonly AssessmentsService _assessmentsService;

        public AssessmentsController(AssessmentsService service)
        {
            _assessmentsService = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assessments>>> GetAll()
        {
            var assessments = await _assessmentsService.GetAllAsync();
            return Ok(assessments);
        }

        public async Task<ActionResult<Assessments>> GetById(string id)
        {
            var assessment = await _assessmentsService.GetByIdAsync(id);
            if (assessment == null)
            {
                return NotFound();
            }
            return Ok(assessment);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Assessments assessment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _assessmentsService.CreateAsync(assessment);
            return Ok(assessment);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string id, Assessments assessment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queriedAssessment = await _assessmentsService.GetByIdAsync(id);
            if (queriedAssessment == null)
            {
                return NotFound();
            }

            await _assessmentsService.UpdateAsync(id, assessment);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var assessment = await _assessmentsService.GetByIdAsync(id);
            if (assessment == null)
            {
                return NotFound();
            }
            await _assessmentsService.DeleteAsync(id);
            return NoContent();
        }
    }
}