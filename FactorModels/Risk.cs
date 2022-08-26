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
    public class Risk
    {
        public string[,] matrix { get; set; }
        public Dictionary<string, string> nistDescriptions { get; set; }
        public HtmlString methodology { get; set; }
        public Risk()
        {
            methodology = new HtmlString("This is determined for you. It is calculated based on the Overall Likelihood and Level of Impact. It is important to understand that just because something has a high Level of Impact, it does not mean that the Risk is high. If there is a moderate chance of the attacker executing the scenario, but there are many proactive controls in place, even a high impact event is considered low risk. For example: <ul><li>Likelihood of Attack Initiation: 5</li><li>Likelihood of Adverse Impact: 2</li><li>Overall Likelihood (Calculated): Low</li><li>Level of Impact: 9</li><li>Risk (Calculated): Low</li></ul>");

            // Handles calculating the over risk factor, taking into account the level of impact
            // as well as the overall likelihood of attack and adverse impacts

            matrix = new string[11, 11];

            // matrix[col, row]
            // col = Level of Impact
            // row = Likelihood

            // Very Low Impact
            for (int i = 0; i < 11; i++)
            { //  + Any Likelihood
                matrix[0, i] = matrix[1, i] = "Very Low";
            }

            // Low Impact 
            for (int i = 2; i < 11; i++)
            { // + (Low->Very High) Likelihood
                matrix[2, i] = matrix[3, i] = matrix[4, i] = "Low";
            }
            // Low Impact + Very Low Likelihood
            matrix[2, 0] = matrix[2, 1] = matrix[3, 0] = matrix[3, 1] = matrix[4, 0] = matrix[4, 1] = "Very Low";

            // Moderate Impact
            for (int i = 5; i < 11; i++)
            { // + Moderate->Very High Likelihood
                matrix[5, i] = matrix[6, i] = matrix[7, i] = "Moderate";
            }
            for (int i = 2; i < 5; i++)
            { // + Low Likelihood
                matrix[5, i] = matrix[6, i] = matrix[7, i] = "Low";
            }
            for (int i = 0; i < 2; i++)
            { // + Very Low Likelihood
                matrix[5, i] = matrix[6, i] = matrix[7, i] = "Very Low";
            }

            // High Impact
            for (int i = 8; i < 11; i++)
            { // + High/Very High Likelihood
                matrix[8, i] = matrix[9, i] = "High";
            }
            for (int i = 5; i < 8; i++)
            { // + Moderate Likelihood
                matrix[8, i] = matrix[9, i] = "Moderate";
            }
            for (int i = 0; i < 5; i++)
            { // + Low/Very Low Likelihood
                matrix[8, i] = matrix[9, i] = "Low";
            }

            // Very High Impact
            for (int i = 8; i < 11; i++)
            { // + High/Very High Likelihood
                matrix[10, i] = "Very High";
            }
            for (int i = 5; i < 8; i++)
            { // + Moderate Likelihood
                matrix[10, i] = "High";
            }
            for (int i = 2; i < 5; i++)
            { // + Low Likelihood
                matrix[10, i] = "Moderate";
            }
            for (int i = 0; i < 2; i++)
            { // + Very Low Likelihood
                matrix[10, i] = "Low";
            }

            // NIST 800-30 Descriptions
            nistDescriptions = new Dictionary<string, string>();
            nistDescriptions.Add("Very High", @"Very high risk means that a threat event could be expected to have multiple severe or
                catastrophic adverse effects on organizational operations, organizational assets, individuals,
                other organizations, or the Nation.");
            nistDescriptions.Add("High", @"High risk means that a threat event could be expected to have a severe or catastrophic adverse
                effect on organizational operations, organizational assets, individuals, other organizations, or the
                Nation.");
            nistDescriptions.Add("Moderate", @"Moderate risk means that a threat event could be expected to have a serious adverse effect on
                organizational operations, organizational assets, individuals, other organizations, or the Nation.");
            nistDescriptions.Add("Low", @"Low risk means that a threat event could be expected to have a limited adverse effect on
                organizational operations, organizational assets, individuals, other organizations, or the Nation.");
            nistDescriptions.Add("Very Low", @"Very low risk means that a threat event could be expected to have a negligible adverse effect on
                organizational operations, organizational assets, individuals, other organizations, or the Nation.");
        }
    }
}
