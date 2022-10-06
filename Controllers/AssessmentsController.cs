/*
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
        private readonly ScenariosService _scenariosService;
        private readonly StepsService _stepsService;
        private readonly KeyService _keyService;
        private const string APIKEY = "X-Api-Key";

        public AssessmentsController(
            AssessmentsService assessmentService, 
            ScenariosService scenarioService,
            StepsService stepsService,
            KeyService keyService)
        {
            _assessmentsService = assessmentService;
            _scenariosService = scenarioService;
            _stepsService = stepsService;
            _keyService = keyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assessments>>> GetAll()
        {
            var keyIn = HttpContext.Request.Headers[APIKEY];
            string authorizedFor = await _keyService.ValidateAssessment(keyIn);
            if (authorizedFor == "")
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }

            if (authorizedFor == "*")
            {
                var assessments = await _assessmentsService.GetAllAsync();
                return Ok(assessments);
            }
            else
            {
                try
                {
                    var assessment = await _assessmentsService.GetByIdAsync(authorizedFor);
                    List<Assessments> retList = new List<Assessments>();
                    retList.Add(assessment);
                    return Ok(retList);
                }
                catch
                {
                    return BadRequest();
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult<Assessments>> GetById(string id)
        {
            var keyIn = HttpContext.Request.Headers[APIKEY];
            string authorizedFor = await _keyService.ValidateAssessment(keyIn);
            if (authorizedFor == "")
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }

            if (authorizedFor == id || authorizedFor == "*")
            {
                Assessments assessment;
                try
                {
                    assessment = await _assessmentsService.GetByIdAsync(id);
                }
                catch
                {
                    return BadRequest();
                }
                if (assessment == null)
                {
                    return NotFound("The assessment provided does not exist.");
                }
                return Ok(assessment);
            }
            else
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Assessments assessment)
        {
            var keyIn = HttpContext.Request.Headers[APIKEY];
            string authorizedFor = await _keyService.ValidateAssessment(keyIn);
            if (authorizedFor != "*")
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("The model supplied is invalid.");
            }
            assessment.Id = null;
            assessment.Scenarios = new string[] { };
            assessment.ThreatTreeId = "";
            try
            {
                await _assessmentsService.CreateAsync(assessment);
            }
            catch
            {
                return BadRequest();
            }

            
            string assessmentApiKey = await _keyService.AddKey(
                 assessment.Name,
                 new List<string> { "C", "R", "U", "D" },
                 new List<string> { "C", "R", "U", "D" },
                 new List<string> { "C", "R", "U", "D" },
                 new List<string> { },
                 new List<string> { },
                 assessment.Id
            );

            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret["id"] = assessment.Id;
            ret["name"] = assessment.Name;
            ret["date"] = assessment.Date;
            ret["createdBy"] = assessment.CreatedBy;
            ret["scenarios"] = assessment.Scenarios;
            ret["threatTreeId"] = assessment.ThreatTreeId;
            ret["apiKey"] = assessmentApiKey;

            return Ok(ret);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string id, [FromBody] Assessments assessment)
        {

            var keyIn = HttpContext.Request.Headers[APIKEY];
            string authorizedFor = await _keyService.ValidateAssessment(keyIn);
            if (authorizedFor == "")
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }

            if (authorizedFor == id || authorizedFor == "*")
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("The model supplied is invalid.");
                }

                // Check if assessment exists
                var queriedAssessment = await _assessmentsService.GetByIdAsync(id);
                if (queriedAssessment == null)
                {
                    return NotFound("The assessment supplied was not found.");
                }

                // Make sure that scenarios are unique (no duplicates)
                if (assessment.Scenarios.Length != assessment.Scenarios.Distinct().ToArray<string>().Length)
                {
                    return BadRequest("A scenario was supplied more than once.");
                }

                // Check if all scenarios exist
                foreach (string scenarioId in assessment.Scenarios)
                {
                    if (await _scenariosService.GetByIdAsync(scenarioId) == null)
                    {
                        return NotFound("One of the scenarios supplied was not found.");
                    }
                }

                // Check that no scenario is already consumed by another assessment
                List<Assessments> assessmentList = await _assessmentsService.GetAllAsync();
                foreach (string scenarioId in assessment.Scenarios)
                {
                    foreach (Assessments a in assessmentList)
                    {
                        if (a.Id == id) { continue; }
                        if (a.Scenarios.Contains(scenarioId)) {
                            return BadRequest("One of the scenarios supplied is already associated with another assessment.");
                        }
                    }
                }


                try
                {
                    await _assessmentsService.UpdateAsync(id, assessment);
                }
                catch
                {
                    return BadRequest();
                }

                return Ok(await _assessmentsService.GetByIdAsync(id));
            }
            else
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var keyIn = HttpContext.Request.Headers[APIKEY];
            string authorizedFor = await _keyService.ValidateAssessment(keyIn);
            if (authorizedFor == "")
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }

            if (authorizedFor == id || authorizedFor == "*")
            {
                // Check if the assessment exists
                var assessment = await _assessmentsService.GetByIdAsync(id);
                if (assessment == null)
                {
                    return NotFound("The assessment provided does not exist.");
                }

                // Take care of orphaned records
                foreach (string scenarioId in assessment.Scenarios)
                {
                    Scenarios scenario = await _scenariosService.GetByIdAsync(scenarioId);
                    if (scenario != null)
                    {
                        foreach (string stepId in scenario.Steps)
                        {
                            Steps step = await _stepsService.GetByIdAsync(stepId);
                            if (step != null)
                            {
                                await _stepsService.DeleteAsync(step.Id);
                            }
                        }
                        await _scenariosService.DeleteAsync(scenario.Id);
                    }
                }
                try
                {
                    await _assessmentsService.DeleteAsync(id);
                }
                catch
                {
                    return BadRequest();
                }
                return Ok();
            }
            else
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }
        }
    }
}