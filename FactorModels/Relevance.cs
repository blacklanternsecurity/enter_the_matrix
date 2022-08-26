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
    public class Relevance
    {
        public Dictionary<string, string> nistDescriptions { get; set; }
        public Dictionary<string, string> blsDescriptions { get; set; }
        public HtmlString methodology { get; set; }
        public Relevance()
        {
            methodology = new HtmlString("Apply the value that most fits the descriptions provided below.");

            nistDescriptions = new Dictionary<string, string>();
            nistDescriptions.Add("Confirmed", "The threat event or TTP has been seen by the organization.");
            nistDescriptions.Add("Expected", "The threat event or TTP has been seen by the organization’s peers or partners.");
            nistDescriptions.Add("Anticipated", "The threat event or TTP has been reported by a trusted source.");
            nistDescriptions.Add("Predicted", "The threat event or TTP has been predicted by a trusted source.");
            nistDescriptions.Add("Possible", "The threat event or TTP has been described by a somewhat credible source.");
            nistDescriptions.Add("N/A", @"The threat event or TTP is not currently applicable. For example, a threat event or TTP could assume specific technologies,
                architectures, or processes that are not present in the organization, mission / business process, EA segment, or information
                system; or predisposing conditions that are not present(e.g., location in a flood plain).Alternately, if the organization is using
                detailed or specific threat information, a threat event or TTP could be deemed inapplicable because information indicates that
                no adversary is expected to initiate the threat event or use the TTP.");

            blsDescriptions = new Dictionary<string, string>();
            blsDescriptions.Add("Confirmed", "The vulnerability has been technically validated (successfully exploited) and shows likelihood of impact and/or result");
            blsDescriptions.Add("Expected", "The vulnerability has been indirectly validated (confirm a patch, version information, etc) but has not been directly exploited or triggered");
            blsDescriptions.Add("Anticipated", "A system exhibits signs of a finding; however technical validation is not possible or not enough information is available to confirm");
            blsDescriptions.Add("Predicted", "A finding is inferred from other relevant information but is not directly confirmed (the organization is demonstrating a consistent lack of patch management and is using severely out of date software could indicate the presence of vulnerabilities without direct verification)");
            blsDescriptions.Add("Possible", "A mitigation for a particular finding was not detected, however no other indicators of vulnerability were observed");
            blsDescriptions.Add("N/A", "");
        }

        public string getAbbreviation(string factor)
        {
            if (factor == "Confirmed") { return "C"; }
            else if (factor == "Expected") { return "E"; }
            else if (factor == "Anticipated") { return "A"; }
            else if (factor == "Predicted") { return "Pr"; }
            else if (factor == "Possible") { return "Po"; }
            else { return ""; }
        }
    }
}
