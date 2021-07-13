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
            await _treeService.DeleteAsync(assessment.ThreatTreeId);

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
                        else { severityAndPervasiveness = (step.Pervasiveness + step.Severity) / 2; }

                        int sevper = (int)Math.Floor(severityAndPervasiveness);

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

                        if (sevper == 0) { worksheet.Cell(row, 13).Value = "N/A"; }
                        else { worksheet.Cell(row, 13).Value = sevper; }
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
            if (
                first.ThreatSource == "Outsider" || 
                first.ThreatSource == "Insider" || 
                first.ThreatSource == "Trusted Insider" || 
                first.ThreatSource == "Privileged Insider"
                )
            {
                nodeList.Add(new Node(null, "malicious-actor", first.ThreatSource, "", "root"));
            }
            else if (
                first.ThreatSource == "Partner Organization" ||
                first.ThreatSource == "Competitor Organization" ||
                first.ThreatSource == "Supplier Organization" ||
                first.ThreatSource == "Customer Organization"
                )
            {
                nodeList.Add(new Node(null, "group", first.ThreatSource, "", "root"));
            }
            else if (
                first.ThreatSource == "Ad Hoc Group" ||
                first.ThreatSource == "Established Group"
                )
            {
                nodeList.Add(new Node(null, "organization", first.ThreatSource, "", "root"));
            }
            else if (
                first.ThreatSource == "Nation State"
                )
            {
                nodeList.Add(new Node(null, "guard-personnel", first.ThreatSource, "", "root"));
            }
            else
            {
                nodeList.Add(new Node(null, "unknown-suspect", first.ThreatSource, "", "root"));
            }

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

                #region Techniques
                Dictionary<string, string> techniques = new Dictionary<string, string>();
                techniques["T1595"] = "Active Scanning";
                techniques["T1595/001"] = "Active Scanning: Scanning IP Blocks";
                techniques["T1595/002"] = "Active Scanning: Vulnerability Scanning";
                techniques["T1592"] = "Gather Victim Host Information";
                techniques["T1592/001"] = "Gather Victim Host Information: Hardware";
                techniques["T1592/002"] = "Gather Victim Host Information: Software";
                techniques["T1592/003"] = "Gather Victim Host Information: Firmware";
                techniques["T1592/004"] = "Gather Victim Host Information: Client Configurations";
                techniques["T1589"] = "Gather Victim Identity Information";
                techniques["T1589/001"] = "Gather Victim Identity Information: Credentials";
                techniques["T1589/002"] = "Gather Victim Identity Information: Email Addresses";
                techniques["T1589/003"] = "Gather Victim Identity Information: Employee Names";
                techniques["T1590"] = "Gather Victim Network Information";
                techniques["T1590/001"] = "Gather Victim Network Information: Domain Properties";
                techniques["T1590/002"] = "Gather Victim Network Information: DNS";
                techniques["T1590/003"] = "Gather Victim Network Information: Network Trust Dependencies";
                techniques["T1590/004"] = "Gather Victim Network Information: Network Topology";
                techniques["T1590/005"] = "Gather Victim Network Information: IP Addresses";
                techniques["T1590/006"] = "Gather Victim Network Information: Network Security Appliances";
                techniques["T1591"] = "Gather Victim Org Information";
                techniques["T1591/001"] = "Gather Victim Org Information: Determine Physical Locations";
                techniques["T1591/002"] = "Gather Victim Org Information: Business Relationships";
                techniques["T1591/003"] = "Gather Victim Org Information: Identify Business Tempo";
                techniques["T1591/004"] = "Gather Victim Org Information: Identify Roles";
                techniques["T1598"] = "Phishing for Information";
                techniques["T1598/001"] = "Phishing for Information: Spearphishing Service";
                techniques["T1598/002"] = "Phishing for Information: Spearphishing Attachment";
                techniques["T1598/003"] = "Phishing for Information: Spearphishing Link";
                techniques["T1597"] = "Search Closed Sources";
                techniques["T1597/001"] = "Search Closed Sources: Threat Intel Vendors";
                techniques["T1597/002"] = "Search Closed Sources: Purchase Technical Data";
                techniques["T1596"] = "Search Open Technical Databases";
                techniques["T1596/001"] = "Search Open Technical Databases: DNS/Passive DNS";
                techniques["T1596/002"] = "Search Open Technical Databases: WHOIS";
                techniques["T1596/003"] = "Search Open Technical Databases: Digital Certificates";
                techniques["T1596/004"] = "Search Open Technical Databases: CDNs";
                techniques["T1596/005"] = "Search Open Technical Databases: Scan Databases";
                techniques["T1593"] = "Search Open Websites/Domains";
                techniques["T1593/001"] = "Search Open Websites/Domains: Social Media";
                techniques["T1593/002"] = "Search Open Websites/Domains: Search Engines";
                techniques["T1594"] = "Search Victim-Owned Websites";
                techniques["T1583"] = "Acquire Infrastructure";
                techniques["T1583/001"] = "Acquire Infrastructure: Domains";
                techniques["T1583/002"] = "Acquire Infrastructure: DNS Server";
                techniques["T1583/003"] = "Acquire Infrastructure: Virtual Private Server";
                techniques["T1583/004"] = "Acquire Infrastructure: Server";
                techniques["T1583/005"] = "Acquire Infrastructure: Botnet";
                techniques["T1583/006"] = "Acquire Infrastructure: Web Services";
                techniques["T1586"] = "Compromise Accounts";
                techniques["T1586/001"] = "Compromise Accounts: Social Media Accounts";
                techniques["T1586/002"] = "Compromise Accounts: Email Accounts";
                techniques["T1584"] = "Compromise Infrastructure";
                techniques["T1584/001"] = "Compromise Infrastructure: Domains";
                techniques["T1584/002"] = "Compromise Infrastructure: DNS Server";
                techniques["T1584/003"] = "Compromise Infrastructure: Virtual Private Server";
                techniques["T1584/004"] = "Compromise Infrastructure: Server";
                techniques["T1584/005"] = "Compromise Infrastructure: Botnet";
                techniques["T1584/006"] = "Compromise Infrastructure: Web Services";
                techniques["T1587"] = "Develop Capabilities";
                techniques["T1587/001"] = "Develop Capabilities: Malware";
                techniques["T1587/002"] = "Develop Capabilities: Code Signing Certificates";
                techniques["T1587/003"] = "Develop Capabilities: Digital Certificates";
                techniques["T1587/004"] = "Develop Capabilities: Exploits";
                techniques["T1585"] = "Establish Accounts";
                techniques["T1585/001"] = "Establish Accounts: Social Media Accounts";
                techniques["T1585/002"] = "Establish Accounts: Email Accounts";
                techniques["T1588"] = "Obtain Capabilities";
                techniques["T1588/001"] = "Obtain Capabilities: Malware";
                techniques["T1588/002"] = "Obtain Capabilities: Tool";
                techniques["T1588/003"] = "Obtain Capabilities: Code Signing Certificates";
                techniques["T1588/004"] = "Obtain Capabilities: Digital Certificates";
                techniques["T1588/005"] = "Obtain Capabilities: Exploits";
                techniques["T1588/006"] = "Obtain Capabilities: Vulnerabilities";
                techniques["T1608"] = "Stage Capabilities";
                techniques["T1608/001"] = "Stage Capabilities: Upload Malware";
                techniques["T1608/002"] = "Stage Capabilities: Upload Tool";
                techniques["T1608/003"] = "Stage Capabilities: Install Digital Certificate";
                techniques["T1608/004"] = "Stage Capabilities: Drive-by Target";
                techniques["T1608/005"] = "Stage Capabilities: Link Target";
                techniques["T1189"] = "Drive-by Compromise";
                techniques["T1190"] = "Exploit Public-Facing Application";
                techniques["T1133"] = "External Remote Services";
                techniques["T1200"] = "Hardware Additions";
                techniques["T1566"] = "Phishing";
                techniques["T1566/001"] = "Phishing: Spearphishing Attachment";
                techniques["T1566/002"] = "Phishing: Spearphishing Link";
                techniques["T1566/003"] = "Phishing: Spearphishing via Service";
                techniques["T1091"] = "Replication Through Removable Media";
                techniques["T1195"] = "Supply Chain Compromise";
                techniques["T1195/001"] = "Supply Chain Compromise: Compromise Software Dependencies and Development Tools";
                techniques["T1195/002"] = "Supply Chain Compromise: Compromise Software Supply Chain";
                techniques["T1195/003"] = "Supply Chain Compromise: Compromise Hardware Supply Chain";
                techniques["T1199"] = "Trusted Relationship";
                techniques["T1078"] = "Valid Accounts";
                techniques["T1078/001"] = "Valid Accounts: Default Accounts";
                techniques["T1078/002"] = "Valid Accounts: Domain Accounts";
                techniques["T1078/003"] = "Valid Accounts: Local Accounts";
                techniques["T1078/004"] = "Valid Accounts: Cloud Accounts";
                techniques["T1059"] = "Command and Scripting Interpreter";
                techniques["T1059/001"] = "Command and Scripting Interpreter: PowerShell";
                techniques["T1059/002"] = "Command and Scripting Interpreter: AppleScript";
                techniques["T1059/003"] = "Command and Scripting Interpreter: Windows Command Shell";
                techniques["T1059/004"] = "Command and Scripting Interpreter: Unix Shell";
                techniques["T1059/005"] = "Command and Scripting Interpreter: Visual Basic";
                techniques["T1059/006"] = "Command and Scripting Interpreter: Python";
                techniques["T1059/007"] = "Command and Scripting Interpreter: JavaScript";
                techniques["T1059/008"] = "Command and Scripting Interpreter: Network Device CLI";
                techniques["T1609"] = "Container Administration Command";
                techniques["T1610"] = "Deploy Container";
                techniques["T1203"] = "Exploitation for client execution";
                techniques["T1559"] = "Inter-process communication";
                techniques["T1559/001"] = "Inter-process communication: Component Object Model";
                techniques["T1559/002"] = "Inter-process communication: Dynamic Data Exchange";
                techniques["T1106"] = "Native API";
                techniques["T1053"] = "Scheduled Task/Job";
                techniques["T1053/001"] = "Scheduled Task/Job: At (Linux)";
                techniques["T1053/002"] = "Scheduled Task/Job: At (Windows)";
                techniques["T1053/003"] = "Scheduled Task/Job: Cron";
                techniques["T1053/004"] = "Scheduled Task/Job: Launchd";
                techniques["T1053/005"] = "Scheduled Task/Job: Scheduled task";
                techniques["T1053/006"] = "Scheduled Task/Job: Systemd Timers";
                techniques["T1053/007"] = "Scheudled Task/Job: Container Orchestration Job";
                techniques["T1129"] = "Shared Modules";
                techniques["T1072"] = "Software Development Tools";
                techniques["T1569"] = "System Services";
                techniques["T1569/001"] = "System Services: Launchctl";
                techniques["T1569/002"] = "System Services: Service Execution";
                techniques["T1204"] = "User Execution";
                techniques["T1204/001"] = "User Execution: Malicious Link";
                techniques["T1204/002"] = "User Execution: Malicious File";
                techniques["T1204/003"] = "User Execution: Malicious Image";
                techniques["T1047"] = "Windows Management Instrumentation";
                techniques["T1098"] = "Account Manipulation";
                techniques["T1098/001"] = "Account Manipulation: Additional Cloud Credentials";
                techniques["T1098/002"] = "Account Manipulation: Exchange Email Delegate Permissions";
                techniques["T1098/003"] = "Account Manipulation: Add Office 365 Global Administrator Role";
                techniques["T1098/004"] = "Account Manipulation: SSH Authorized Keys";
                techniques["T1197"] = "BITS Jobs";
                techniques["T1547"] = "Boot or Logon Autostart Execution";
                techniques["T1547/001"] = "Boot or Logon Autostart Execution: Registry Run Keys / Startup Folder";
                techniques["T1547/002"] = "Boot or Logon Autostart Execution: Authentication Package";
                techniques["T1547/003"] = "Boot or Logon Autostart Execution: Time Providers";
                techniques["T1547/004"] = "Boot or Logon Autostart Execution: Winlogon Helper DLL";
                techniques["T1547/005"] = "Boot or Logon Autostart Execution: Security Support Provider";
                techniques["T1547/006"] = "Boot or Logon Autostart Execution: Kernel Modules and Extensions";
                techniques["T1547/007"] = "Boot or Logon Autostart Execution: Re-opened Applications";
                techniques["T1547/008"] = "Boot or Logon Autostart Execution: LSASS Driver";
                techniques["T1547/009"] = "Boot or Logon Autostart Execution: Shortcut Modification";
                techniques["T1547/010"] = "Boot or Logon Autostart Execution: Port Monitors";
                techniques["T1547/011"] = "Boot or Logon Autostart Execution: Plist Modification";
                techniques["T1547/012"] = "Boot or Logon Autostart Execution: Print Processors";
                techniques["T1547/013"] = "Boot or Logon Autostart Execution: XDG Autostart Entries";
                techniques["T1547/014"] = "Boot or Logon Autostart Execution: Active Setup";
                techniques["T1037"] = "Boot or Logon Initialization Scripts";
                techniques["T1037/001"] = "Boot or Logon Initialization Scripts: Logon Script (Windows)";
                techniques["T1037/002"] = "Boot or Logon Initialization Scripts: Logon Script (Linux)";
                techniques["T1037/003"] = "Boot or Logon Initialization Scripts: Network Logon Script";
                techniques["T1037/004"] = "Boot or Logon Initialization Scripts: RC Scripts";
                techniques["T1037/005"] = "Boot or Logon Initialization Scripts: Startup Items";
                techniques["T1176"] = "Browser Extensions";
                techniques["T1554"] = "Compromise Client Software Binary";
                techniques["T1136"] = "Create Account";
                techniques["T1136/001"] = "Create Account: Local Account";
                techniques["T1136/002"] = "Create Account: Domain Account";
                techniques["T1136/003"] = "Create Account: Cloud Account";
                techniques["T1543"] = "Create or modify system process";
                techniques["T1543/001"] = "Create or modify system process: Launch agent";
                techniques["T1543/002"] = "Create or modify system process: Systemd service";
                techniques["T1543/003"] = "Create or modify system process: Windows service";
                techniques["T1543/004"] = "Create or modify system process: Launch daemon";
                techniques["T1546"] = "Event Triggered Execution";
                techniques["T1546/001"] = "Event Triggered Execution: Change default file association";
                techniques["T1546/002"] = "Event Triggered Execution: Screensaver";
                techniques["T1546/003"] = "Event Triggered Execution: Windows Management Instrumentation Event Subscription";
                techniques["T1546/004"] = "Event Triggered Execution: Unix Shell Configuration Modification";
                techniques["T1546/005"] = "Event Triggered Execution: Trap";
                techniques["T1546/006"] = "Event Triggered Execution: LC_LOAD_DYLIB Addition";
                techniques["T1546/007"] = "Event Triggered Execution: Netsh helper DLL";
                techniques["T1546/008"] = "Event Triggered Execution: Accessibility features";
                techniques["T1546/009"] = "Event Triggered Execution: AppCert DLLs";
                techniques["T1546/010"] = "Event Triggered Execution: AppInit DLLs";
                techniques["T1546/011"] = "Event Triggered Execution: Application Shimming";
                techniques["T1546/012"] = "Event Triggered Execution: Image File Execution Options Injection";
                techniques["T1546/013"] = "Event Triggered Execution: PowerShell profile";
                techniques["T1546/014"] = "Event Triggered Execution: Emond";
                techniques["T1546/015"] = "Event Triggered Execution: Component Object Model Hijacking";
                techniques["T1133"] = "External Remote Services";
                techniques["T1574"] = "Hijack Execution Flow";
                techniques["T1574/001"] = "Hijack Execution Flow: DLL Search Order Hijacking";
                techniques["T1574/002"] = "Hijack Execution Flow: DLL Side-Loading";
                techniques["T1574/004"] = "Hijack Execution Flow: Dylib Hijacking";
                techniques["T1574/005"] = "Hijack Execution Flow: Executable Installer File Permissions Weakness";
                techniques["T1574/006"] = "Hijack Execution Flow: Dynamic Linker Hijacking";
                techniques["T1574/007"] = "Hijack Execution Flow: Path Interception by PATH Environment Variable";
                techniques["T1574/008"] = "Hijack Execution Flow: Path Interception by Search Order Hijacking";
                techniques["T1574/009"] = "Hijack Execution Flow: Path Interception by Unquoted Path";
                techniques["T1574/010"] = "Hijack Execution Flow: Services File Permissions Weakness";
                techniques["T1574/011"] = "Hijack Execution Flow: Services Registry Permissions Weakness";
                techniques["T1574/012"] = "Hijack Execution Flow: COR_PROFILER";
                techniques["T1525"] = "Implant Internal Image";
                techniques["T1556"] = "Modify Authentication Process";
                techniques["T1556/001"] = "Modify Authentication Process: Domain Controller Authentication";
                techniques["T1556/002"] = "Modify Authentication Process: Password Filter DLL";
                techniques["T1556/003"] = "Modify Authentication Process: Pluggable Authentication Modules";
                techniques["T1556/004"] = "Modify Authentication Process: Network Device Authentication";
                techniques["T1137"] = "Office application startup";
                techniques["T1137/001"] = "Office application startup: Office template macros";
                techniques["T1137/002"] = "Office application startup: Office test";
                techniques["T1137/003"] = "Office application startup: Outlook forms";
                techniques["T1137/004"] = "Office application startup: Outlook home page";
                techniques["T1137/005"] = "Office application startup: Outlook rules";
                techniques["T1137/006"] = "Office application startup: Add-ins";
                techniques["T1542"] = "Pre-OS boot";
                techniques["T1542/001"] = "Pre-OS boot: System firmware";
                techniques["T1542/002"] = "Pre-OS boot: Component firmware";
                techniques["T1542/003"] = "Pre-OS boot: Bootkit";
                techniques["T1542/004"] = "Pre-OS boot: ROMMONkit";
                techniques["T1542/005"] = "Pre-OS boot: TFTP Boot";
                techniques["T1053"] = "Scheduled Task/Job";
                techniques["T1053/001"] = "Scheduled Task/Job: At (Linux)";
                techniques["T1053/002"] = "Scheduled Task/Job: At (Windows)";
                techniques["T1053/003"] = "Scheduled Task/Job: Cron";
                techniques["T1053/004"] = "Scheduled Task/Job: Launchd";
                techniques["T1053/005"] = "Scheduled Task/Job: Scheduled task";
                techniques["T1053/006"] = "Scheduled Task/Job: Systemd Timers";
                techniques["T1053/007"] = "Scheduled Task/Job: Container Orchestration Job";
                techniques["T1505"] = "Server software component";
                techniques["T1505/001"] = "Server software component: SQL stored procedures";
                techniques["T1505/002"] = "Server software component: Transport agent";
                techniques["T1505/003"] = "Server software component: Web shell";
                techniques["T1205"] = "Traffic Signaling";
                techniques["T1205/001"] = "Traffic Signaling: Port Knocking";
                techniques["T1078"] = "Valid accounts";
                techniques["T1078/001"] = "Valid accounts: Default accounts";
                techniques["T1078/002"] = "Valid accounts: Domain accounts";
                techniques["T1078/003"] = "Valid accounts: Local accounts";
                techniques["T1078/004"] = "Valid accounts: Cloud accounts";
                techniques["T1548"] = "Abuse elevation control mechanism";
                techniques["T1548/001"] = "Abuse elevation control mechanism: Setuid and Setgid";
                techniques["T1548/002"] = "Abuse elevation control mechanism: Bypass user access control";
                techniques["T1548/003"] = "Abuse elevation control mechanism: Sudo and sudo control";
                techniques["T1548/004"] = "Abuse elevation control mechanism: Elevated execution with prompt";
                techniques["T1134"] = "Access Token Manipulation";
                techniques["T1134/001"] = "Access Token Manipulation: Token Impersonation/Theft";
                techniques["T1134/002"] = "Access Token Manipulation: Create Process with Token";
                techniques["T1134/003"] = "Access Token Manipulation: Make and Impersonate Token";
                techniques["T1134/004"] = "Access Token Manipulation: Parent PID Spoofing";
                techniques["T1134/005"] = "Access Token Manipulation: SID-History Injection";
                techniques["T1547"] = "Boot or Logon Autostart Execution";
                techniques["T1547/001"] = "Boot or Logon Autostart Execution: Registry Run Keys / Startup Folder";
                techniques["T1547/002"] = "Boot or Logon Autostart Execution: Authenticaton Package";
                techniques["T1547/003"] = "Boot or Logon Autostart Execution: Time Providers";
                techniques["T1547/004"] = "Boot or Logon Autostart Execution: Winlogon Helper DLL";
                techniques["T1547/005"] = "Boot or Logon Autostart Execution: Security Support Provider";
                techniques["T1547/006"] = "Boot or Logon Autostart Execution: Kernel Modules and Extensions";
                techniques["T1547/007"] = "Boot or Logon Autostart Execution: Re-opened Applications";
                techniques["T1547/008"] = "Boot or Logon Autostart Execution: LSASS Driver";
                techniques["T1547/009"] = "Boot or Logon Autostart Execution: Shortcut Modification";
                techniques["T1547/010"] = "Boot or Logon Autostart Execution: Port Monitors";
                techniques["T1547/011"] = "Boot or Logon Autostart Execution: Plist Modification";
                techniques["T1547/012"] = "Boot or Logon Autostart Execution: Print Processors";
                techniques["T1547/013"] = "Boot or Logon Autostart Execution: XDG Autostart Entries";
                techniques["T1547/014"] = "Boot or Logon Autostart Execution: Active Setup";
                techniques["T1037"] = "Boot or Logon Initialization Scripts";
                techniques["T1037/001"] = "Boot or Logon Initialization Scripts: Logon Script (Windows)";
                techniques["T1037/002"] = "Boot or Logon Initialization Scripts: Logon Script (Linux)";
                techniques["T1037/003"] = "Boot or Logon Initialization Scripts: Network Logon Script";
                techniques["T1037/004"] = "Boot or Logon Initialization Scripts: RC Scrits";
                techniques["T1037/005"] = "Boot or Logon Initialization Scripts: Startup Items";
                techniques["T1543"] = "Create or modify system process";
                techniques["T1543/001"] = "Create or modify system process: Launch agent";
                techniques["T1543/002"] = "Create or modify system process: Systemd service";
                techniques["T1543/003"] = "Create or modify system process: Windows service";
                techniques["T1543/004"] = "Create or modify system process: Launch daemon";
                techniques["T1484"] = "Domain Policy Modification";
                techniques["T1484/001"] = "Domain Policy Modification: Group Policy Modification";
                techniques["T1484/002"] = "Domain Policy Modification: Domain Trust Modification";
                techniques["T1611"] = "Escape to Host";
                techniques["T1546"] = "Event Triggered Execution";
                techniques["T1546/001"] = "Event Triggered Execution: Change default file association";
                techniques["T1546/002"] = "Event Triggered Execution: Screensaver";
                techniques["T1546/003"] = "Event Triggered Execution: Windows Management Instrumentation Event Subscription";
                techniques["T1546/004"] = "Event Triggered Execution: Unix Shell Configuration Modification";
                techniques["T1546/005"] = "Event Triggered Execution: Trap";
                techniques["T1546/006"] = "Event Triggered Execution: LC_LOAD_DYLIB Addition";
                techniques["T1546/007"] = "Event Triggered Execution: Netsh helper DLL";
                techniques["T1546/008"] = "Event Triggered Execution: Accessibility features";
                techniques["T1546/009"] = "Event Triggered Execution: AppCert DLLs";
                techniques["T1546/010"] = "Event Triggered Execution: AppInit DLLs";
                techniques["T1546/011"] = "Event Triggered Execution: Application Shimming";
                techniques["T1546/012"] = "Event Triggered Execution: Image File Execution Options Injection";
                techniques["T1546/013"] = "Event Triggered Execution: PowerShell profile";
                techniques["T1546/014"] = "Event Triggered Execution: Emond";
                techniques["T1546/015"] = "Event Triggered Execution: Component Object Model Hijacking";
                techniques["T1068"] = "Exploitation for Privilege Escalation";
                techniques["T1574"] = "Hijack Execution Flow";
                techniques["T1574/001"] = "Hijack Execution Flow: DLL Search Order Hijacking";
                techniques["T1574/002"] = "Hijack Execution Flow: DLL Side-Loading";
                techniques["T1574/004"] = "Hijack Execution Flow: Dylib Hijacking";
                techniques["T1574/005"] = "Hijack Execution Flow: Executable Installer File Permissions Weakness";
                techniques["T1574/006"] = "Hijack Execution Flow: Dynamic Linker Hijacking";
                techniques["T1574/007"] = "Hijack Execution Flow: Path Interception by PATH Environment Variable";
                techniques["T1574/008"] = "Hijack Execution Flow: Path Interception by Search Order Hijacking";
                techniques["T1574/009"] = "Hijack Execution Flow: Path Interception by Unquoted Path";
                techniques["T1574/010"] = "Hijack Execution Flow: Services File Permissions Weakness";
                techniques["T1574/011"] = "Hijack Execution Flow: Services Registry Permissions Weakness";
                techniques["T1574/012"] = "Hijack Execution Flow: COR_PROFILER";
                techniques["T1055"] = "Process Injection";
                techniques["T1055/001"] = "Process Injection: Dynamic-link Library Injection";
                techniques["T1055/002"] = "Process Injection: Portable Executable Injection";
                techniques["T1055/003"] = "Process Injection: Thread Execution Hijacking";
                techniques["T1055/004"] = "Process Injection: Asynchronous Procedure Call";
                techniques["T1055/005"] = "Process Injection: Thread Local Storage";
                techniques["T1055/008"] = "Process Injection: Ptrace System Calls";
                techniques["T1055/009"] = "Process Injection: Proc Memory";
                techniques["T1055/011"] = "Process Injection: Extra Window Memory Injection";
                techniques["T1055/012"] = "Process Injection: Process Hollowing";
                techniques["T1055/013"] = "Process Injection: Process Doppelganging";
                techniques["T1055/014"] = "Process Injection: VDSO Hijacking";
                techniques["T1053"] = "Scheduled Task/Job";
                techniques["T1053/001"] = "Scheduled Task/Job: At (Linux)";
                techniques["T1053/002"] = "Scheduled Task/Job: At (Windows)";
                techniques["T1053/003"] = "Scheduled Task/Job: Cron";
                techniques["T1053/004"] = "Scheduled Task/Job: Launchd";
                techniques["T1053/005"] = "Scheduled Task/Job: Scheduled task";
                techniques["T1053/006"] = "Scheduled Task/Job: Systemd Timers";
                techniques["T1053/007"] = "Scheduled Task/Job: Container Orchestration Job";
                techniques["T1078"] = "Valid Accounts";
                techniques["T1078/001"] = "Valid Accounts: Default Accounts";
                techniques["T1078/002"] = "Valid Accounts: Domain Accounts";
                techniques["T1078/003"] = "Valid Accounts: Local Accounts";
                techniques["T1078/004"] = "Valid Accounts: Cloud Accounts";
                techniques["T1548"] = "Abuse elevation control mechanism";
                techniques["T1548/001"] = "Abuse elevation control mechanism: Setuid and setgid";
                techniques["T1548/002"] = "Abuse elevation control mechanism: Bypass user access control";
                techniques["T1548/003"] = "Abuse elevation control mechanism: Sudo and sudo caching";
                techniques["T1548/004"] = "Abuse elevation control mechanism: Elevated execution with prompt";
                techniques["T1134"] = "Access Token Manipulation";
                techniques["T1134/001"] = "Access Token Manipulation: Token Impersonation/Theft";
                techniques["T1134/002"] = "Access Token Manipulation: Create Process with Token";
                techniques["T1134/003"] = "Access Token Manipulation: Make and Impersonate Token";
                techniques["T1134/004"] = "Access Token Manipulation: Parent PID Spoofing";
                techniques["T1134/005"] = "Access Token Manipulation: SID-History Injection";
                techniques["T1197"] = "BITS Jobs";
                techniques["T1610"] = "Deploy Container";
                techniques["T1612"] = "Build Image on Host";
                techniques["T1140"] = "Deobfuscate/decode files or information";
                techniques["T1006"] = "Direct Volume Access";
                techniques["T1480"] = "Execution Guardrails";
                techniques["T1480/001"] = "Execution Guardrails: Environmental Keying";
                techniques["T1211"] = "Exploitation for defense evasion";
                techniques["T1222"] = "File and directory permissions modificaton";
                techniques["T1222/001"] = "File and directory permissions modificaton: Windows file and directory permissions modification";
                techniques["T1222/002"] = "File and directory permissions modificaton: Linux and Mac file and directory permissions modification";
                techniques["T1564"] = "Hide artifacts";
                techniques["T1564/001"] = "Hide artifacts: Hidden files and directories";
                techniques["T1564/002"] = "Hide artifacts: Hidden users";
                techniques["T1564/003"] = "Hide artifacts: Hidden window";
                techniques["T1564/004"] = "Hide artifacts: NTFS file attributes";
                techniques["T1564/005"] = "Hide artifacts: Hidden file system";
                techniques["T1564/006"] = "Hide artifacts: Run virtual instance";
                techniques["T1564/007"] = "Hide artifacts: VBA Stomping";
                techniques["T1574"] = "Hijack Execution Flow";
                techniques["T1574/001"] = "Hijack Execution Flow: DLL Search Order Hijacking";
                techniques["T1574/002"] = "Hijack Execution Flow: DLL Side-Loading";
                techniques["T1574/004"] = "Hijack Execution Flow: Dylib Hijacking";
                techniques["T1574/005"] = "Hijack Execution Flow: Executable Intaller File Permissions Weakness";
                techniques["T1574/006"] = "Hijack Execution Flow: Dynamic Linker Hijacking";
                techniques["T1574/007"] = "Hijack Execution Flow: Path Interception by PATH Environment Variable";
                techniques["T1574/008"] = "Hijack Execution Flow: Path Interception by Search Order Hijacking";
                techniques["T1574/009"] = "Hijack Execution Flow: Path Interception by Unquoted Path";
                techniques["T1574/010"] = "Hijack Execution Flow: Services File Permissions Weakness";
                techniques["T1574/011"] = "Hijack Execution Flow: Services Registry Permissions Weakness";
                techniques["T1574/012"] = "Hijack Execution Flow: COR_PROFILER";
                techniques["T1562"] = "Impair Defenses";
                techniques["T1562/001"] = "Impair Defenses: Disable or Modify Tools";
                techniques["T1562/002"] = "Impair Defenses: Disable Windows Event Logging";
                techniques["T1562/003"] = "Impair Defenses: Impair Command History Logging";
                techniques["T1562/004"] = "Impair Defenses: Disable or Modify System Firewall";
                techniques["T1562/006"] = "Impair Defenses: Indicator Blocking";
                techniques["T1562/007"] = "Impair Defenses: Disable or Modify Cloud Firewall";
                techniques["T1562/008"] = "Impair Defenses: Disable Cloud Logs";
                techniques["T1070"] = "Indicator Removal on Host";
                techniques["T1070/001"] = "Indicator Removal on Host: Clear Windows Event Logs";
                techniques["T1070/002"] = "Indicator Removal on Host: Clear Linux or Mac System Logs";
                techniques["T1070/003"] = "Indicator Removal on Host: Clear Command History";
                techniques["T1070/004"] = "Indicator Removal on Host: File Deletion";
                techniques["T1070/005"] = "Indicator Removal on Host: Network Share Connection Removal";
                techniques["T1070/006"] = "Indicator Removal on Host: Timestomp";
                techniques["T1202"] = "Indirect Command Execution";
                techniques["T1036"] = "Masquerading";
                techniques["T1036/001"] = "Masquerading: Invalid Code Signature";
                techniques["T1036/002"] = "Masquerading: Right-to-Left Override";
                techniques["T1036/003"] = "Masquerading: Rename System Utilities";
                techniques["T1036/004"] = "Masquerading: Masquerade Task or Service";
                techniques["T1036/005"] = "Masquerading: Match Legitimate Name or Location";
                techniques["T1036/006"] = "Masquerading: Space after Filename";
                techniques["T1556"] = "Modify Authentication Process";
                techniques["T1556/001"] = "Modify Authentication Process: Domain Controller Authentication";
                techniques["T1556/002"] = "Modify Authentication Process: Password Filter DLL";
                techniques["T1556/003"] = "Modify Authentication Process: Pluggable Authentication Modules";
                techniques["T1556/004"] = "Modify Authentication Process: Network Device Authentication";
                techniques["T1578"] = "Modify Cloud Compute Infrastructure";
                techniques["T1578/001"] = "Modify Cloud Compute Infrastructure: Create Snapshot";
                techniques["T1578/002"] = "Modify Cloud Compute Infrastructure: Create Cloud Instance";
                techniques["T1578/003"] = "Modify Cloud Compute Infrastructure: Delete Cloud Instance";
                techniques["T1578/004"] = "Modify Cloud Compute Infrastructure: Revert Cloud Instance";
                techniques["T1112"] = "Modify Registry";
                techniques["T1601"] = "Modify System Image";
                techniques["T1601/001"] = "Modify System Image: Patch System Image";
                techniques["T1601/002"] = "Modify System Image: Downgrade System Image";
                techniques["T1599"] = "Network Boundary Bridging";
                techniques["T1599/001"] = "Network Boundary Bridging: Network Address Translation Traversal";
                techniques["T1027"] = "Obfuscated Files or Information";
                techniques["T1027/001"] = "Obfuscated Files or Information: Binary Padding";
                techniques["T1027/002"] = "Obfuscated Files or Information: Software Packing";
                techniques["T1027/003"] = "Obfuscated Files or Information: Steganography";
                techniques["T1027/004"] = "Obfuscated Files or Information: Compile after Delivery";
                techniques["T1027/005"] = "Obfuscated Files or Information: Indicator Removal from Tools";
                techniques["T1542"] = "Pre-OS boot";
                techniques["T1542/001"] = "Pre-OS boot: System Firmware";
                techniques["T1542/002"] = "Pre-OS boot: Component Firmware";
                techniques["T1542/003"] = "Pre-OS boot: Bootkit";
                techniques["T1542/004"] = "Pre-OS boot: ROMMONkit";
                techniques["T1542/005"] = "Pre-OS boot: TFTP Boot";
                techniques["T1055"] = "Process Injection";
                techniques["T1055/001"] = "Process Injection: Dynamic-link Library Injection";
                techniques["T1055/002"] = "Process Injection: Portable Executable Injection";
                techniques["T1055/003"] = "Process Injection: Thread Execution Hijacking";
                techniques["T1055/004"] = "Process Injection: Asynchronous Procedure Call";
                techniques["T1055/005"] = "Process Injection: Thread Local Storage";
                techniques["T1055/008"] = "Process Injection: Ptrace System Calls";
                techniques["T1055/009"] = "Process Injection: Proc Memory";
                techniques["T1055/011"] = "Process Injection: Extra Window Memory Injection";
                techniques["T1055/012"] = "Process Injection: Process Hollowing";
                techniques["T1055/013"] = "Process Injection: Process Doppelganging";
                techniques["T1055/014"] = "Process Injection: VDSO Hijacking";
                techniques["T1207"] = "Rogue Domain Controller";
                techniques["T1014"] = "Rootkit";
                techniques["T1218"] = "Signed Binary Proxy Execution";
                techniques["T1218/001"] = "Signed Binary Proxy Execution: Compiled HTML File";
                techniques["T1218/002"] = "Signed Binary Proxy Execution: Control Panel";
                techniques["T1218/003"] = "Signed Binary Proxy Execution: CMSTP";
                techniques["T1218/004"] = "Signed Binary Proxy Execution: InstallUtil";
                techniques["T1218/005"] = "Signed Binary Proxy Execution: Mshta";
                techniques["T1218/007"] = "Signed Binary Proxy Execution: Msiexec";
                techniques["T1218/008"] = "Signed Binary Proxy Execution: Odbcconf";
                techniques["T1218/009"] = "Signed Binary Proxy Execution: Regsvcs/Regasm";
                techniques["T1218/010"] = "Signed Binary Proxy Execution: Regsvr32";
                techniques["T1218/011"] = "Signed Binary Proxy Execution: Rundll32";
                techniques["T1218/012"] = "Signed Binary Proxy Execution: Verclsid";
                techniques["T1216"] = "Signed Script Proxy Execution";
                techniques["T1216/001"] = "Signed Script Proxy Execution: PubPrn";
                techniques["T1553"] = "Subvert Trust Controls";
                techniques["T1553/001"] = "Subvert Trust Controls: Gatekeeper Bypass";
                techniques["T1553/002"] = "Subvert Trust Controls: Code Signing";
                techniques["T1553/003"] = "Subvert Trust Controls: SIP and Trust Provider Hijacking";
                techniques["T1553/004"] = "Subvert Trust Controls: Install Root Certificate";
                techniques["T1553/005"] = "Subvert Trust Controls: Mark-of-the-Web Bypass";
                techniques["T1553/006"] = "Subvert Trust Controls: Code Signing Policy Modificaiton";
                techniques["T1221"] = "Template Injection";
                techniques["T1205"] = "Traffic Signaling";
                techniques["T1205/001"] = "Traffic Signaling: Port Knocking";
                techniques["T1127"] = "Trusted Developer Utilities Proxy Execution";
                techniques["T1127/001"] = "Trusted Developer Utilities Proxy Execution: MSBuild";
                techniques["T1535"] = "Unused/Unsupported Cloud Regions";
                techniques["T1550"] = "Use Alternative Authentication Material";
                techniques["T1550/001"] = "Use Alternative Authentication Material: Application Access Token";
                techniques["T1550/002"] = "Use Alternative Authentication Material: Pass the Hash";
                techniques["T1550/003"] = "Use Alternative Authentication Material: Pass the Ticket";
                techniques["T1550/004"] = "Use Alternative Authentication Material: Web Session Cookie";
                techniques["T1078"] = "Valid Accounts";
                techniques["T1078/001"] = "Valid Accounts: Default Accounts";
                techniques["T1078/002"] = "Valid Accounts: Domain Accounts";
                techniques["T1078/003"] = "Valid Accounts: Local Accounts";
                techniques["T1078/004"] = "Valid Accounts: Cloud Accounts";
                techniques["T1497"] = "Virtualization/Sandbox Evasion";
                techniques["T1497/001"] = "Virtualization/Sandbox Evasion: System Checks";
                techniques["T1497/002"] = "Virtualization/Sandbox Evasion: User Activity Based Checks";
                techniques["T1497/003"] = "Virtualization/Sandbox Evasion: Time Based Evasion";
                techniques["T1600"] = "Weaken Encryption";
                techniques["T1600/001"] = "Weaken Encryption: Reduce Key Space";
                techniques["T1600/002"] = "Weaken Encryption: Disable Crypto Hardware";
                techniques["T1220"] = "XSL Script Processing";
                techniques["T1484"] = "Domain Policy Modification";
                techniques["T1484/001"] = "Domain Policy Modification: Group Policy Modification";
                techniques["T1484/002"] = "Domain Policy Modification: Domain Trust Modification";
                techniques["T1110"] = "Brute Force";
                techniques["T1110/001"] = "Brute Force: Password Guessing";
                techniques["T1110/002"] = "Brute Force: Password Cracking";
                techniques["T1110/003"] = "Brute Force: Password Spraying";
                techniques["T1110/004"] = "Brute Force: Credential Stuffing";
                techniques["T1555"] = "Credentials from Password Stores";
                techniques["T1555/001"] = "Credentials from Password Stores: Keychain";
                techniques["T1555/002"] = "Credentials from Password Stores: Securityd Memory";
                techniques["T1555/003"] = "Credentials from Password Stores: Credentials from Web Browsers";
                techniques["T1555/004"] = "Credentials from Password Stores: Windows Credential Manager";
                techniques["T1555/005"] = "Credentials from Password Stores: Password Managers";
                techniques["T1212"] = "Exploitation for credential access";
                techniques["T1187"] = "Forced authentication";
                techniques["T1056"] = "Input Capture";
                techniques["T1056/001"] = "Input Capture: Keylogging";
                techniques["T1056/002"] = "Input Capture: GUI Input Capture";
                techniques["T1056/003"] = "Input Capture: Web Portal Capture";
                techniques["T1056/004"] = "Input Capture: Credential API Hooking";
                techniques["T1557"] = "Man-in-the-Middle";
                techniques["T1557/001"] = "Man-in-the-Middle: LLMNR/NBT-NS poisoning and SMB relay";
                techniques["T1557/002"] = "Man-in-the-Middle: ARP Cache Poisoning";
                techniques["T1556"] = "Modify Authentication Process";
                techniques["T1556/001"] = "Modify Authentication Process: Domain Controller Authentication";
                techniques["T1556/002"] = "Modify Authentication Process: Password Filter DLL";
                techniques["T1556/003"] = "Modify Authentication Process: Pluggable Authentication Modules";
                techniques["T1556/004"] = "Modify Authentication Process: Network Device Authentication";
                techniques["T1040"] = "Network Sniffing";
                techniques["T1003"] = "OS Credential Dumping";
                techniques["T1003/001"] = "OS Credential Dumping: LSASS Memory";
                techniques["T1003/002"] = "OS Credential Dumping: Security Account Manager";
                techniques["T1003/003"] = "OS Credential Dumping: NTDS";
                techniques["T1003/004"] = "OS Credential Dumping: LSA Secrets";
                techniques["T1003/005"] = "OS Credential Dumping: Cached Domain Credentials";
                techniques["T1003/006"] = "OS Credential Dumping: DCSync";
                techniques["T1003/007"] = "OS Credential Dumping: Proc Filesystem";
                techniques["T1003/008"] = "OS Credential Dumping: /etc/passwd and /etc/shadow";
                techniques["T1528"] = "Steal Application Access Token";
                techniques["T1558"] = "Steal or Forge Kerberos Tickets";
                techniques["T1558/001"] = "Steal or Forge Kerberos Tickets: Golden Ticket";
                techniques["T1558/002"] = "Steal or Forge Kerberos Tickets: Silver Ticket";
                techniques["T1558/003"] = "Steal or Forge Kerberos Tickets: Kerberoasting";
                techniques["T1558/004"] = "Steal or Forge Kerberos Tickets: AS-REP Roasting";
                techniques["T1539"] = "Steal Web Session Cookie";
                techniques["T1111"] = "Two-Factor Authentication Interception";
                techniques["T1552"] = "Unsecured Credentials";
                techniques["T1552/001"] = "Unsecured Credentials: Credentials in Files";
                techniques["T1552/002"] = "Unsecured Credentials: Credentials in Registry";
                techniques["T1552/003"] = "Unsecured Credentials: Bash History";
                techniques["T1552/004"] = "Unsecured Credentials: Private Keys";
                techniques["T1552/005"] = "Unsecured Credentials: Cloud Instance Metadata API";
                techniques["T1552/006"] = "Unsecured Credentials: Group Policy Preferences";
                techniques["T1552/007"] = "Unsecured Credentials: Container API";
                techniques["T1606"] = "Forge Web Credentials";
                techniques["T1606/001"] = "Forge Web Credentials: Web Cookies";
                techniques["T1606/002"] = "Forge Web Credentials: SAML Tokens";
                techniques["T1087"] = "Account Discovery";
                techniques["T1087/001"] = "Account Discovery: Local Account";
                techniques["T1087/002"] = "Account Discovery: Domain Account";
                techniques["T1087/003"] = "Account Discovery: Email Account";
                techniques["T1087/004"] = "Account Discovery: Cloud Account";
                techniques["T1010"] = "Application Window Discovery";
                techniques["T1217"] = "Browser Bookmark Discovery";
                techniques["T1580"] = "Cloud Infrastructure Discovery";
                techniques["T1538"] = "Cloud Service Dashboard";
                techniques["T1526"] = "Cloud Service Discovery";
                techniques["T1613"] = "Container and Resource Discovery";
                techniques["T1482"] = "Domain Trust Discovery";
                techniques["T1083"] = "File and Directory Discovery";
                techniques["T1046"] = "Network Service Scanning";
                techniques["T1135"] = "Network Share Discovery";
                techniques["T1040"] = "Network Sniffing";
                techniques["T1201"] = "Password Policy Discovery";
                techniques["T1120"] = "Peripheral Device Discovery";
                techniques["T1069"] = "Permission Groups Discovery";
                techniques["T1069/001"] = "Permission Groups Discovery: Local Groups";
                techniques["T1069/002"] = "Permission Groups Discovery: Domain Groups";
                techniques["T1069/003"] = "Permission Groups Discovery: Cloud Groups";
                techniques["T1057"] = "Process Discovery";
                techniques["T1012"] = "Query Registry";
                techniques["T1018"] = "Remote System Discovery";
                techniques["T1518"] = "Software Discovery";
                techniques["T1518/001"] = "Software Discovery: Security Software Discovery";
                techniques["T1082"] = "System Information Discovery";
                techniques["T1614"] = "System Location Discovery";
                techniques["T1016"] = "System Network Configuration Discovery";
                techniques["T1016/001"] = "System Network Configuration Discovery: Internet Connection Discovery";
                techniques["T1049"] = "System Network Connections Discovery";
                techniques["T1033"] = "System Owner/User Discovery";
                techniques["T1007"] = "System Service Discovery";
                techniques["T1124"] = "System Time Discovery";
                techniques["T1497"] = "Virtualization/Sandbox Evasion";
                techniques["T1497/001"] = "Virtualization/Sandbox Evasion: System Checks";
                techniques["T1497/002"] = "Virtualization/Sandbox Evasion: User Activity Based Checks";
                techniques["T1497/003"] = "Virtualization/Sandbox Evasion: Time Based Evasion";
                techniques["T1210"] = "Exploitation of Remote Services";
                techniques["T1534"] = "Internal Spearphishing";
                techniques["T1570"] = "Lateral Tool Transfer";
                techniques["T1563"] = "Remote Service Session Hijacking";
                techniques["T1563/001"] = "Remote Service Session Hijacking: SSH Hijacking";
                techniques["T1563/002"] = "Remote Service Session Hijacking: RDP Hijacking";
                techniques["T1021"] = "Remote Services";
                techniques["T1021/001"] = "Remote Services: Remote Desktop Protocol";
                techniques["T1021/002"] = "Remote Services: SMB/Windows Admin Shares";
                techniques["T1021/003"] = "Remote Services: Distributed Component Object Model";
                techniques["T1021/004"] = "Remote Services: SSH";
                techniques["T1021/005"] = "Remote Services: VNC";
                techniques["T1021/006"] = "Remote Services: Windows Remote Management";
                techniques["T1091"] = "Replication through Removable Media";
                techniques["T1072"] = "Software Deployment Tools";
                techniques["T1080"] = "Taint Shared Content";
                techniques["T1550"] = "Use Alternate Authentication Material";
                techniques["T1550/001"] = "Use Alternate Authentication Material: Application Access Token";
                techniques["T1550/002"] = "Use Alternate Authentication Material: Pass the Hash";
                techniques["T1550/003"] = "Use Alternate Authentication Material: Pass the Ticket";
                techniques["T1550/004"] = "Use Alternate Authentication Material: Web Session Cookie";
                techniques["T1560"] = "Archive Collected Data";
                techniques["T1560/001"] = "Archive Collected Data: Archive via Utility";
                techniques["T1560/002"] = "Archive Collected Data: Archive via Library";
                techniques["T1560/003"] = "Archive Collected Data: Archive via Custom Method";
                techniques["T1123"] = "Audio Capture";
                techniques["T1119"] = "Automated Collection";
                techniques["T1115"] = "Clipboard Data";
                techniques["T1530"] = "Data from Cloud Storage Object";
                techniques["T1602"] = "Data from Configuration Repository";
                techniques["T1602/001"] = "Data from Configuration Repository: SNMP (MIB Dump)";
                techniques["T1602/002"] = "Data from Configuration Repository: Network Device Configuration Dump";
                techniques["T1213"] = "Data from Information Repositories";
                techniques["T1213/001"] = "Data from Information Repositories: Confluence";
                techniques["T1213/002"] = "Data from Information Repositories: Sharepoint";
                techniques["T1005"] = "Data from Local System";
                techniques["T1039"] = "Data from Network Shared Drive";
                techniques["T1025"] = "Data from Removable Media";
                techniques["T1074"] = "Data Staged";
                techniques["T1074/001"] = "Data Staged: Local Data Staging";
                techniques["T1074/002"] = "Data Staged: Remote Data Staging";
                techniques["T1114"] = "Email Collection";
                techniques["T1114/001"] = "Email Collection: Local Email Collection";
                techniques["T1114/002"] = "Email Collection: Remote Email Collection";
                techniques["T1114/003"] = "Email Collection: Email Forwarding Rule";
                techniques["T1056"] = "Input Capture";
                techniques["T1056/001"] = "Input Capture: Keylogging";
                techniques["T1056/002"] = "Input Capture: GUI input capture";
                techniques["T1056/003"] = "Input Capture: Web portal capture";
                techniques["T1056/004"] = "Input Capture: Credential API hooking";
                techniques["T1185"] = "Man in the Browser";
                techniques["T1557"] = "Man-in-the-Middle";
                techniques["T1557/001"] = "Man-in-the-Middle: LLMNR/NBT-NS Poisoning and SMB Relay";
                techniques["T1557/002"] = "Man-in-the-Middle: ARP Cache Poisoning";
                techniques["T1113"] = "Screen Capture";
                techniques["T1125"] = "Video Capture";
                techniques["T1071"] = "Application layer protocol";
                techniques["T1071/001"] = "Application layer protocol: Web protocols";
                techniques["T1071/002"] = "Application layer protocol: File transfer protocols";
                techniques["T1071/003"] = "Application layer protocol: Mail protocols";
                techniques["T1071/004"] = "Application layer protocol: DNS";
                techniques["T1092"] = "Communication through removable media";
                techniques["T1132"] = "Data encoding";
                techniques["T1132/001"] = "Data encoding: Standard encoding";
                techniques["T1132/002"] = "Data encoding: Non-standard encoding";
                techniques["T1001"] = "Data obfuscation";
                techniques["T1001/001"] = "Data obfuscation: Junk data";
                techniques["T1001/002"] = "Data obfuscation: Steganography";
                techniques["T1001/003"] = "Data obfuscation: Protocol impersonation";
                techniques["T1568"] = "Dynamic resolution";
                techniques["T1568/001"] = "Dynamic resolution: Fast Flux DNS";
                techniques["T1568/002"] = "Dynamic resolution: Domain Generation Algorithms";
                techniques["T1568/003"] = "Dynamic resolution: DNS Calculation";
                techniques["T1573"] = "Encrypted Channel";
                techniques["T1573/001"] = "Encrypted Channel: Symmetric Cryptography";
                techniques["T1573/002"] = "Encrypted Channel: Asymmetric cryptography";
                techniques["T1008"] = "Fallback channels";
                techniques["T1105"] = "Ingress tool transfer";
                techniques["T1104"] = "Multi-stage channels";
                techniques["T1095"] = "Non-application layer protocol";
                techniques["T1571"] = "Non-standard port";
                techniques["T1572"] = "Protocol tunneling";
                techniques["T1090"] = "Proxy";
                techniques["T1090/001"] = "Proxy: Internal proxy";
                techniques["T1090/002"] = "Proxy: External proxy";
                techniques["T1090/003"] = "Proxy: Multi-hop proxy";
                techniques["T1090/004"] = "Proxy: Domain fronting";
                techniques["T1219"] = "Remote Access Software";
                techniques["T1205"] = "Traffic Signaling";
                techniques["T1205/001"] = "Traffic Signaling: Port Knocking";
                techniques["T1102"] = "Web service";
                techniques["T1102/001"] = "Web service: Dead drop resolver";
                techniques["T1102/002"] = "Web service: Bidirectional communication";
                techniques["T1102/003"] = "Web service: One-way communication";
                techniques["T1020"] = "Automated Exfiltration";
                techniques["T1020/001"] = "Automated exfiltration: Traffic Duplication";
                techniques["T1030"] = "Data trasnfer size limits";
                techniques["T1048"] = "Exfiltration over alternative protocol";
                techniques["T1048/001"] = "Exfiltration over alternative protocol: Exfiltration over symmetric encrypted non-C2 protocol";
                techniques["T1048/002"] = "Exfiltration over alternative protocol: Exfiltration over asymmetric encrypted non-C2 protocol";
                techniques["T1048/003"] = "Exfiltration over alternative protocol: Exfiltration over unencrypted/obfuscated non-C2 protocol";
                techniques["T1041"] = "Exfiltration over C2 channel";
                techniques["T1011"] = "Exfiltration over other network medium";
                techniques["T1011/001"] = "Exfiltration over other network medium: Exfiltration over bluetooth";
                techniques["T1052"] = "Exfiltration over physical medium";
                techniques["T1052/001"] = "Exfiltration over physical medium: Exfiltration over USB";
                techniques["T1567"] = "Exfiltration over web service";
                techniques["T1567/001"] = "Exfiltration over web service: Exfiltration to code repository";
                techniques["T1567/002"] = "Exfiltration over web service: Exfiltration to cloud storage";
                techniques["T1029"] = "Scheduled transfer";
                techniques["T1537"] = "Transfer Data to Cloud Account";
                techniques["T1531"] = "Account Access Removal";
                techniques["T1485"] = "Data Destruction";
                techniques["T1486"] = "Data Encrypted for Impact";
                techniques["T1565"] = "Data Manipulation";
                techniques["T1565/001"] = "Data Manipulation: Stored Data Manipulation";
                techniques["T1565/002"] = "Data Manipulation: Transmitted Data Manipulation";
                techniques["T1565/003"] = "Data Manipulation: Runtime Data Manipulation";
                techniques["T1491"] = "Defacement";
                techniques["T1491/001"] = "Defacement: Internal Defacement";
                techniques["T1491/002"] = "Defacement: External Defacement";
                techniques["T1561"] = "Disk Wipe";
                techniques["T1561/001"] = "Disk Wipe: Disk Content Wipe";
                techniques["T1561/002"] = "Disk Wipe: Disk Structure Wipe";
                techniques["T1499"] = "Endpoint Denial of Service";
                techniques["T1499/001"] = "Endpoint Denial of Service: OS Exhaustion Flood";
                techniques["T1499/002"] = "Endpoint Denial of Service: Service Exhaustion Flood";
                techniques["T1499/003"] = "Endpoint Denial of Service: Application Exhaustion Flood";
                techniques["T1499/004"] = "Endpoint Denial of Service: Application or System Exploitation";
                techniques["T1495"] = "Firmware Corruption";
                techniques["T1490"] = "Inhibit System Recovery";
                techniques["T1498"] = "Network Denial of Service";
                techniques["T1498/001"] = "Network Denial of Service: Direct Network Flood";
                techniques["T1498/002"] = "Network Denial of Service: Reflection Amplification";
                techniques["T1496"] = "Resource Hijacking";
                techniques["T1489"] = "Service Stop";
                techniques["T1529"] = "System Shutdown/Reboot";
                techniques["T1475"] = "Deliver malicious app via authorized app store";
                techniques["T1476"] = "Deliver malicious app via other means";
                techniques["T1456"] = "Drive-by compromise";
                techniques["T1458"] = "Exploit via charging station or PC";
                techniques["T1477"] = "Exploit via radio interfaces";
                techniques["T1478"] = "Install insecure of malicious configuration";
                techniques["T1461"] = "Lockscreen bypass";
                techniques["T1444"] = "Masquerade as legitimate application";
                techniques["T1474"] = "Supply chain compromise";
                techniques["T1402"] = "Broadcast receivers";
                techniques["T1575"] = "Native code";
                techniques["T1605"] = "Command-Line Interface";
                techniques["T1603"] = "Scheduled Task/Job";
                techniques["T1401"] = "Abuse device administrator access to prevent removal";
                techniques["T1402"] = "Broadcast receivers";
                techniques["T1540"] = "Code injection";
                techniques["T1577"] = "Compromise application executable";
                techniques["T1541"] = "Foreground persistence";
                techniques["T1403"] = "Modify cached executable code";
                techniques["T1398"] = "Modify OS kernal or boot partition";
                techniques["T1400"] = "Modify system partition";
                techniques["T1399"] = "Modify trusted execution environment";
                techniques["T1603"] = "Scheduled Task/Job";
                techniques["T1540"] = "Code injection";
                techniques["T1401"] = "Device Administrator Permissions";
                techniques["T1404"] = "Exploit OS vulnerability";
                techniques["T1405"] = "Exploit TEE vulnerability";
                techniques["T1418"] = "Application discovery";
                techniques["T1540"] = "Code injection";
                techniques["T1447"] = "Delete Device Data";
                techniques["T1446"] = "Device lockout";
                techniques["T1408"] = "Disguise root/jailbreak indicators";
                techniques["T1407"] = "Download new code at runtime";
                techniques["T1523"] = "Evade analysis environment";
                techniques["T1581"] = "Geofencing";
                techniques["T1516"] = "Input injection";
                techniques["T1478"] = "Install insecure or malicious configuration";
                techniques["T1444"] = "Masquerade as legitimate application";
                techniques["T1398"] = "Modify OS kernel or boot partition";
                techniques["T1400"] = "Modify system partition";
                techniques["T1399"] = "Modify trusted execution environment";
                techniques["T1575"] = "Native code";
                techniques["T1406"] = "Obfuscated files or information";
                techniques["T1508"] = "Suppress application icon";
                techniques["T1576"] = "Uninstall malicious application";
                techniques["T1604"] = "Proxy Through Victim";
                techniques["T1517"] = "Access notifications";
                techniques["T1413"] = "Access sensitive data in device logs";
                techniques["T1409"] = "Access stored application data";
                techniques["T1414"] = "Capture clipboard data";
                techniques["T1412"] = "Capture SMS messages";
                techniques["T1405"] = "Exploit TEE vulnerability";
                techniques["T1417"] = "Input capture";
                techniques["T1411"] = "Input prompt";
                techniques["T1579"] = "Keychain";
                techniques["T1410"] = "Network traffic capture or redirection";
                techniques["T1416"] = "URI Hijacking";
                techniques["T1418"] = "Application discovery";
                techniques["T1523"] = "Evade analysis environment";
                techniques["T1420"] = "File and directory discovery";
                techniques["T1430"] = "Location tracking";
                techniques["T1423"] = "Network service scanning";
                techniques["T1424"] = "Process discovery";
                techniques["T1426"] = "System infromation discovery";
                techniques["T1422"] = "System netwrok configuration discovery";
                techniques["T1421"] = "System network connections discovery";
                techniques["T1427"] = "Attack PC via USB connection";
                techniques["T1428"] = "Exploit enterprise resources";
                techniques["T1435"] = "Access calendar entries";
                techniques["T1433"] = "Access call log";
                techniques["T1432"] = "Access contact list";
                techniques["T1517"] = "Access notifications";
                techniques["T1413"] = "Access sensitive data in device logs";
                techniques["T1409"] = "Access stored application data";
                techniques["T1429"] = "Capture audio";
                techniques["T1512"] = "Capture camera";
                techniques["T1414"] = "Capture clipboard data";
                techniques["T1412"] = "Capture SMS messages";
                techniques["T1533"] = "Data from local system";
                techniques["T1541"] = "Foreground persistence";
                techniques["T1417"] = "Input capture";
                techniques["T1430"] = "Location tracking";
                techniques["T1507"] = "Network information discovery";
                techniques["T1410"] = "Network traffic capture or redirection";
                techniques["T1513"] = "Screen capture";
                techniques["T1438"] = "Alternate network mediums";
                techniques["T1436"] = "Commonly used port";
                techniques["T1520"] = "Domain generation algorithms";
                techniques["T1544"] = "Remote file copy";
                techniques["T1437"] = "Standard application layer protocol";
                techniques["T1521"] = "Standard cryptographic protocol";
                techniques["T1509"] = "Uncommonly used port";
                techniques["T1481"] = "Web service";
                techniques["T1438"] = "Alternate network mediums";
                techniques["T1436"] = "Commonly used port";
                techniques["T1532"] = "Data encrypted";
                techniques["T1437"] = "Standard application layer protocol";
                techniques["T1448"] = "Carrier billing fraud";
                techniques["T1510"] = "Clipboard modification";
                techniques["T1471"] = "Data encrypted for impact";
                techniques["T1447"] = "Delete device data";
                techniques["T1446"] = "Device lockout";
                techniques["T1472"] = "Generate fraudulent advertising revenue";
                techniques["T1516"] = "Input injection";
                techniques["T1452"] = "Manipulate app store rankings or ratings";
                techniques["T1400"] = "Modify system partition";
                techniques["T1582"] = "SMS Control";
                techniques["T1466"] = "Downgrade to insecure protocols";
                techniques["T1439"] = "Eavesdrop on insecure network communication";
                techniques["T1449"] = "Exploit SS7 to redirect phone calls/SMS";
                techniques["T1450"] = "Exploit SS7 to track device location";
                techniques["T1464"] = "Jamming or denial of service";
                techniques["T1463"] = "Manipulate device communication";
                techniques["T1467"] = "Rogue cellular base station";
                techniques["T1465"] = "Rogue Wi-Fi access points";
                techniques["T1451"] = "SIM card swap";
                techniques["T1470"] = "Obtain device cloud backups";
                techniques["T1468"] = "Remotely track device without authorization";
                techniques["T1469"] = "Remotely wipe data without authorization";
                techniques["ics-T0802"] = "Automated Collection";
                techniques["ics-T0811"] = "Data from Information Repositories";
                techniques["ics-T0868"] = "Detect Operating Mode";
                techniques["ics-T0877"] = "I/O Image";
                techniques["ics-T0830"] = "Man in the Middle";
                techniques["ics-T0801"] = "Monitor Process State";
                techniques["ics-T0861"] = "Point & Tag Identification";
                techniques["ics-T0845"] = "Program Upload";
                techniques["ics-T0852"] = "Screen Capture";
                techniques["ics-T0887"] = "Wireless Sniffing";
                techniques["ics-T0885"] = "Commonly Used Port";
                techniques["ics-T0884"] = "Connection Proxy";
                techniques["ics-T0869"] = "Standard Application Layer Protocol";
                techniques["ics-T0840"] = "Network Connection Enumeration";
                techniques["ics-T0842"] = "Network Sniffing";
                techniques["ics-T0846"] = "Remote System Discovery";
                techniques["ics-T0888"] = "Remote System Information Discovery";
                techniques["ics-T0887"] = "Wireless Sniffing";
                techniques["ics-T0858"] = "Change Operating Mode";
                techniques["ics-T0820"] = "Exploitation for Evasion";
                techniques["ics-T0872"] = "Indicator Removal on Host";
                techniques["ics-T0849"] = "Masquerading";
                techniques["ics-T0851"] = "Rootkit";
                techniques["ics-T0856"] = "Spoof Reporting Message";
                techniques["ics-T0858"] = "Change Operating Mode";
                techniques["ics-T0807"] = "Command-Line Interface";
                techniques["ics-T0871"] = "Execution through API";
                techniques["ics-T0823"] = "Graphical User Interface";
                techniques["ics-T0874"] = "Hooking";
                techniques["ics-T0821"] = "Modify Controller Tasking";
                techniques["ics-T0834"] = "Native API";
                techniques["ics-T0853"] = "Scripting";
                techniques["ics-T0863"] = "User Execution";
                techniques["ics-T0879"] = "Damage to Property";
                techniques["ics-T0813"] = "Denial of Control";
                techniques["ics-T0815"] = "Denial of View";
                techniques["ics-T0826"] = "Loss of Availability";
                techniques["ics-T0827"] = "Loss of Control";
                techniques["ics-T0828"] = "Loss of Productivity and Revenue";
                techniques["ics-T0837"] = "Loss of Protection";
                techniques["ics-T0880"] = "Loss of Safety";
                techniques["ics-T0829"] = "Loss of View";
                techniques["ics-T0831"] = "Manipulation of Control";
                techniques["ics-T0832"] = "Manipulation of View";
                techniques["ics-T0882"] = "Theft of Operational Information";
                techniques["ics-T0806"] = "Brute Force I/O";
                techniques["ics-T0836"] = "Modify Parameter";
                techniques["ics-T0839"] = "Module Firmware";
                techniques["ics-T0856"] = "Spoof Reporting Message";
                techniques["ics-T0855"] = "Unauthorized Command Message";
                techniques["ics-T0800"] = "Activate Firmware Update Mode";
                techniques["ics-T0878"] = "Alarm Suppression";
                techniques["ics-T0803"] = "Block Command Message";
                techniques["ics-T0804"] = "Block Reporting Message";
                techniques["ics-T0805"] = "Block Serial COM";
                techniques["ics-T0809"] = "Data Destruction";
                techniques["ics-T0814"] = "Denial of Service";
                techniques["ics-T0816"] = "Device Restart/Shutdown";
                techniques["ics-T0835"] = "Manipulate I/O Image";
                techniques["ics-T0838"] = "Modify Alarm Settings";
                techniques["ics-T0851"] = "Rootkit";
                techniques["ics-T0881"] = "Service Stop";
                techniques["ics-T0857"] = "System Firmware";
                techniques["ics-T0810"] = "Data Historian Compromise";
                techniques["ics-T0817"] = "Drive-by Compromise";
                techniques["ics-T0818"] = "Engineering Workstation Compromise";
                techniques["ics-T0819"] = "Exploit Public-Facing Application";
                techniques["ics-T0866"] = "Exploitation of Remote Services";
                techniques["ics-T0822"] = "External Remote Services";
                techniques["ics-T0883"] = "Internet Accessible Device";
                techniques["ics-T0886"] = "Remote Services";
                techniques["ics-T0847"] = "Replication Through Removable Media";
                techniques["ics-T0848"] = "Rogue Master";
                techniques["ics-T0865"] = "Spearphishing Attachment";
                techniques["ics-T0862"] = "Supply Chain Compromise";
                techniques["ics-T0860"] = "Wireless Compromise";
                techniques["ics-T0812"] = "Default Credentials";
                techniques["ics-T0866"] = "Exploitation of Remote Services";
                techniques["ics-T0867"] = "Lateral Tool Transfer";
                techniques["ics-T0843"] = "Program Download";
                techniques["ics-T0886"] = "Remote Services";
                techniques["ics-T0859"] = "Valid Accounts";
                techniques["ics-T0889"] = "Modify Program";
                techniques["ics-T0839"] = "Module Firmware";
                techniques["ics-T0873"] = "Project File Infection";
                techniques["ics-T0857"] = "System Firmware";
                techniques["ics-T0859"] = "Valid Accounts";
                techniques["ics-T0890"] = "Exploitation for Privilege Escalation";
                techniques["ics-T0874"] = "Hooking";
                ViewBag.techniques = techniques;
                #endregion

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
                if (tree.IsClustered) { document.Add("\tsubgraph cluster_" + category[0].Replace(" ", "").ToLower() + " {"); }
                else { document.Add("\tsubgraph " + category[0].Replace(" ", "").ToLower() + " {"); }
                

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
                document.Add("\tsubgraph " + category[0].Replace(" ","").ToLower() + " {");
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

            return View();
        }

#endregion

    }
}
