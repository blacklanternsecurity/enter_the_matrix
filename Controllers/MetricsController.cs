/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     08-23-2022
# Copyright:   (c) BLS OPS LLC. 2022
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Enter_The_Matrix.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace Enter_The_Matrix.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MetricsController : ControllerBase
    {
        private readonly MetricsService _metricsService;

        public MetricsController(MetricsService service)
        {
            _metricsService = service;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetMitreIdsUsed()
        {
            Dictionary<string, (int, List<string>)> used = await _metricsService.GetMitreIdsUsed();
            Dictionary<string, int> mitreCount = new Dictionary<string, int>();
            foreach (var entry in used)
            {
                mitreCount[entry.Key] = entry.Value.Item1;
            }
            var sortedUsed = from entry in used
                             orderby entry.Value.Item1 descending
                             select new
                             {
                                 MitreId = entry.Key,
                                 Count = entry.Value.Item1,
                                 Descriptions = entry.Value.Item2
                             };

            return Content(JsonConvert.SerializeObject(sortedUsed), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> GetAttackChains()
        {
            Dictionary<string, List<(string, string)>> chains = await _metricsService.GetAttackChains();
            var returnChains = from entry in chains
                               select new
                               {
                                   ScenarioId = entry.Key,
                                   Events = (from sub in entry.Value
                                             select new
                                             {
                                                 MitreId = sub.Item1,
                                                 Description = sub.Item2
                                             })
                               };
            return Content(JsonConvert.SerializeObject(returnChains), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> GetCommonStarts()
        {
            Dictionary<string, (int, List<string>)> events = await _metricsService.GetCommonEvents(0);
            var returnEvents = from entry in events
                               orderby entry.Value.Item1 descending
                               select new
                               {
                                   MitreId = entry.Key,
                                   Count = entry.Value.Item1,
                                   Descriptions = entry.Value.Item2
                               };
            return Content(JsonConvert.SerializeObject(returnEvents), "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> GetCommonEnds()
        {
            Dictionary<string, (int, List<string>)> events = await _metricsService.GetCommonEvents(-1);
            var returnEvents = from entry in events
                               orderby entry.Value.Item1 descending
                               select new
                               {
                                   MitreId = entry.Key,
                                   Count = entry.Value.Item1,
                                   Descriptions = entry.Value.Item2
                               };
            return Content(JsonConvert.SerializeObject(returnEvents), "application/json");
        }
    }
}