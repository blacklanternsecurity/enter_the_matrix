/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     08-23-2022
# Copyright:   (c) BLS OPS LLC. 2022
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using Enter_The_Matrix.Models;

namespace Enter_The_Matrix.Services
{
    public class MetricsService
    {
        private readonly StepsService _stepsService;
        private readonly ScenariosService _scenariosService;

        public MetricsService(IETMDatabaseSettings settings)
        {
            _stepsService = new StepsService(settings);
            _scenariosService = new ScenariosService(settings);
        }

        public async Task<Dictionary<string, (int, List<string>)>> GetMitreIdsUsed()
        {
            List<Steps> allSteps = await _stepsService.GetAllAsync();
            Dictionary<string, (int, List<string>)> MitreIdsUsed = new Dictionary<string, (int, List<string>)>();
            foreach (Steps step in allSteps)
            {
                if (step.MitreId != null)
                {
                    if (!MitreIdsUsed.ContainsKey(step.MitreId))
                    {
                        MitreIdsUsed[step.MitreId] = (1, new List<string>(new string[] { step.Event }));
                    }
                    else
                    {
                        int newCount = MitreIdsUsed[step.MitreId].Item1 + 1;
                        List<string> newList = MitreIdsUsed[step.MitreId].Item2;
                        newList.Add(step.Event);
                        MitreIdsUsed[step.MitreId] = (newCount, newList);
                    }
                }
            }
            return MitreIdsUsed;
        }

        public async Task<Dictionary<string, List<(string, string)>>> GetAttackChains()
        {
            List<Scenarios> allScenarios = await _scenariosService.GetAllAsync();
            Dictionary<string, List<(string, string)>> attackChains = new Dictionary<string, List<(string, string)>>();
            foreach (Scenarios scenario in allScenarios)
            {
                List<Steps> stepsInScenario = new List<Steps>();
                foreach (string eventId in scenario.Steps)
                {
                    stepsInScenario.Add(await _stepsService.GetByIdAsync(eventId));
                }
                List<(string, string)> stepDetails = new List<(string, string)>();
                foreach (Steps step in stepsInScenario)
                {
                    stepDetails.Add((step.MitreId, step.Event));
                }
                attackChains[scenario.Id] = stepDetails;
            }

            return attackChains;
        }

        public async Task<Dictionary<string, (int, List<string>)>> GetCommonEvents(int position)
        {
            List<Scenarios> allScenarios = await _scenariosService.GetAllAsync();
            Dictionary<string, (int, List<string>)> commonEvents = new Dictionary<string, (int, List<string>)>();
            List<Steps> startingSteps = new List<Steps>();
            foreach (Scenarios scenario in allScenarios)
            {
                int actualPosition = 0;
                if (position == -1) { actualPosition = scenario.Steps.Length - 1; }
                startingSteps.Add(await _stepsService.GetByIdAsync(scenario.Steps[actualPosition]));
            }
            foreach (Steps step in startingSteps)
            {
                if (step.MitreId != null)
                {
                    if (!commonEvents.ContainsKey(step.MitreId))
                    {
                        commonEvents[step.MitreId] = (1, new List<string>(new string[] { step.Event }));
                    }
                    else
                    {
                        int newCount = commonEvents[step.MitreId].Item1 + 1;
                        List<string> newList = commonEvents[step.MitreId].Item2;
                        newList.Add(step.Event);
                        commonEvents[step.MitreId] = (newCount, newList);
                    }
                }
            }
            return commonEvents;
        }
    }
}
