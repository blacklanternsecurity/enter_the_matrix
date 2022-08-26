/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Updated:     08-25-2022
# Copyright:   (c) BLS OPS LLC. 2022
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace Enter_The_Matrix.FactorModels
{
    public class Adverse
    {
        public Dictionary<string, string> nistDescriptions { get; set; }
        public Dictionary<string, string> blsDescriptions { get; set; }
        public HtmlString methodology { get; set; }

        public Adverse()
        {
            methodology = new HtmlString("You will need to take into account the previous 4 factors (Mitigation, Predisposing Conditions, Finding Reference, Event Relevance). Based on those items, how likely is the adverse impact of this event to happen, however severe it is? For example:<ul><li>Event Description: Attacker lands C2 shell via phish</li><li>Finding Reference: [None]</li><li>Predisposing Condition: [None]</li><li>Mitigation Observed: Advanced email filtering (emails did not get through), Advanced EDR (binaries were blocked from executing), Advanced GPO (macros disabled in environment)</li><li>Likelihood of Adverse Impact: Low (there is always a chance of bypassing all these protections, but it is not likely)</li></ul>");

            nistDescriptions = new Dictionary<string, string>();
            nistDescriptions.Add("Very High", "If the threat event is initiated or occurs, it is almost certain to have adverse impacts.");
            nistDescriptions.Add("High", "If the threat event is initiated or occurs, it is highly likely to have adverse impacts.");
            nistDescriptions.Add("Moderate", "If the threat event is initiated or occurs, it is somewhat likely to have adverse impacts.");
            nistDescriptions.Add("Low", "If the threat event is initiated or occurs, it is unlikely to have adverse impacts.");
            nistDescriptions.Add("Very Low", "If the threat event is initiated or occurs, it is highly unlikely to have adverse impacts.");

            blsDescriptions = new Dictionary<string, string>();
            blsDescriptions.Add("Very High", "The attack will definitely cause an adverse impact");
            blsDescriptions.Add("High", "The attack is very likely to cause an adverse impact but is not guaranteed");
            blsDescriptions.Add("Moderate", "It is possible that the attack could cause an adverse impact, but other factors largely influence this");
            blsDescriptions.Add("Low", "It is not likely that the attack could cause an adverse impact, but there is a small chance of affect");
            blsDescriptions.Add("Very Low", "There is almost no chance that the attack could cause an adverse impact");
        }
    }
}
