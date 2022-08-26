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
    public class Pervasiveness
    {
        public Dictionary<string, string> nistDescriptions { get; set; }
        public Dictionary<string, string> blsDescriptions { get; set; }
        public HtmlString methodology { get; set; }

        public Pervasiveness()
        {
            methodology = new HtmlString("This is only used if there is a predisposing condition. This should mirror the DREAD score or use best judgement to how widespread the issue is. For example: <ul><li>Pervasiveness of Predisposing Conditions: This could be a company wide policy enforcement or training issue (High [8-9])</li></ul>");

            nistDescriptions = new Dictionary<string, string>();
            nistDescriptions.Add("Very High", @"Applies to all organizational missions/business functions (Tier 1), mission/business processes
                (Tier 2), or information systems (Tier 3).");
            nistDescriptions.Add("High", @"Applies to most organizational missions/business functions (Tier 1), mission/business processes
                (Tier 2), or information systems (Tier 3).");
            nistDescriptions.Add("Moderate", @"Applies to many organizational missions/business functions (Tier 1), mission/business processes
                (Tier 2), or information systems (Tier 3).");
            nistDescriptions.Add("Low", @"Applies to some organizational missions/business functions (Tier 1), mission/business processes
                (Tier 2), or information systems (Tier 3).");
            nistDescriptions.Add("Very Low", @"Applies to few organizational missions/business functions (Tier 1), mission/business processes
                (Tier 2), or information systems (Tier 3).");

            blsDescriptions = new Dictionary<string, string>();
            blsDescriptions.Add("Very High", "Completely affects the entire organization or business components");
            blsDescriptions.Add("High", "Affects the majority of the organization or business components");
            blsDescriptions.Add("Moderate", "Affects a significant portion of business components or resources");
            blsDescriptions.Add("Low", "Affects a small number of resources or business components");
            blsDescriptions.Add("Very Low", "Affects very few or no resources or business components");
        }
    }
}
