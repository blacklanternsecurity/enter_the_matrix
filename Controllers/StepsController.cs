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
using Enter_The_Matrix.FactorModels;
using System.Linq;

namespace Enter_The_Matrix.Controllers
{
    [Route("api/Events/[action]")]
    [ApiController]
    public class StepsController : ControllerBase
    {
        private readonly StepsService _stepsService;
        private readonly ScenariosService _scenariosService;
        private readonly AssessmentsService _assessmentsService;
        private readonly KeyService _keyService;
        private const string APIKEY = "X-Api-Key";

        public StepsController(
            StepsService service, 
            ScenariosService scenariosService,
            AssessmentsService assessmentsService,
            KeyService keyService)
        {
            _stepsService = service;
            _scenariosService = scenariosService;
            _assessmentsService = assessmentsService;
            _keyService = keyService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Steps>>> GetAll()
        {
            var keyIn = HttpContext.Request.Headers[APIKEY];
            string authorizedFor = await _keyService.ValidateAssessment(keyIn);
            if (authorizedFor == "")
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }

            if (authorizedFor == "*")
            {
                var steps = await _stepsService.GetAllAsync();
                return Ok(steps);
            }
            else
            {
                try
                {
                    List<Steps> stepList = new List<Steps>();
                    var assessment = await _assessmentsService.GetByIdAsync(authorizedFor);
                    foreach (string scenarioId in assessment.Scenarios)
                    {
                        var scenario = await _scenariosService.GetByIdAsync(scenarioId);
                        foreach (string stepId in scenario.Steps)
                        {
                            stepList.Add(await _stepsService.GetByIdAsync(stepId));
                        }
                    }
                    return Ok(stepList);
                }
                catch
                {
                    return BadRequest();
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult<Steps>> GetById(string id)
        {
            var keyIn = HttpContext.Request.Headers[APIKEY];
            string authorizedFor = await _keyService.ValidateAssessment(keyIn);
            if (authorizedFor == "")
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }

            try
            {
                var scenario = await _scenariosService.GetByStepIdAsync(id);
                var assessment = await _assessmentsService.GetByScenarioIdAsync(scenario.Id);
                if (authorizedFor == "*" || authorizedFor == assessment.Id)
                {
                    // Check if event exists
                    Steps step;
                    try
                    {
                        step = await _stepsService.GetByIdAsync(id);
                    }
                    catch
                    {
                        return BadRequest();
                    }
                    if (step == null)
                    {
                        return NotFound("The event supplied does not exist.");
                    }

                    return Ok(step);
                }
                else
                {
                    return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Steps step)
        {
            var keyIn = HttpContext.Request.Headers[APIKEY];
            string authorizedFor = await _keyService.ValidateAssessment(keyIn);
            if (authorizedFor == "")
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("The model is invalid.");
            }
            step.Id = null;
            step.GraphNode = null;

            // Validate the data supplied
            if (step.Capability < 0 || step.Capability > 10)
            {
                return BadRequest("The capability supplied must be between 0 and 10.");
            }
            if (step.Intent < 0 || step.Intent > 10)
            {
                return BadRequest("The intent supplied must be between 0 and 10.");
            }
            if (step.Targeting < 0 || step.Targeting > 10)
            {
                return BadRequest("The targeting supplied must be between 0 and 10.");
            }
            if (step.Initiation < 0 || step.Initiation > 10)
            {
                return BadRequest("The initiation supplied must be between 0 and 10.");
            }
            if (step.Severity < 0 || step.Severity > 10)
            {
                return BadRequest("The severity supplied must be between 0 and 10.");
            }
            if (step.Pervasiveness < 0 || step.Pervasiveness > 10)
            {
                return BadRequest("The pervasiveness supplied must be between 0 and 10.");
            }
            if (step.Adverse < 0 || step.Adverse > 10)
            {
                return BadRequest("The adverse supplied must be between 0 and 10.");
            }
            if (step.Impact < 0 || step.Impact > 10)
            {
                return BadRequest("The impact supplied must be between 0 and 10.");
            }
            List<string> riskList = new List<string>();
            riskList.AddRange(new string[] { "Very Low", "Low", "Moderate", "High", "Very High" });
            if (step.Risk == null || step.Risk == "")
            {
                step.Risk = "Very Low";
            }
            else if (!riskList.Contains(step.Risk))
            {
                return BadRequest("The risk supplied is invalid. Must be one of: " + riskList.ToString());
            }

            List<string> relevanceList = new List<string>();
            relevanceList.AddRange(new string[] { "Confirmed", "Expected", "Anticipated", "Predicted", "Possible", "N/A" });
            if (step.Relevance == null || step.Relevance == "")
            {
                step.Relevance = "";
            }
            else if (!relevanceList.Contains(step.Relevance))
            {
                return BadRequest("The relevance supplied is invalid. Must be one of: " + relevanceList.ToString());
            }

            if (step.Likelihood == null || step.Likelihood == "") 
            { 
                step.Likelihood = "Very Low"; 
            }
            else if (!riskList.Contains(step.Likelihood))
            {
                return BadRequest("The likelihood supplied is invalid. Must be one of: " + riskList.ToString());
            }

            ThreatSources ts = new ThreatSources();
            if (step.ThreatSource == null || step.ThreatSource == "")
            {
                step.ThreatSource = "";
            }
            else if (!ts.sources.Contains(step.ThreatSource))
            {
                return BadRequest("The threat source supplied is invalid. Must be one of: " + ts.sources.ToString());
            }

            Techniques mitre = new Techniques("");
            if (step.MitreId == null || step.MitreId == "") { step.MitreId = ""; }
            else if (!mitre.techniquesFull.ContainsKey(step.MitreId))
            {
                return BadRequest("The MITRE ATT&CK ID supplied is invalid or was not found. Must be in the format of either T#### or T####/###");
            }

            if (step.Vulnerability == null || step.Vulnerability == "") { step.Vulnerability = ""; step.Severity = 0; }
            if (step.Condition == null || step.Condition == "") { step.Condition = ""; step.Pervasiveness = 0; }
            if (step.Mitigation == null) { step.Mitigation = ""; }
            
            if (step.Event == null || step.Event == "")
            {
                return BadRequest("Event should describe what is happening. Should not be empty or null.");
            }

            try
            {
                await _stepsService.CreateAsync(step);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(step);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string id, Steps step)
        {
            var keyIn = HttpContext.Request.Headers[APIKEY];
            string authorizedFor = await _keyService.ValidateAssessment(keyIn);
            if (authorizedFor == "")
            {
                return Unauthorized("You are not authorized to perform this action due to assessment level restrictions.");
            }

            var s = await _scenariosService.GetByStepIdAsync(id);
            var assessment = await _assessmentsService.GetByScenarioIdAsync(s.Id);
            
            if (authorizedFor == assessment.Id || authorizedFor == "*")
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Bad model received");
                }

                // Make sure the event exists
                Steps queriedStep;

                try
                {
                    queriedStep = await _stepsService.GetByIdAsync(id);
                }
                catch
                {
                    return BadRequest();
                }
                if (queriedStep == null)
                {
                    return NotFound("The event supplied was not found.");
                }

                // Validate the data supplied
                if (step.Capability < 0 || step.Capability > 10)
                {
                    return BadRequest("The capability supplied must be between 0 and 10.");
                }
                if (step.Intent < 0 || step.Intent > 10)
                {
                    return BadRequest("The intent supplied must be between 0 and 10.");
                }
                if (step.Targeting < 0 || step.Targeting > 10)
                {
                    return BadRequest("The targeting supplied must be between 0 and 10.");
                }
                if (step.Initiation < 0 || step.Initiation > 10)
                {
                    return BadRequest("The initiation supplied must be between 0 and 10.");
                }
                if (step.Severity < 0 || step.Severity > 10)
                {
                    return BadRequest("The severity supplied must be between 0 and 10.");
                }
                if (step.Pervasiveness < 0 || step.Pervasiveness > 10)
                {
                    return BadRequest("The pervasiveness supplied must be between 0 and 10.");
                }
                if (step.Adverse < 0 || step.Adverse > 10)
                {
                    return BadRequest("The adverse supplied must be between 0 and 10.");
                }
                if (step.Impact < 0 || step.Impact > 10)
                {
                    return BadRequest("The impact supplied must be between 0 and 10.");
                }
                List<string> riskList = new List<string>();
                riskList.AddRange(new string[] { "Very Low", "Low", "Moderate", "High", "Very High" });
                if (step.Risk == null || step.Risk == "")
                {
                    step.Risk = "Very Low";
                }
                else if (!riskList.Contains(step.Risk))
                {
                    return BadRequest("The risk supplied is invalid. Must be one of: " + riskList.ToString());
                }

                List<string> relevanceList = new List<string>();
                relevanceList.AddRange(new string[] { "Confirmed", "Expected", "Anticipated", "Predicted", "Possible", "N/A" });
                if (step.Relevance == null || step.Relevance == "")
                {
                    step.Relevance = "";
                }
                else if (!relevanceList.Contains(step.Relevance))
                {
                    return BadRequest("The relevance supplied is invalid. Must be one of: " + relevanceList.ToString());
                }

                if (step.Likelihood == null || step.Likelihood == "")
                {
                    step.Likelihood = "Very Low";
                }
                else if (!riskList.Contains(step.Likelihood))
                {
                    return BadRequest("The likelihood supplied is invalid. Must be one of: " + riskList.ToString());
                }

                ThreatSources ts = new ThreatSources();
                if (step.ThreatSource == null || step.ThreatSource == "")
                {
                    step.ThreatSource = "";
                }
                else if (!ts.sources.Contains(step.ThreatSource))
                {
                    return BadRequest("The threat source supplied is invalid. Must be one of: " + ts.sources.ToString());
                }

                Techniques mitre = new Techniques("");
                if (step.MitreId == null || step.MitreId == "") { step.MitreId = ""; }
                else if (!mitre.techniquesFull.ContainsKey(step.MitreId))
                {
                    return BadRequest("The MITRE ATT&CK ID supplied is invalid or was not found. Must be in the format of either T#### or T####/###");
                }

                if (step.Vulnerability == null || step.Vulnerability == "") { step.Vulnerability = ""; step.Severity = 0; }
                if (step.Condition == null || step.Condition == "") { step.Condition = ""; step.Pervasiveness = 0; }
                if (step.Mitigation == null) { step.Mitigation = ""; }

                if (step.Event == null || step.Event == "")
                {
                    return BadRequest("Event should describe what is happening. Should not be empty or null.");
                }

                // Validate the GraphNode
                if (step.GraphNode != null)
                {
                    step.GraphNode.Id = step.Id;
                    step.GraphNode.Risk = step.Risk;
                    if (step.GraphNode.EntityType == null || step.GraphNode.EntityType == "")
                    {
                        step.GraphNode.EntityType = "autonomous-system";
                    }
                    else
                    {
                        EntityTypes et = new EntityTypes();
                        Dictionary<string, string> entities = et.getTypes();
                        if (!entities.ContainsKey(step.GraphNode.EntityType))
                        {
                            return BadRequest("Supplied GraphNode EntityType is invalid. Please use one of the following: " + entities.Keys.ToString());
                        }
                    }
                    if (step.GraphNode.EntityDescription == null || step.GraphNode.EntityDescription == "")
                    {
                        step.GraphNode.EntityDescription = " ";
                    }
                    if (step.GraphNode.ParentId == null) { step.GraphNode.ParentId = new string[] { null }; }
                    foreach (string pId in step.GraphNode.ParentId)
                    {
                        if (pId == null) { continue; }
                        else
                        {
                            // We need to make sure the parent node exists
                            Steps p;
                            try
                            {
                                p = await _stepsService.GetByIdAsync(pId);
                            }
                            catch
                            {
                                return BadRequest();
                            }
                            if (p == null)
                            {
                                return NotFound("One of the parent nodes in the supplied event does not exist.");
                            }

                            // We need to make sure the parent node belongs to the same scenario
                            foreach (Scenarios scenario in await _scenariosService.GetAllAsync())
                            {
                                if (scenario.Steps.Contains(id))
                                {
                                    if (!scenario.Steps.Contains(pId))
                                    {
                                        return NotFound("One of the parent nodes in the supplied event does not belong to the same scenario.");
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                try
                {
                    await _stepsService.UpdateAsync(id, step);
                }
                catch
                {
                    return BadRequest();
                }

                return Ok(await _stepsService.GetByIdAsync(id));
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

            try
            {
                var scenario = await _scenariosService.GetByStepIdAsync(id);
                var assessment = await _assessmentsService.GetByScenarioIdAsync(scenario.Id);
                if (authorizedFor == assessment.Id || authorizedFor == "*")
                {
                    // Make sure event exists
                    Steps step;
                    try
                    {
                        step = await _stepsService.GetByIdAsync(id);
                    }
                    catch
                    {
                        return BadRequest();
                    }
                    if (step == null)
                    {
                        return NotFound("The supplied event was not found.");
                    }
                    try
                    {
                        await _stepsService.DeleteAsync(id);
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
            catch
            {
                return BadRequest();
            }
        }
    }
}