/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     06-07-2021
# Copyright:   (c) BLS OPS LLC. 2021
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Enter_The_Matrix.Models;
using System.Collections.Generic;
using Enter_The_Matrix.Services;
using Enter_The_Matrix.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using Enter_The_Matrix.FactorModels;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using Manatee.Json;
using Manatee.Json.Serialization;
using Manatee.Json.Schema;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Enter_The_Matrix.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AssessmentsService _assessmentsService;
        private readonly ScenariosService _scenariosService;
        private readonly StepsService _stepsService;
        private readonly SteplatesService _steplatesService;
        private readonly TreeService _treeService;

        public HomeController(
            ILogger<HomeController> logger,
            AssessmentsService assessmentsService,
            ScenariosService scenariosService,
            StepsService stepsService,
            SteplatesService steplatesService,
            TreeService treeService)
        {
            _logger = logger;
            _assessmentsService = assessmentsService;
            _scenariosService = scenariosService;
            _stepsService = stepsService;
            _steplatesService = steplatesService;
            _treeService = treeService;
        }

        #region Index

        public IActionResult Index()
        {
            return RedirectToAction("Login", "Security");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Main()
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }
            return View();
        }

        #endregion

        #region Assessments

        [HttpGet]
        public IActionResult Assessments()
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            List<Assessments> assessmentList = _assessmentsService.GetAllAsync().Result.ToList();
            assessmentList.Reverse();

            return View(assessmentList);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAssessment(string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            // Grab the assessent
            Assessments assessment = _assessmentsService.GetByIdAsync(assessmentId).Result;

            // Delete the threat tree associated with the assessment
            if (assessment.ThreatTreeId != null && assessment.ThreatTreeId != "") { await _treeService.DeleteAsync(assessment.ThreatTreeId); }

            // Clean out scenarios and events
            foreach (var scenarioId in assessment.Scenarios)
            {
                // Grab the scenario
                Models.Scenarios scenario = _scenariosService.GetByIdAsync(scenarioId).Result;
                foreach (var eventId in scenario.Steps)
                {
                    // Clean out events
                    await _stepsService.DeleteAsync(eventId);
                }

                // All events cleaned, remove scenario
                await _scenariosService.DeleteAsync(scenarioId);
            }

            // All scenarios cleaned, remove assessent
            await _assessmentsService.DeleteAsync(assessmentId);

            return RedirectToAction("Assessments", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssessment(string name)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            var identity = User.Identity as ClaimsIdentity;
            string user = "";
            if (identity != null)
            {
                try
                {
                    user = identity.FindFirst("DisplayName").Value;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }

            Assessments assessment = new Assessments
            {
                Name = name,
                Date = DateTime.Now,
                CreatedBy = user,
                Scenarios = new string[] { },
                ThreatTreeId = ""
            };

            Assessments newAssessment = await _assessmentsService.CreateAsync(assessment);

            return RedirectToAction("Scenarios", "Home", new { assessmentId = newAssessment.Id });
        }

        [HttpPost]
        public async Task<IActionResult> EditAssessment(string assessmentId, string name)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            Assessments assessment = _assessmentsService.GetByIdAsync(assessmentId).Result;

            assessment.Name = name;

            await _assessmentsService.UpdateAsync(assessmentId, assessment);

            return RedirectToAction("Scenarios", "Home", new { assessmentId = assessmentId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAssessment(string assessmentId, string[] scenarioIds)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            Assessments assessment = await _assessmentsService.GetByIdAsync(assessmentId);

            assessment.Scenarios = scenarioIds;

            await _assessmentsService.UpdateAsync(assessmentId, assessment);

            return RedirectToAction("Scenarios", "Home", new { assessmentId = assessmentId });
        }

        #endregion

        #region Scenarios

        [HttpGet]
        public IActionResult Scenarios(string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }
            Assessments assessments = _assessmentsService.GetByIdAsync(assessmentId).Result;
            List<Models.Scenarios> scenarioList = new List<Models.Scenarios>();

            foreach (var scenarioId in assessments.Scenarios)
            {
                scenarioList.Add(_scenariosService.GetByIdAsync(scenarioId).Result);
            }

            AssessmentScenarios assessmentModel = new AssessmentScenarios();
            assessmentModel.scenarioList = scenarioList;
            assessmentModel.assessment = assessments;

            ViewBag.ThreatTreeTemplate = new ThreatTree();

            List<(string, string, string)> defaultCategories = new List<(string, string, string)>();
            defaultCategories.Add(("Recon", "lightslateblue", "#8470ff"));
            defaultCategories.Add(("Initial Access", "dodgerblue1", "#1e90ff"));
            defaultCategories.Add(("Execution", "lightseagreen", "#20b2aa"));
            defaultCategories.Add(("Persistence", "lawngreen", "#7cfc00"));
            defaultCategories.Add(("Privilege Escalation", "greenyellow", "#adff2f"));
            defaultCategories.Add(("Defense Evasion", "yellow2", "#eeee00"));
            defaultCategories.Add(("Credential Access", "goldenrod2", "#eeb422"));
            defaultCategories.Add(("Lateral Movement", "orange3", "#cd8500"));
            defaultCategories.Add(("Collection", "orangered2", "#ee4000"));
            defaultCategories.Add(("Exfiltration", "crimson", "#dc143c"));
            defaultCategories.Add(("Impact", "hotpink3", "#cd6090"));

            ViewBag.DefaultCategories = defaultCategories;

            return View(assessmentModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteScenario(string scenarioId, string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            // Remove the steps belonging to the scenario
            Models.Scenarios scenario = _scenariosService.GetByIdAsync(scenarioId).Result;
            foreach (var step in scenario.Steps)
            {
                await _stepsService.DeleteAsync(step);
            }

            // Remove the scenario
            await _scenariosService.DeleteAsync(scenarioId);

            // Remove the scenario ID from the assessment owning it
            Assessments assessment = _assessmentsService.GetByIdAsync(assessmentId).Result;
            List<string> scenarios = assessment.Scenarios.ToList();
            scenarios.Remove(scenarioId);
            assessment.Scenarios = scenarios.ToArray();
            await _assessmentsService.UpdateAsync(assessmentId, assessment);

            // Return to scenario listing for the assessment
            return RedirectToAction("Scenarios", "Home", new { assessmentId = assessmentId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateScenario(string assessmentId, string name)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            var identity = User.Identity as ClaimsIdentity;
            string user = "";
            if (identity != null)
            {
                try
                {
                    user = identity.FindFirst("DisplayName").Value;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }

            Models.Scenarios scenario = new Models.Scenarios
            {
                Name = name,
                CreatedBy = user,
                Date = DateTime.Now,
                Steps = new string[] { }
            };

            // Create the scenario
            Models.Scenarios newScenario = await _scenariosService.CreateAsync(scenario);

            // Add the scenario to this assessments list of scenarios
            Assessments assessment = await _assessmentsService.GetByIdAsync(assessmentId);
            List<string> scenarioIds = assessment.Scenarios.ToList();
            scenarioIds.Add(newScenario.Id);
            assessment.Scenarios = scenarioIds.ToArray();
            await _assessmentsService.UpdateAsync(assessmentId, assessment);

            return RedirectToAction("Events", "Home", new { scenarioId = newScenario.Id, assessmentId = assessmentId });
        }

        [HttpPost]
        public async Task<IActionResult> EditScenario(string scenarioId, string assessmentId, string name)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            Models.Scenarios scenario = await _scenariosService.GetByIdAsync(scenarioId);

            scenario.Name = name;

            await _scenariosService.UpdateAsync(scenarioId, scenario);

            return RedirectToAction("Events", "Home", new { scenarioId = scenarioId, assessmentId = assessmentId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateScenario(string scenarioId, string assessmentId, string[] stepIds)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            Models.Scenarios scenario = await _scenariosService.GetByIdAsync(scenarioId);

            scenario.Steps = stepIds;

            await _scenariosService.UpdateAsync(scenarioId, scenario);

            return RedirectToAction("Events", "Home", new { scenarioId = scenarioId, assessmentId = assessmentId });
        }
        #endregion

        #region Events

        [HttpGet]
        public IActionResult Events(string scenarioId, string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }
            Models.Scenarios scenarios = _scenariosService.GetByIdAsync(scenarioId).Result;
            List<Steps> stepList = new List<Steps>();

            foreach (var stepId in scenarios.Steps)
            {
                stepList.Add(_stepsService.GetByIdAsync(stepId).Result);
            }

            Assessments assessment = _assessmentsService.GetByIdAsync(assessmentId).Result;

            ScenarioSteps scenarioModel = new ScenarioSteps();
            scenarioModel.stepList = stepList;
            scenarioModel.scenario = scenarios;
            scenarioModel.assessment = assessment;

            ViewBag.steplates = _steplatesService.GetAllAsync().Result;

            return View(scenarioModel);
        }

        [HttpGet]
        public IActionResult EditEvent(string stepId, string scenarioId, string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }
            ScenarioSteps scenarioModel = new ScenarioSteps();
            scenarioModel.scenario = _scenariosService.GetByIdAsync(scenarioId).Result;

            Steps step = _stepsService.GetByIdAsync(stepId).Result;
            scenarioModel.stepList = new List<Steps>();
            scenarioModel.stepList.Add(step);

            Assessments assessment = _assessmentsService.GetByIdAsync(assessmentId).Result;
            scenarioModel.assessment = assessment;

            ViewBag.sources = new ThreatSources();
            ViewBag.relevances = new Relevance();
            ViewBag.risks = new Risk();
            ViewBag.capabilities = new Capability();
            ViewBag.intents = new Intent();
            ViewBag.targetings = new Targeting();
            ViewBag.initiations = new Initiation();
            ViewBag.adverses = new Adverse();
            ViewBag.impacts = new Impact();
            ViewBag.vulnerabilities = new Vulnerability();
            ViewBag.pervasivenesses = new Pervasiveness();
            ViewBag.severities = new Severity();
            ViewBag.conditions = new Conditions();
            ViewBag.techniques = new Techniques(step.MitreId);

            List<Steps> prev_steps = new List<Steps>();
            foreach (string id in scenarioModel.scenario.Steps)
            {
                if (id != step.Id)
                {
                    prev_steps.Add(_stepsService.GetByIdAsync(id).Result);
                }
            }
            ViewBag.previous_steps = prev_steps;
            EntityTypes types = new EntityTypes();
            ViewBag.entities = types.getTypes();

            return View(scenarioModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent(
            string ev,
            string source,
            int capability,
            int intent,
            int targeting,
            string relevance,
            int initiation,
            int severity,
            int pervasiveness,
            int adverse,
            string likelihood,
            int impact,
            string risk,
            string stepId,
            string scenarioId,
            string assessmentId,
            string addedBy,
            DateTime dateCreated,
            string vulnerability = "",
            string mitigation = "",
            string condition = "",
            string entity_type = "",
            string entity_description = "",
            string[] entity_preceded_by = null,
            string mitreId = "")
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }
            if (!ModelState.IsValid) { return BadRequest(); }

            if(entity_description == "" || entity_description == null) { entity_description = " "; }
            Node node = new Node(entity_preceded_by, entity_type, entity_description, risk, stepId);


            Steps step = new Steps()
            {
                Id = stepId,
                Event = ev,
                ThreatSource = source,
                Capability = capability,
                Intent = intent,
                Targeting = targeting,
                Relevance = relevance,
                Initiation = initiation,
                Vulnerability = vulnerability,
                Mitigation = mitigation,
                Severity = severity,
                Condition = condition,
                Pervasiveness = pervasiveness,
                Adverse = adverse,
                Likelihood = likelihood,
                Impact = impact,
                Risk = risk,
                AddedBy = addedBy,
                Date = dateCreated,
                GraphNode = node,
                MitreId = mitreId
            };

            await _stepsService.UpdateAsync(step.Id, step);
            return RedirectToAction("Events", "Home", new { scenarioId = scenarioId, assessmentId = assessmentId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(string stepId, string scenarioId, string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            await _stepsService.DeleteAsync(stepId);

            Models.Scenarios scenario = await _scenariosService.GetByIdAsync(scenarioId);

            List<string> steps = scenario.Steps.ToList();

            steps.Remove(stepId);

            scenario.Steps = steps.ToArray();

            await _scenariosService.UpdateAsync(scenario.Id, scenario);

            return RedirectToAction("Events", "Home", new { scenarioId = scenarioId, assessmentId = assessmentId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(string scenarioId, string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            // Create the new empty step
            Steps step = new Steps
            {
                Event = "",
                ThreatSource = "",
                Capability = 0,
                Intent = 0,
                Targeting = 0,
                Relevance = "",
                Initiation = 0,
                Vulnerability = "",
                Severity = 0,
                Condition = "",
                Pervasiveness = 0,
                Mitigation = "",
                Adverse = 0,
                Likelihood = "Very Low",
                Impact = 0,
                Risk = "Very Low",
                Date = DateTime.Now,
                AddedBy = User.FindFirst("DisplayName").Value,
                MitreId = ""
            };

            // Add to the DB
            Steps newStep = await _stepsService.CreateAsync(step);

            // Link the new step to the current scenario
            Models.Scenarios scenario = await _scenariosService.GetByIdAsync(scenarioId);
            List<string> stepIds = scenario.Steps.ToList();
            stepIds.Add(newStep.Id);
            scenario.Steps = stepIds.ToArray();
            await _scenariosService.UpdateAsync(scenario.Id, scenario);

            // Return to step editting
            return RedirectToAction("EditEvent", "Home", new { stepId = newStep.Id, scenarioId = scenario.Id, assessmentId = assessmentId });
        }

        public async Task<IActionResult> ImportEvent(string steplateId, string assessmentId, string scenarioId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            if (steplateId == null || steplateId == "") { return RedirectToAction("CrateEvent", "Home", new { scenarioId = scenarioId, assessmentId = assessmentId}); }

            Steplates steplate = await _steplatesService.GetByIdAsync(steplateId);

            Steps step = new Steps();
            step.AddedBy = User.FindFirst("DisplayName").Value;
            step.Date = DateTime.Now;
            step.Adverse = steplate.Adverse;
            step.Capability = steplate.Capability;
            step.Condition = steplate.Condition;
            step.Event = steplate.Event;
            step.Impact = steplate.Impact;
            step.Initiation = steplate.Initiation;
            step.Intent = steplate.Intent;
            step.Likelihood = steplate.Likelihood;
            step.Mitigation = steplate.Mitigation;
            step.Pervasiveness = steplate.Pervasiveness;
            step.Relevance = steplate.Relevance;
            step.Risk = steplate.Risk;
            step.Severity = steplate.Severity;
            step.Targeting = steplate.Targeting;
            step.ThreatSource = steplate.ThreatSource;
            step.Vulnerability = steplate.Vulnerability;
            step.MitreId = steplate.MitreId;

            // Create the step
            Steps importedStep = await _stepsService.CreateAsync(step);

            // Add to our scenario
            Scenarios scenario = await _scenariosService.GetByIdAsync(scenarioId);
            List<string> stepIds = scenario.Steps.ToList();
            stepIds.Add(importedStep.Id);
            scenario.Steps = stepIds.ToArray();
            await _scenariosService.UpdateAsync(scenario.Id, scenario);

            return RedirectToAction("EditEvent", "Home", new { stepId = importedStep.Id, scenarioId = scenarioId, assessmentId = assessmentId });
        }

        [HttpPost]
        public async Task<IActionResult> TemplateEvent(string stepId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            Steps step = new Steps();
            step = await _stepsService.GetByIdAsync(stepId);

            Steplates steplate = new Steplates();
            steplate.AddedBy = User.FindFirst("DisplayName").Value;
            steplate.Adverse = step.Adverse;
            steplate.Capability = step.Capability;
            steplate.Date = DateTime.Now;
            steplate.Event = step.Event;
            steplate.Impact = step.Impact;
            steplate.Initiation = step.Initiation;
            steplate.Intent = step.Intent;
            steplate.Likelihood = step.Likelihood;
            steplate.MitreId = step.MitreId;
            steplate.Risk = step.Risk;
            steplate.Targeting = step.Targeting;
            steplate.ThreatSource = step.ThreatSource;

            Node node = new Node(null, step.GraphNode.EntityType, step.GraphNode.EntityDescription, "", "");

            steplate.GraphNode = node;

            steplate.Vulnerability = "";
            steplate.Pervasiveness = 0;
            steplate.Severity = 0;
            steplate.Mitigation = "";
            steplate.Relevance = "";
            steplate.Condition = "";

            Steplates retSteplate = await _steplatesService.CreateAsync(steplate);

            return RedirectToAction("EditSteplate", "Home", new { steplateId = retSteplate.Id });

        }
        
        #endregion

        #region Steplates

        [HttpGet]
        public async Task<IActionResult> Steplates()
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            List<Steplates> steplateList = await _steplatesService.GetAllAsync();

            return View(steplateList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSteplate()
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            Steplates steplate = new Steplates {
                Event = "",
                ThreatSource = "",
                Capability = 0,
                Intent = 0,
                Targeting = 0,
                Relevance = "",
                Initiation = 0,
                Vulnerability = "",
                Severity = 0,
                Condition = "",
                Pervasiveness = 0,
                Mitigation = "",
                Adverse = 0,
                Likelihood = "Very Low",
                Impact = 0,
                Risk = "Very Low",
                Date = DateTime.Now,
                AddedBy = User.FindFirst("DisplayName").Value,
                MitreId = ""
            };

            Steplates retSteplate = await _steplatesService.CreateAsync(steplate);

            return RedirectToAction("EditSteplate", "Home", new { steplateId = retSteplate.Id });
        }

        [HttpGet]
        public async Task<IActionResult> EditSteplate(string steplateId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            Steplates steplate = await _steplatesService.GetByIdAsync(steplateId);

            ViewBag.sources = new ThreatSources();
            ViewBag.relevances = new Relevance();
            ViewBag.risks = new Risk();
            ViewBag.capabilities = new Capability();
            ViewBag.intents = new Intent();
            ViewBag.targetings = new Targeting();
            ViewBag.initiations = new Initiation();
            ViewBag.adverses = new Adverse();
            ViewBag.impacts = new Impact();
            ViewBag.vulnerabilities = new Vulnerability();
            ViewBag.pervasivenesses = new Pervasiveness();
            ViewBag.severities = new Severity();
            ViewBag.conditions = new Conditions();
            ViewBag.techniques = new Techniques(steplate.MitreId);

            return View(steplate);
        }

        [HttpPost]
        public async Task<IActionResult> EditSteplate(
            string ev,
            string source,
            int capability,
            int intent,
            int targeting,
            string relevance,
            int initiation,
            int severity,
            int pervasiveness,
            int adverse,
            string likelihood,
            int impact,
            string risk,
            string steplateId,
            string addedBy,
            DateTime dateCreated,
            string vulnerability = "",
            string mitigation = "",
            string condition = "",
            string mitreId = ""
        )
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            Steplates steplate = new Steplates
            {
                Event = ev,
                ThreatSource = source,
                Capability = capability,
                Intent = intent,
                Targeting = targeting,
                Relevance = relevance,
                Initiation = initiation,
                Severity = severity,
                Pervasiveness = pervasiveness,
                Adverse = adverse,
                Likelihood = likelihood,
                Impact = impact,
                Risk = risk,
                Id = steplateId,
                AddedBy = addedBy,
                Date = dateCreated,
                Vulnerability = vulnerability,
                Mitigation = mitigation,
                Condition = condition,
                MitreId = mitreId
            };

            await _steplatesService.UpdateAsync(steplateId, steplate);

            return RedirectToAction("Steplates", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSteplate(string steplateId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            await _steplatesService.DeleteAsync(steplateId);

            return RedirectToAction("Steplates", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAllTemplates()
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            List<Steplates> templateList = await _steplatesService.GetAllAsync();
            foreach (Steplates template in templateList)
            {
               await _steplatesService.DeleteAsync(template.Id);
            }

            return RedirectToAction("Steplates", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ExportTemplates()
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            List<Steplates> templateList = await _steplatesService.GetAllAsync();
            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };
            string json = System.Text.Json.JsonSerializer.Serialize(templateList, options);
            string filename = "wwwroot/templates/templates.json";
            System.IO.File.WriteAllText(filename, json);
            var bytes = System.IO.File.ReadAllBytes(filename);
            return File(bytes, "text/plain", Path.GetFileName(filename));
        }

        [HttpPost]
        public async Task<IActionResult> ImportTemplates(IFormFile file)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            List<Steplates> templateList = new List<Steplates>();
            var reader = new StreamReader(file.OpenReadStream());
            string json = reader.ReadToEnd();
            reader.Close();

            string arraySchemaJson = "{\"type\":\"array\",\"minItems\":1,\"items\":[{\"type\":\"object\",\"properties\":{\"Id\":{\"type\":\"string\"},\"Event\":{\"type\":\"string\"},\"ThreatSource\":{\"type\":\"string\"},\"Capability\":{\"type\":\"integer\"},\"Intent\":{\"type\":\"integer\"},\"Targeting\":{\"type\":\"integer\"},\"Relevance\":{\"type\":\"string\"},\"Initiation\":{\"type\":\"integer\"},\"Vulnerability\":{\"type\":\"string\"},\"Severity\":{\"type\":\"integer\"},\"Condition\":{\"type\":\"string\"},\"Pervasiveness\":{\"type\":\"integer\"},\"Mitigation\":{\"type\":\"string\"},\"Adverse\":{\"type\":\"integer\"},\"Likelihood\":{\"type\":\"string\"},\"Impact\":{\"type\":\"integer\"},\"Risk\":{\"type\":\"string\"},\"Date\":{\"type\":\"string\"},\"AddedBy\":{\"type\":\"string\"},\"GraphNode\":{\"type\":\"null\"},\"MitreId\":{\"type\":\"string\"}},\"required\":[\"Id\",\"Event\",\"ThreatSource\",\"Capability\",\"Intent\",\"Targeting\",\"Relevance\",\"Initiation\",\"Vulnerability\",\"Severity\",\"Condition\",\"Pervasiveness\",\"Mitigation\",\"Adverse\",\"Likelihood\",\"Impact\",\"Risk\",\"Date\",\"AddedBy\",\"GraphNode\",\"MitreId\"]}]}";
            Manatee.Json.Serialization.JsonSerializer serializer = new Manatee.Json.Serialization.JsonSerializer();
            JsonValue arraySchemaParsed = JsonValue.Parse(arraySchemaJson);
            JsonSchema arraySchema = serializer.Deserialize<JsonSchema>(arraySchemaParsed);
            JsonValue jsonImported = new JsonValue();
            try
            {
                jsonImported = JsonValue.Parse(json);
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now.ToString() + " : Error parsing JSON input");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return RedirectToAction("Steplates", "Home");
            }
            
            SchemaValidationResults isArray = arraySchema.Validate(jsonImported);

            if (isArray.IsValid)
            {
                string templateSchemaJson = "{\"type\":\"object\",\"properties\":{\"Id\":{\"type\":\"string\"},\"Event\":{\"type\":\"string\"},\"ThreatSource\":{\"type\":\"string\"},\"Capability\":{\"type\":\"integer\"},\"Intent\":{\"type\":\"integer\"},\"Targeting\":{\"type\":\"integer\"},\"Relevance\":{\"type\":\"string\"},\"Initiation\":{\"type\":\"integer\"},\"Vulnerability\":{\"type\":\"string\"},\"Severity\":{\"type\":\"integer\"},\"Condition\":{\"type\":\"string\"},\"Pervasiveness\":{\"type\":\"integer\"},\"Mitigation\":{\"type\":\"string\"},\"Adverse\":{\"type\":\"integer\"},\"Likelihood\":{\"type\":\"string\"},\"Impact\":{\"type\":\"integer\"},\"Risk\":{\"type\":\"string\"},\"Date\":{\"type\":\"string\"},\"AddedBy\":{\"type\":\"string\"},\"GraphNode\":{\"type\":\"null\"},\"MitreId\":{\"type\":\"string\"}},\"required\":[\"Id\",\"Event\",\"ThreatSource\",\"Capability\",\"Intent\",\"Targeting\",\"Relevance\",\"Initiation\",\"Vulnerability\",\"Severity\",\"Condition\",\"Pervasiveness\",\"Mitigation\",\"Adverse\",\"Likelihood\",\"Impact\",\"Risk\",\"Date\",\"AddedBy\",\"GraphNode\",\"MitreId\"]}";
                JsonValue templateSchemaParsed = JsonValue.Parse(templateSchemaJson);
                JsonSchema templateSchema = serializer.Deserialize<JsonSchema>(templateSchemaParsed);

                using (JsonDocument jDoc = JsonDocument.Parse(json))
                {
                    JsonElement root = jDoc.RootElement;
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        // We have our array, start grabbing each object and comparing against our template schema
                        for (int i = 0; i < root.GetArrayLength(); i++)
                        {
                            JsonElement jsonTemplate = root[i];
                            if (jsonTemplate.ValueKind != JsonValueKind.Object) { break; }
                            string jsonTemplateStr = jsonTemplate.ToString();
                            JsonValue jsonTemplateImported = JsonValue.Parse(jsonTemplateStr);
                            SchemaValidationResults isTemplate = templateSchema.Validate(jsonTemplateImported, new JsonSchemaOptions { OutputFormat = SchemaValidationOutputFormat.Basic });

                            if (isTemplate.IsValid)
                            {
                                // Valid Template!
                                // Create the template and add it to our list
                                Steplates templateImport = serializer.Deserialize<Steplates>(jsonTemplateImported);
                                templateList.Add(new Steplates
                                {
                                    AddedBy = templateImport.AddedBy,
                                    Adverse = templateImport.Adverse,
                                    Capability = templateImport.Capability,
                                    Condition = templateImport.Condition,
                                    Date = templateImport.Date,
                                    Event = templateImport.Event,
                                    GraphNode = templateImport.GraphNode,
                                    Impact = templateImport.Impact,
                                    Initiation = templateImport.Initiation,
                                    Intent = templateImport.Intent,
                                    Likelihood = templateImport.Likelihood,
                                    Mitigation = templateImport.Mitigation,
                                    MitreId = templateImport.MitreId,
                                    Pervasiveness = templateImport.Pervasiveness,
                                    Relevance = templateImport.Relevance,
                                    Risk = templateImport.Risk,
                                    Severity = templateImport.Severity,
                                    Targeting = templateImport.Targeting,
                                    ThreatSource = templateImport.ThreatSource,
                                    Vulnerability = templateImport.Vulnerability
                                });
                            }
                            else
                            {
                                Console.WriteLine(DateTime.Now.ToString() + " : Template Import Failed");
                                Console.WriteLine(DateTime.Now.ToString() + " : Template failed schema check");
                                try { Console.WriteLine(isTemplate.ErrorMessage.ToString()); }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(DateTime.Now.ToString() + " : Template Import Failed");
                        Console.WriteLine(DateTime.Now.ToString() + " : Input not array");
                    }
                }
            }
            else
            {
                Console.WriteLine(DateTime.Now.ToString() + " : Template Import Failed");
                Console.WriteLine(DateTime.Now.ToString() + " : Input not array");
            }

            // Check if the list was populated
            // If populated, add templates to DB
            if (templateList.Count > 0) { foreach (Steplates template in templateList) { await _steplatesService.CreateAsync(template); } }
            
            return RedirectToAction("Steplates", "Home");

        }
        #endregion

        #region Export
        public async Task<IActionResult> ExportExcel(string filename, string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            Assessments assessment = await _assessmentsService.GetByIdAsync(assessmentId);
            Relevance rel = new Relevance();

            using (var workbook = new XLWorkbook())
            {
                var id = 0;
                var worksheet = workbook.Worksheets.Add("Threat Matrix");

                worksheet.Range("A1:Q2").Style.Alignment.WrapText = true;

                worksheet.Row(1).Height = 65;
                worksheet.Row(2).Height = 100;

                worksheet.Column(1).Width = 8;
                worksheet.Column(2).Width = 16;
                worksheet.Column(3).Width = 4;
                worksheet.Column(4).Width = 20;
                worksheet.Column(5).Width = 15;
                worksheet.Column(6).Width = 4;
                worksheet.Column(7).Width = 4;
                worksheet.Column(8).Width = 4;
                worksheet.Column(9).Width = 8;
                worksheet.Column(10).Width = 5;
                worksheet.Column(11).Width = 15;
                worksheet.Column(12).Width = 15;
                worksheet.Column(13).Width = 4;
                worksheet.Column(14).Width = 25;
                worksheet.Column(15).Width = 5;
                worksheet.Column(16).Width = 12;
                worksheet.Column(17).Width = 4;
                worksheet.Column(18).Width = 12;

                worksheet.Range("A1:D2").Merge();

                worksheet.Cell("E1").Value = "Threat Sources";
                worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Range("E1:E2").Merge();

                worksheet.Cell("F1").Value = "Threat Source Characteristics";
                worksheet.Cell("F1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Range("F1:H1").Merge();

                worksheet.Cell("F2").Value = "Capability (0-10)";
                worksheet.Cell("F2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                worksheet.Cell("F2").Style.Alignment.SetTextRotation(90);

                worksheet.Cell("G2").Value = "Intent (0-10)";
                worksheet.Cell("G2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                worksheet.Cell("G2").Style.Alignment.SetTextRotation(90);

                worksheet.Cell("H2").Value = "Targeting (0-10)";
                worksheet.Cell("H2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                worksheet.Cell("H2").Style.Alignment.SetTextRotation(90);

                worksheet.Cell("I1").Value = "Relevance (Confirmed (C), Expected (E), Anticipated (A), Predicted (Pr), Possible (Po))";
                worksheet.Cell("I1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                worksheet.Cell("I1").Style.Alignment.SetTextRotation(90);
                worksheet.Range("I1:I2").Merge();

                worksheet.Cell("J1").Value = "Likelihood of Attack Initiation (0-10)";
                worksheet.Cell("J1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                worksheet.Cell("J1").Style.Alignment.SetTextRotation(90);
                worksheet.Range("J1:J2").Merge();

                worksheet.Cell("K1").Value = "Vulnerabilities";
                worksheet.Cell("K1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Range("K1:K2").Merge();

                worksheet.Cell("L1").Value = "Predisposing Conditions";
                worksheet.Cell("L1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Range("L1:L2").Merge();

                worksheet.Cell("M1").Value = "Severity and Pervasiveness (0-10)";
                worksheet.Cell("M1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                worksheet.Cell("M1").Style.Alignment.SetTextRotation(90);
                worksheet.Range("M1:M2").Merge();

                worksheet.Cell("N1").Value = "Mitigation";
                worksheet.Cell("N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Range("N1:N2").Merge();

                worksheet.Cell("O1").Value = "Likelihood Attack Results in Adverse Impact (0-10)";
                worksheet.Cell("O1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                worksheet.Cell("O1").Style.Alignment.SetTextRotation(90);
                worksheet.Range("O1:O2").Merge();

                worksheet.Cell("P1").Value = "Overall Likelihood (Very Low, Low, Moderate, High, Very High)";
                worksheet.Cell("P1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                worksheet.Cell("P1").Style.Alignment.SetTextRotation(90);
                worksheet.Range("P1:P2").Merge();

                worksheet.Cell("Q1").Value = "Level of Impact (0-10)";
                worksheet.Cell("Q1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Bottom);
                worksheet.Cell("Q1").Style.Alignment.SetTextRotation(90);
                worksheet.Range("Q1:Q2").Merge();

                worksheet.Cell("R1").Value = "Risk \n \n (Very Low, Low, Moderate, High, Very High)";
                worksheet.Cell("R1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Range("R1:R2").Merge();

                worksheet.Cell("A3").Value = "ADVERSARIAL";
                worksheet.Cell("A3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Range("A3:R3").Merge();
                worksheet.Range("A3:R3").Style.Fill.BackgroundColor = XLColor.LightGray;
                worksheet.Range("A3:R3").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

                worksheet.Range("A4:E4").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                worksheet.Range("F4:R4").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                worksheet.Range("F4:R4").Style.Fill.BackgroundColor = XLColor.LightGray;

                worksheet.Cell("A4").Value = "ID";
                worksheet.Cell("A4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                worksheet.Cell("B4").Value = "MITRE ATT&CK ID";
                worksheet.Cell("B4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                worksheet.Cell("C4").Value = "Threat Scenario and Events";
                worksheet.Cell("C4").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Range("C4:D4").Merge();

                worksheet.Range("A1:R4").Style.Font.Bold = true;

                worksheet.SheetView.FreezeRows(4);

                int row = 4;

                foreach (var scenarioId in assessment.Scenarios)
                {
                    row++;
                    id++;
                    var subId = 0;
                    Enter_The_Matrix.Models.Scenarios scenario = await _scenariosService.GetByIdAsync(scenarioId);
                    worksheet.Cell(row, 1).Value = id;
                    worksheet.Cell(row, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    
                    worksheet.Cell(row, 3).Value = "Scenario - " + scenario.Name;
                    worksheet.Cell(row, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell(row, 3).Style.Alignment.WrapText = true;
                    worksheet.Range(row, 3, row, 4).Merge();

                    worksheet.Range(row, 5, row, 18).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Range(row, 5, row, 18).Style.Border.OutsideBorder = XLBorderStyleValues.Thick;

                    worksheet.Row(row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    foreach (var eventId in scenario.Steps)
                    {
                        row++;
                        subId++;

                        Steps step = await _stepsService.GetByIdAsync(eventId);

                        double severityAndPervasiveness = 0;
                        if (step.Severity == 0) { severityAndPervasiveness = step.Pervasiveness; }
                        else if (step.Pervasiveness == 0) { severityAndPervasiveness = step.Severity; }
                        else { severityAndPervasiveness = (int)Math.Max(step.Severity, step.Pervasiveness); }

                        worksheet.Cell(row, 1).Value = id.ToString() + "." + subId.ToString();

                        if (subId.ToString().ElementAt(subId.ToString().Length - 1) == '0')
                        {
                            worksheet.Cell(row, 1).Style.NumberFormat.Format = "0.00";
                        }

                        if (step.MitreId != null && step.MitreId != "")
                        {
                            string val = step.MitreId.Replace('/', '.');
                            string url = @"https://attack.mitre.org/techniques/";
                            string tech = step.MitreId + @"/";
                            if (step.MitreId.StartsWith("ics-")) { 
                                url = @"https://collaborate.mitre.org/attackics/index.php/Technique/"; 
                                tech = step.MitreId.Substring(4);
                                val = tech;
                            }
                            worksheet.Cell(row, 2).Value = val;
                            worksheet.Cell(row, 2).Hyperlink = new XLHyperlink(url + tech);
                            worksheet.Cell(row, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        }
                        else
                        {
                            worksheet.Cell(row, 2).Value = "N/A";
                            worksheet.Cell(row, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        }

                        worksheet.Cell(row, 4).Value = step.Event;
                        worksheet.Cell(row, 4).Style.Alignment.WrapText = true;

                        worksheet.Cell(row, 5).Value = step.ThreatSource;
                        worksheet.Cell(row, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        worksheet.Cell(row, 6).Value = step.Capability;
                        worksheet.Cell(row, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        worksheet.Cell(row, 7).Value = step.Intent;
                        worksheet.Cell(row, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        worksheet.Cell(row, 8).Value = step.Targeting;
                        worksheet.Cell(row, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        worksheet.Cell(row, 9).Value = rel.getAbbreviation(step.Relevance);
                        worksheet.Cell(row, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        worksheet.Cell(row, 10).Value = step.Initiation;
                        worksheet.Cell(row, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        if (step.Vulnerability == null || step.Vulnerability.ToString() == "")
                        {
                            worksheet.Cell(row, 11).Value = "N/A";
                        }
                        else
                        {
                            worksheet.Cell(row, 11).Value = step.Vulnerability;
                        }
                        worksheet.Cell(row, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(row, 11).Style.Alignment.WrapText = true;

                        if (step.Condition == null || step.Condition.ToString() == "")
                        {
                            worksheet.Cell(row, 12).Value = "N/A";
                        }
                        else
                        {
                            worksheet.Cell(row, 12).Value = step.Condition;
                        }
                        worksheet.Cell(row, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(row, 12).Style.Alignment.WrapText = true;

                        if (severityAndPervasiveness == 0) { worksheet.Cell(row, 13).Value = "N/A"; }
                        else { worksheet.Cell(row, 13).Value = severityAndPervasiveness; }
                        worksheet.Cell(row, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        if (step.Mitigation == null || step.Mitigation.ToString() == "")
                        {
                            worksheet.Cell(row, 14).Value = "None";
                        }
                        else
                        {
                            worksheet.Cell(row, 14).Value = step.Mitigation;
                        }
                        worksheet.Cell(row, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(row, 14).Style.Alignment.WrapText = true;

                        worksheet.Cell(row, 15).Value = step.Adverse;
                        worksheet.Cell(row, 15).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        worksheet.Cell(row, 16).Value = step.Likelihood;
                        worksheet.Cell(row, 16).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(row, 16).Style.Alignment.WrapText = true;
                        string likelihoodFormula = "=IF(AND(J1<2,P1<5),\"Very Low\", IF(AND(J1<5,P1<2),\"Very Low\", IF(AND(J1<2,AND(P1<11,P1>4)),\"Low\", IF(AND(AND(J1<5,J1>1),AND(P1<8,P1>1)), \"Low\", IF(AND(AND(J1<8,J1>4),AND(P1<5,P1>=0)), \"Low\", IF(AND(AND(J1<11,J1>7),AND(P1<2,P1>=0)), \"Low\", IF(AND(AND(J1<5,J1>1),AND(P1<11,P1>7)), \"Moderate\", IF(AND(AND(J1<8,J1>4),AND(P1<10,P1>4)), \"Moderate\", IF(AND(AND(J1<10,J1>7),AND(P1<8,P1>1)), \"Moderate\", IF(AND(AND(J1<11,J1>9),AND(P1<5,P1>1)), \"Moderate\", IF(AND(AND(J1<8,J1>4),AND(P1<11,P1>9)), \"High\", IF(AND(AND(J1<10,J1>7),AND(P1<10,P1>7)), \"High\", IF(AND(AND(J1<11,J1>9),AND(P1<8,P1>4)), \"High\", IF(AND(AND(J1<10,J1>7),AND(P1<11,P1>9)), \"Very High\", IF(AND(AND(J1<11,J1>9),AND(P1<11,P1>7)), \"Very High\", \"\")))))))))))))))";
                        likelihoodFormula = likelihoodFormula.Replace("J1", "J" + row).Replace("P1", "O" + row);
                        worksheet.Cell(row, 16).FormulaA1 = likelihoodFormula;

                        worksheet.Cell(row, 17).Value = step.Impact;
                        worksheet.Cell(row, 17).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                        worksheet.Cell(row, 18).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Cell(row, 18).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(row, 18).Style.Alignment.WrapText = true;
                        worksheet.Cell(row, 18).Value = step.Risk;
                        XLColor bgRisk = XLColor.White;
                        if (step.Risk == "Very Low") { bgRisk = XLColor.Aqua; }
                        else if (step.Risk == "Low") { bgRisk = XLColor.Green; }
                        else if (step.Risk == "Moderate") { bgRisk = XLColor.Yellow; }
                        else if (step.Risk == "High") { bgRisk = XLColor.Orange; }
                        else if (step.Risk == "Very High") { bgRisk = XLColor.Red; }
                        worksheet.Cell(row, 18).Style.Fill.BackgroundColor = bgRisk;
                        worksheet.Cell(row, 18).AddConditionalFormat().WhenIsTrue("=R" + row + "=\"Very Low\"").Fill.BackgroundColor = XLColor.Aqua;
                        worksheet.Cell(row, 18).AddConditionalFormat().WhenIsTrue("=R" + row + "=\"Low\"").Fill.BackgroundColor = XLColor.Green;
                        worksheet.Cell(row, 18).AddConditionalFormat().WhenIsTrue("=R" + row + "=\"Moderate\"").Fill.BackgroundColor = XLColor.Yellow;
                        worksheet.Cell(row, 18).AddConditionalFormat().WhenIsTrue("=R" + row + "=\"High\"").Fill.BackgroundColor = XLColor.Orange;
                        worksheet.Cell(row, 18).AddConditionalFormat().WhenIsTrue("=R" + row + "=\"Very High\"").Fill.BackgroundColor = XLColor.Red;
                        string riskFormula = "=IF(Z1=\"Very Low\",IF(AND(X1<8,X1>=0),\"Very Low\",IF(AND(X1<11,X1>7),\"Low\",\"\")),IF(Z1=\"Low\",IF(AND(X1<2,X1>=0),\"Very Low\",IF(AND(X1<10,X1>1),\"Low\",IF(AND(X1<11,X1>9),\"Moderate\",\"\"))),IF(Z1=\"Moderate\",IF(AND(X1<2,X1>=0),\"Very Low\",IF(AND(X1<5,X1>1),\"Low\",IF(AND(X1<10,X1>4),\"Moderate\",IF(AND(X1<11,X1>9),\"High\",\"\")))),IF(OR(Z1=\"High\",Z1=\"Very High\"),IF(AND(X1<2,X1>=0),\"Very Low\",IF(AND(X1<5,X1>1),\"Low\",IF(AND(X1<8,X1>4),\"Moderate\",IF(AND(X1<10,X1>7),\"High\",IF(AND(X1<11,X1>9),\"Very High\",\"\"))))),\"\"))))";
                        riskFormula = riskFormula.Replace("Z1", "P" + row).Replace("X1", "Q" + row);
                        worksheet.Cell(row, 18).FormulaA1 = riskFormula;

                        worksheet.Row(row).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        filename + ".xlsx");
                }
            }
        }

        public async Task<IActionResult> ExportPDF (string assessmentId, string fileName)
        {
            Assessments assessment = await _assessmentsService.GetByIdAsync(assessmentId);
            
            List<Scenarios> scenarioList = new List<Scenarios>();
            foreach (var scenario in assessment.Scenarios) { scenarioList.Add(await _scenariosService.GetByIdAsync(scenario)); }
            
            Dictionary<string, List<Steps>> stepsDict = new Dictionary<string, List<Steps>>();
            
            foreach (var scenario in scenarioList)
            {
                List<Steps> scenarioSteps = new List<Steps>();
                foreach (var step in scenario.Steps) { scenarioSteps.Add(await _stepsService.GetByIdAsync(step)); }
                stepsDict[scenario.Id] = scenarioSteps;
            }

            // We have the assessment, the scenarios, and the events tied to each scenario
            // Send them over to the view via the ViewBag and work some magic

            ViewBag.Assessment = assessment;
            ViewBag.Scenarios = scenarioList;
            ViewBag.Steps = stepsDict;
            ViewBag.FileName = fileName;

            return View();
        }
        #endregion

        #region Diagram

        public async Task<IActionResult> Diagram(string scenarioId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            /* Deprecated GraphViz Code
            List<string> document = new List<string>();

            Scenarios scenario = await _scenariosService.GetByIdAsync(scenarioId);
            List<Steps> stepList = new List<Steps>();
            foreach (string stepId in scenario.Steps)
            {
                stepList.Add(await _stepsService.GetByIdAsync(stepId));
            }

            List<Node> nodeList = new List<Node>();

            // Fixing root node issue
            var first = stepList.First();
            

            foreach (var step in stepList)
            {
                for (int i = 0; i < step.GraphNode.ParentId.Length; i++)
                {
                    if (step.GraphNode == null || step.GraphNode.Id == null || step.GraphNode.EntityDescription == null || step.GraphNode.EntityType == null)
                    {
                        break;
                    }
                    else if (step.GraphNode.ParentId[i] == null || step.GraphNode.ParentId[i] == "")
                    {
                        step.GraphNode.ParentId[i] = "root";
                    }
                    else
                    {
                        continue;
                    }
                }

                // Once we've made sure all root replacements have been made, add the node to the list
                nodeList.Add(step.GraphNode);

            }

            // Workaround for graphiz IDs
            int nodeId = 0;
            List<int> sameRanks = new List<int>();
            Dictionary<string, string> graphizIdTable = new Dictionary<string, string>();
            foreach (var node in nodeList)
            {
                if ( nodeId % 3 == 0)
                {
                    sameRanks.Add(nodeId);
                }
                graphizIdTable.Add(node.Id, nodeId++.ToString());
            }
            // setup the document
            document.Add("digraph Scenario {");
            document.Add("");
            document.Add("\tcompound=\"true\";");
            document.Add("\tranksep=1.25;");
            document.Add("\trankdir=\"LR\";");
            document.Add("\tnode[shape=\"plaintext\", fontsize=12, label=\"\"];");
            document.Add("\tbgcolor=\"transparent\";");
            document.Add("\tedge[arrowsize=1, color=\"black\"];");
            document.Add("\tgraph[penwidth=0, labelloc=\"b\"];");
            document.Add("\tnewrank=\"true\";");


            // create the nodes
            foreach (var node in nodeList)
            {
                List<string> wrappingDescription = node.EntityDescription.Split(' ').ToList();
                string wrappedDescription = "";
                int breaker = 4;
                int index = 0;
                foreach (string word in wrappingDescription)
                {
                    wrappedDescription += word + " ";
                    index++;
                    if (index % breaker == 0)
                    {
                        wrappedDescription += @"\n";
                    }
                }
                string idString = graphizIdTable.GetValueOrDefault(node.Id);
                document.Add("\tsubgraph cluster_" + idString + " {");
                document.Add("\t\tlabel=\"" + wrappedDescription.Replace("\"", "\\\"") + "\";");
                document.Add("\t\t" + idString + " [shape=\"none\",fixedsize=true,height=\"1\",width=\"1\",label=\"\",image=\"/var/matrix/app/enter_the_matrix/wwwroot/icons/" + node.EntityType + ".png\"]");
                document.Add("\t}");
            }

            string sameRank = "{rank=\"same\"; ";
            foreach (int element in sameRanks)
            {
                sameRank += element.ToString() + "; ";
            }
            sameRank += "};";
            document.Add(sameRank);

            // create the Links
            int stepCount = 1;
            foreach (var node in nodeList)
            {
                if (node.ParentId != null)
                {
                    int stepCount2 = stepCount++;
                    foreach (var parent in node.ParentId)
                    {
                        string idString = graphizIdTable.GetValueOrDefault(node.Id);
                        string parentIdString = graphizIdTable.GetValueOrDefault(parent);
                        string indexColor = node.GetRiskColor();
                        string borderColor = node.GetRiskColor();
                        string fillColor = "black";
                        string textStyle = "courier new";
                        document.Add("\t" + parentIdString + " -> " + idString + " [label=<<table border=\"0\" cellborder=\"2\" cellpadding=\"0\" color=\"" + borderColor + "\"><tr><td cellpadding=\"2\" border=\"2\" bgcolor=\"" + fillColor + "\"><font color=\"" + indexColor + "\" point-size=\"12.0\" face=\"" + textStyle + "\"><b>" + stepCount2 + "</b></font></td></tr></table>>, color=\"" + node.GetRiskColor() + "\"]");
                    }
                }
            }

            // Label Things
            document.Add("\tlabelloc=\"t\";");
            document.Add("\tfontsize=\"28\";");
            document.Add("\tlabel=\"" + scenario.Name.Replace("\"", "\\\"") + "\";");
            document.Add("}");
            */

            // Hijacking flow for D3 //

            Scenarios scenario = await _scenariosService.GetByIdAsync(scenarioId);

            List<Steps> eventList = new List<Steps>();
            foreach (string stepId in scenario.Steps) { eventList.Add(await _stepsService.GetByIdAsync(stepId)); }

            string quoteEscaper = @"\" + "\"";

            using (StreamWriter outputFile = new StreamWriter(Path.Combine("wwwroot/graphs/", "graph.json")))
            {
                outputFile.WriteLine("{");
                outputFile.WriteLine("\"nodes\": [");
                List<string> Ids = new List<string>();
                List<(string parent, string id, string risk, int step)> links = new List<(string parent, string id, string risk, int step)>();
                int counter = 0;

                // Accounting for root node
                Ids.Add("");

                // Setting icon according to first event threat actor
                var first = eventList.First();
                string attackerIcon = "";

                if (
                first.ThreatSource == "Outsider" ||
                first.ThreatSource == "Insider" ||
                first.ThreatSource == "Trusted Insider" ||
                first.ThreatSource == "Privileged Insider"
                )
                {
                    attackerIcon = "malicious-actor";
                }
                else if (
                    first.ThreatSource == "Partner Organization" ||
                    first.ThreatSource == "Competitor Organization" ||
                    first.ThreatSource == "Supplier Organization" ||
                    first.ThreatSource == "Customer Organization"
                    )
                {
                    attackerIcon = "group";
                }
                else if (
                    first.ThreatSource == "Ad Hoc Group" ||
                    first.ThreatSource == "Established Group"
                    )
                {
                    attackerIcon = "organization";
                }
                else if (
                    first.ThreatSource == "Nation State"
                    )
                {
                    attackerIcon = "guard-personnel";
                }
                else
                {
                    attackerIcon = "unknown-suspect";
                }

                outputFile.WriteLine("{");
                outputFile.WriteLine($"\"x\": 0,");
                outputFile.WriteLine($"\"y\": 0,");
                outputFile.WriteLine($"\"title\": \"{first.ThreatSource}\",");
                outputFile.WriteLine($"\"icon\": \"/icons/{attackerIcon}.png\"");
                outputFile.WriteLine("},");

                foreach (Steps ev in eventList)
                {

                    // Inject Node Information
                    if (!Ids.Contains(ev.Id)) { Ids.Add(ev.Id); }
                    outputFile.WriteLine("{");
                    outputFile.WriteLine("\"x\": 0,");
                    outputFile.WriteLine("\"y\": 0,");
                    if (ev.GraphNode.EntityDescription != null)
                    {
                        outputFile.WriteLine($"\"title\":  \"{ev.GraphNode.EntityDescription.Replace(@"\", @"\\").Replace("\"", quoteEscaper).Trim() }\",");
                    }
                    else
                    {
                        outputFile.WriteLine($"\"title\":  \"\",");
                    }
                    outputFile.WriteLine($"\"icon\": \"/icons/{ev.GraphNode.EntityType}.png\"");

                    string end = "}";
                    if (counter++ < eventList.Count-1) { end = end + ","; }
                    outputFile.WriteLine(end);

                    // Collect link information
                    foreach (string parentId in ev.GraphNode.ParentId)
                    {
                        string parent = "";
                        if (parentId != null) { parent = parentId; }
                        links.Add((parent, ev.Id, ev.Risk.ToLower().Replace(" ", "-"), counter));
                    }
                }
                outputFile.WriteLine("],");
                outputFile.WriteLine("\"links\": [");
                counter = 0;
                foreach (var link in links)
                {
                    outputFile.WriteLine("{");
                    outputFile.WriteLine($"\"source\": {Ids.IndexOf(link.parent)},");
                    outputFile.WriteLine($"\"target\": {Ids.IndexOf(link.id)},");
                    outputFile.WriteLine($"\"risk\": \"{link.risk}\",");
                    outputFile.WriteLine($"\"label\": \"{link.step}.\"");
                    
                    string end = "}";
                    if (counter++ < links.Count-1) { end = end + ","; }
                    outputFile.WriteLine(end);
                }
                outputFile.WriteLine("]");
                outputFile.WriteLine("}");
                
            }

            string markdownTable = "| Event |\n| ----- |\n";
            int eventCounter = 1;
            foreach (var ev in eventList)
            {
                markdownTable += "| " + eventCounter++.ToString() + ". " + ev.Event + " |\n";
            }

            ViewBag.MarkdownTable = markdownTable;

            scenario.Name = scenario.Name.Replace(" ", "_");
            return View(scenario);

            // End Hijacking, following code is deprecated

            /*
            // write the text to a DOT file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine("wwwroot/graphs/", "graph.dot")))
            {
                foreach (string line in document)
                {
                    outputFile.WriteLine(line);
                }
            }

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"dot -Tpng -owwwroot/graphs/Graph.png wwwroot/graphs/graph.dot\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error)) { Console.WriteLine(output); }
            else { Console.WriteLine(error); }


            return View(stepList);
            */
        }

        #endregion
    
        #region Threat Trees

        [HttpGet]
        public async Task<IActionResult> ThreatTree(string treeId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            ThreatTree tree = await _treeService.GetByIdAsync(treeId);

            // Call out to a helper method to generate tree image

            return View(tree);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTree(string treeId, string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            Assessments assessment = await _assessmentsService.GetByIdAsync(assessmentId);
            assessment.ThreatTreeId = "";
            await _treeService.DeleteAsync(treeId);
            await _assessmentsService.UpdateAsync(assessmentId, assessment);
            return RedirectToAction("Scenarios", "Home", new { assessmentId = assessmentId});
        }

        [HttpGet]
        public async Task<IActionResult> CreateTree(string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            ThreatTree tree = new ThreatTree();
            tree = _treeService.CreateAsync(tree).Result;
            Assessments assessment = await _assessmentsService.GetByIdAsync(assessmentId);
            assessment.ThreatTreeId = tree.Id;
            await _assessmentsService.UpdateAsync(assessmentId, assessment);
            return RedirectToAction("EditTree", "Home", new { threatTreeId = tree.Id, assessmentId = assessmentId});
        }

        [HttpPost]
        public async Task<IActionResult> CreateTree(string assessmentId, List<string> categories, List<string> colors)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            ThreatTree tree = new ThreatTree();
            Dictionary<string, string> selectedColors = new Dictionary<string, string>();

            foreach (var color in colors)
            {
                selectedColors.Add(color, tree.ColorList.GetValueOrDefault(color));
            }

            List<string[]> selectedCategories = new List<string[]>();

            for (var i = 0; i < categories.Count; i++)
            {
                selectedCategories.Add(new string[]
                {
                    categories[i], selectedColors.ElementAt(i).Key, selectedColors.ElementAt(i).Value
                });
            }

            tree.Categories = selectedCategories;
            tree = _treeService.CreateAsync(tree).Result;

            Assessments assessment = await _assessmentsService.GetByIdAsync(assessmentId);
            assessment.ThreatTreeId = tree.Id;
            await _assessmentsService.UpdateAsync(assessmentId, assessment);
            return RedirectToAction("EditTree", "Home", new { threatTreeId = tree.Id, assessmentId = assessmentId });
        }

        [HttpGet]
        public async Task<IActionResult> EditTree(string threatTreeId, string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            if ( threatTreeId == null || threatTreeId == "")
            {
                return RedirectToAction("CreateTree", "Home", new { assessmentId = assessmentId });
            }
            else
            {
                ThreatTree tree = await _treeService.GetByIdAsync(threatTreeId);
                ViewBag.assessmentId = assessmentId;
                
                Assessments assessment = _assessmentsService.GetByIdAsync(assessmentId).Result;
                ViewBag.assessmentTitle = assessment.Name;

                List<Scenarios> scenarios = new List<Scenarios>();
                
                foreach (var scenarioId in assessment.Scenarios)
                {
                    scenarios.Add(_scenariosService.GetByIdAsync(scenarioId).Result);
                }
                
                List<Steps> stepsList = new List<Steps>();
                foreach (var scenario in scenarios)
                {
                    foreach (var stepId in scenario.Steps)
                    {
                        stepsList.Add(_stepsService.GetByIdAsync(stepId).Result);
                    }
                }

                // Leaving the above will cause duplicate MITRE IDs to be present
                // We will only take distinctly used IDs for our graph purposes
                List<string> steps = new List<string>();
                foreach (var step in stepsList)
                {
                    steps.Add(step.MitreId);
                }
                List<string> distinctSteps = new List<string>();
                distinctSteps = steps.Distinct().ToList();
                stepsList = new List<Steps>();
                foreach (var step in distinctSteps)
                {
                    if (step != null && step != "") { stepsList.Add(new Steps { MitreId = step }); }
                }

                ViewBag.stepCollection = stepsList;

                Techniques tech = new Techniques("");
                ViewBag.techniques = tech.techniquesFull;
                
                ViewBag.threatTreeNodeTemplate = new ThreatTreeNode();

                return View(tree);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditTree(
            // Graph Values
            string threatTreeId, 
            string assessmentId, 
            double edgeWidth, 
            string edgeType, 
            bool isRanked, 
            bool isClustered,
            bool isMerged,
            bool isMergeNode,
            string mergeEdgeType,
            bool isDigraph,
            int graphHeight,
            int graphWidth,
            string graphDirection,
            string fontFace,
            string fontColor,
            string arrowType,
            // Node values
            List<string> parentId,
            List<string> occurence,
            List<string> classification,
            List<string> attackId,
            List<string> attackDescription,
            List<string> customDescription,
            List<bool> isNodeFilled,
            List<bool> isBorderDiagonals,
            List<bool> isBorderRounded,
            List<bool> isBorderBold,
            List<bool> displayMitreId,
            List<string> borderStyle,
            List<string> displayStyle,
            List<string> nodeShape
            )
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            ThreatTree tree = _treeService.GetByIdAsync(threatTreeId).Result;
            
            // Apply Graph Settings
            tree.EdgeWidth = edgeWidth;
            tree.EdgeType = edgeType;
            tree.IsRanked = isRanked;
            tree.IsClustered = isClustered;
            tree.IsMerged = isMerged;
            tree.IsMergeNode = isMergeNode;
            tree.MergeEdgeType = mergeEdgeType;
            tree.IsDigraph = isDigraph;
            tree.GraphHeight = graphHeight;
            tree.GraphWidth = graphWidth;
            tree.GraphDirection = graphDirection;
            tree.FontFace = fontFace;
            tree.FontColor = fontColor;
            tree.ArrowType = arrowType;

            // Build out graph nodes
            List<ThreatTreeNode> nodeList = new List<ThreatTreeNode>();
            while (attackId.Count > 0)
            {
                // Build the node out
                ThreatTreeNode node = new ThreatTreeNode();
                
                // Simple Node Settings
                node.AttackId = attackId.First();
                node.AttackDescription = attackDescription.First();
                node.CustomDescription = customDescription.First();
                node.IsNodeFilled = isNodeFilled.First();
                node.IsBorderDiagonals = isBorderDiagonals.First();
                node.IsBorderRounded = isBorderRounded.First();
                node.IsBorderBold = isBorderBold.First();
                node.DisplayMitreId = displayMitreId.First();
                node.BorderStyle = borderStyle.First();
                node.DisplayStyle = displayStyle.First();
                node.NodeShape = nodeShape.First();

                // Iterate through occurences and remove as we go until we hit delimiter value '#null'
                List<string> occurenceList = new List<string>();
                while (true)
                {
                    if (occurence.First() == "#null")
                    {
                        occurence.RemoveAt(0);
                        break;
                    }
                    else
                    {
                        occurenceList.Add(occurence.First());
                        occurence.RemoveAt(0);
                    }
                }
                node.Occurence = occurenceList.ToArray();

                // Iterate through the classification and set the appropriate values, delimeter value '|'
                string[] split = classification.First().Split('|');
                node.Classification = new string[3];
                node.Classification[2] = split[2];
                node.Classification[1] = split[1];
                node.Classification[0] = split[0];

                // Iterate through the parent nodes and remove as we go until we hit delimeter value '#null'
                List<string> parentIdList = new List<string>();
                while (true)
                {
                    if (parentId.First() == "#null")
                    {
                        parentId.RemoveAt(0);
                        break;
                    }
                    else
                    {
                        parentIdList.Add(parentId.First());
                        parentId.RemoveAt(0);
                    }
                }
                node.ParentId = parentIdList.ToArray();

                // Remove this nodes data from the response data
                attackId.RemoveAt(0);
                attackDescription.RemoveAt(0);
                customDescription.RemoveAt(0);
                isNodeFilled.RemoveAt(0);
                isBorderDiagonals.RemoveAt(0);
                isBorderRounded.RemoveAt(0);
                isBorderBold.RemoveAt(0);
                displayMitreId.RemoveAt(0);
                borderStyle.RemoveAt(0);
                displayStyle.RemoveAt(0);
                classification.RemoveAt(0);
                nodeShape.RemoveAt(0);

                // Add the node to the list
                nodeList.Add(node);
            }

            // Add the node list to the tree
            tree.NodeList = nodeList;

            await _treeService.UpdateAsync(tree.Id, tree);

            return RedirectToAction("EditTree", "Home", new { assessmentId = assessmentId, threatTreeId = threatTreeId });

        }

        public async Task<IActionResult> EditTreeCategories(string[] categories, string[] colors, string threatTreeId, string assessmentId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            ThreatTree tree = await _treeService.GetByIdAsync(threatTreeId);

            // Build the new list of categories we want to use
            List<string[]> newCategories = new List<string[]>();
            for (int i = 0; i < categories.Length; i++)
            {
                newCategories.Add(new string[]
                {
                    categories[i], colors[i], tree.ColorList.GetValueOrDefault(colors[i])
                });
            }

            // Check to see which of the old categories are no longer used
            List<string> removeCategories = new List<string>();
            foreach (string[] category in tree.Categories)
            {
                if (!categories.Contains(category[0]))
                {
                    removeCategories.Add(category[0]);
                }
            }

            // Reset each of the nodes which belongs to the removed categories
            foreach (ThreatTreeNode node  in tree.NodeList)
            {
               if (removeCategories.Contains(node.Classification[0]))
               {
                    node.Classification = newCategories.First();
               }
               // If it is not to be reset, verify the colors are up to date
               else
                {
                    foreach (string[] category in newCategories)
                    {
                        if (node.Classification[0] == category[0]) { node.Classification = category; break; }
                    }
                }
            }

            // Set the tree categories to our new list
            tree.Categories = newCategories;

            // Update the tree
            await _treeService.UpdateAsync(tree.Id, tree);

            return RedirectToAction("EditTree", "Home", new { threatTreeId = tree.Id, assessmentId = assessmentId });
        }

        public async Task<IActionResult> ExportTree(string treeId)
        {
            if (!User.Identity.IsAuthenticated) { return RedirectToAction("Login", "Security"); }

            ThreatTree tree = await _treeService.GetByIdAsync(treeId);

            List<string> document = new List<string>();

            // Set up the graph settings
            string arrow = "-";

            if (tree.IsDigraph)
            {
                document.Add("digraph G {");
                arrow += ">";
            }
            else
            {
                document.Add("graph G {");
                arrow += "-";
            }
            document.Add("\tsize=\"" + tree.GraphHeight + "," + tree.GraphWidth + "\";");
            document.Add("\trankdir=\"" + tree.GraphDirection + "\";");
            document.Add("\tranksep=0.5;");
            document.Add("\tnodesep=0.75;");
            document.Add("\tsplines=\"" + tree.EdgeType + "\";");
            document.Add("\tpenwidth=0.0;");
            document.Add("\tedge [arrowhead=\"" + tree.ArrowType + "\",penwidth=" + tree.EdgeWidth + "];");

            Dictionary<string, string> mergeNodeTracker = new Dictionary<string, string>();

            // Build out the subgraphs
            foreach (var category in tree.Categories)
            {
                // Handle Clustering
                string cleanCategory = Regex.Replace(category[0], "[^0-9a-zA-Z_]+", "_");
                if (tree.IsClustered) { document.Add("\tsubgraph cluster_" + cleanCategory.ToLower() + " {"); }
                else { document.Add("\tsubgraph " + cleanCategory.ToLower() + " {"); }
                

                List<string> nodesInSubgraph = new List<string>();
                // Build out the nodes that reside in this subgraph
                if (tree.NodeList != null)
                {
                    foreach (var node in tree.NodeList)
                    {
                        if (node.Classification[0] == category[0])
                        {
                            nodesInSubgraph.Add("\"" + node.AttackId + "\"");

                            // Build a proper node title
                            string nodeTitle = "";
                            if (node.DisplayMitreId) { nodeTitle += node.AttackId + " - "; }
                            if (node.DisplayStyle == "attackDescription") { nodeTitle += node.AttackDescription; }
                            else if (node.DisplayStyle == "customDescription") { nodeTitle += node.CustomDescription; }
                            else if (node.DisplayStyle == "none" && node.DisplayMitreId) { nodeTitle = node.AttackId; }

                            // Build out the node's style
                            List<string> nodeStyleBuilder = new List<string>();
                            if (node.IsNodeFilled) { nodeStyleBuilder.Add("filled"); }
                            if (node.IsBorderRounded) { nodeStyleBuilder.Add("rounded"); }
                            if (node.IsBorderBold) { nodeStyleBuilder.Add("bold"); }
                            if (node.IsBorderDiagonals) { nodeStyleBuilder.Add("diagonals"); }
                            nodeStyleBuilder.Add(node.BorderStyle);

                            string nodeStyle = "";
                            foreach (var style in nodeStyleBuilder)
                            {
                                if (nodeStyle == "") { nodeStyle += style; }
                                else { nodeStyle += "," + style; }
                            }

                            // Build out the node occurences
                            string occurenceBuilder = "";
                            foreach (var occurence in node.Occurence)
                            {
                                if (occurence != "" && occurence != null) { occurenceBuilder += "<tr><td><font face=\"" + tree.FontFace + "\" color=\"" + tree.FontColor + "\">" + occurence + "</font></td></tr>"; }
                            }

                            // Add the node to the document
                            document.Add("\t\t\"" + node.AttackId.ToLower() + "\" [style=\"" + nodeStyle + "\",color=" + node.Classification[1] + ",shape=" + node.NodeShape + ",label=<<table><tr><td><b><font face=\"" + tree.FontFace + "\" color=\"" + tree.FontColor + "\">" + nodeTitle + "</font></b></td></tr>" + occurenceBuilder + "</table>>];");

                            // Check if node has multiple parents
                            if (node.ParentId.Length > 1 && tree.IsMerged)
                            {
                                // We need to track our merge nodes per each child node by ID
                                string mergeNodeName = "\"m" + node.AttackId.ToLower() + "\"";
                                mergeNodeTracker[node.AttackId.ToLower()] = mergeNodeName;
                                nodesInSubgraph.Add(mergeNodeName);

                                // Add the merge node to the subgraph
                                string nodeSize = "width=0.4,height=0.4";
                                if (!tree.IsMergeNode) { nodeSize = "width=0.01,height=0.01"; }
                                document.Add("\t" + mergeNodeName + " [color=" + node.Classification[1] + ",shape=point," + nodeSize + "];");
                            }
                        }
                    }
                }
                // Handle ranked trees
                if (tree.IsRanked)
                {
                    string rankBuilder = "\t\t{rank=same; ";
                    foreach (var subNode in  nodesInSubgraph) { rankBuilder += subNode.ToLower() + "; "; }
                    rankBuilder += "}";
                    document.Add(rankBuilder);
                }

                // Close the subgraph
                document.Add("\t}");
            }

            // Draw our edges
            if (tree.NodeList != null)
            {
                foreach (var node in tree.NodeList)
                {
                    // Handle merged edges
                    if (tree.IsMerged)
                    {
                        if (node.ParentId.Length > 1)
                        {
                            string edgeBuilder = "\t{";
                            foreach (var parent in node.ParentId)
                            {
                                edgeBuilder += "\"" + parent.ToLower() + "\"; ";
                            }
                            edgeBuilder = edgeBuilder.Substring(0, edgeBuilder.LastIndexOf(";")) + "} " + arrow + " " + mergeNodeTracker[node.AttackId.ToLower()] + " [color=" + node.Classification[1] + ",dir=none,style=" + tree.MergeEdgeType + "];";
                            document.Add(edgeBuilder);
                            document.Add("\t" + mergeNodeTracker[node.AttackId.ToLower()] + " " + arrow + " \"" + node.AttackId.ToLower() + "\" [color=" + node.Classification[1] + "];");
                        }
                        else
                        {
                            foreach (var parent in node.ParentId)
                            {
                                document.Add("\t\"" + parent.ToLower() + "\" " + arrow + " \"" + node.AttackId.ToLower() + "\" [color=" + node.Classification[1] + "];");
                            }
                        }
                    }
                    else
                    {
                        foreach (var parent in node.ParentId)
                        {
                            document.Add("\t\"" + parent.ToLower() + "\" " + arrow + " \"" + node.AttackId.ToLower() + "\" [color=" + node.Classification[1] + "];");
                        }
                    }
                }
            }
            // Close the graph
            document.Add("}");

            // write the text to a DOT file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine("wwwroot/graphs/", "tree.dot")))
            {
                foreach (string line in document)
                {
                    outputFile.WriteLine(line);
                }
            }

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"dot -Tpng -owwwroot/graphs/Tree.png wwwroot/graphs/tree.dot\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error)) { Console.WriteLine(output); }
            else { Console.WriteLine(error); }
            
            // Now we create the legend DOT file and generate the image
            document = new List<string>();
            document.Add("graph G{");
            document.Add("\tsize=\"8192,8192\";");
            document.Add("\trankdir=\"TB\";");
            document.Add("\tranksep=0.25;");
            document.Add("\tnodesep=0.75;");
            document.Add("\tpenwidth=0.0");
            document.Add("\tedge [penwidth=0.0];");

            int legendCounter = 0;
            string legendEdge = "";
            foreach (var category in tree.Categories)
            {
                string cleanCategory = Regex.Replace(category[0], "[^0-9a-zA-Z_]+", "_");
                document.Add("\tsubgraph " + cleanCategory.ToLower() + " {");
                document.Add("\t\t" + legendCounter + " [color=" + category[1] + ",style=\"rounded,filled\",shape=box,label=\"" + category[0] + "\"];");
                document.Add("\t\t{rank=same; " + legendCounter + ";}");
                document.Add("\t}");
                legendEdge += legendCounter.ToString() + " -- ";
                legendCounter++;
            }

            legendEdge = legendEdge.Substring(0, legendEdge.LastIndexOf(" -- "));
            document.Add("\t" + legendEdge + ";");

            document.Add("}");

            // write the text to a DOT file
            using (StreamWriter outputFile = new StreamWriter(Path.Combine("wwwroot/graphs/", "legend.dot")))
            {
                foreach (string line in document)
                {
                    outputFile.WriteLine(line);
                }
            }

            process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"dot -Tpng -owwwroot/graphs/Legend.png wwwroot/graphs/legend.dot\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error)) { Console.WriteLine(output); }
            else { Console.WriteLine(error); }

            string treePath = @"wwwroot/graphs/Tree.png";
            string legendPath = @"wwwroot/graphs/Legend.png";

            DateTime treeDate = System.IO.File.GetLastWriteTime(treePath);
            DateTime legendDate = System.IO.File.GetLastWriteTime(legendPath);

            ViewBag.TreeDate = treeDate;
            ViewBag.LegendDate = legendDate;

            return View();
        }

#endregion

    }
}
