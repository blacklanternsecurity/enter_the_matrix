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
    public class Impact
    {
        public Dictionary<string, string> nistDescriptions { get; set; }
        public Dictionary<string, string> blsDescriptions { get; set; }
        public HtmlString methodology { get; set; }

        public Impact()
        {
            methodology = new HtmlString("This should be determined based on the event outcome. You should <b>not</b> take into account any of the previous factors that you used when determining adverse impact likelihood. This is soley describing the potential impact of the event occuring. For example: <ul><li>Event Description: Attacker deploys ransomware to entire corporate domain</li><li>Level of Impact: High (unlikely to destroy a business or kill someone, but still pretty bad)</li></ul>");

            nistDescriptions = new Dictionary<string, string>();
            nistDescriptions.Add("Very High", @"The threat event could be expected to have multiple severe or catastrophic adverse effects on
                organizational operations, organizational assets, individuals, other organizations, or the Nation.");
            nistDescriptions.Add("High", @"The threat event could be expected to have a severe or catastrophic adverse effect on
                organizational operations, organizational assets, individuals, other organizations, or the Nation. A
                severe or catastrophic adverse effect means that, for example, the threat event might: (i) cause a
                severe degradation in or loss of mission capability to an extent and duration that the organization is
                not able to perform one or more of its primary functions; (ii) result in major damage to
                organizational assets; (iii) result in major financial loss; or (iv) result in severe or catastrophic harm
                to individuals involving loss of life or serious life-threatening injuries.");
            nistDescriptions.Add("Moderate", @"The threat event could be expected to have a serious adverse effect on organizational operations,
                organizational assets, individuals other organizations, or the Nation. A serious adverse effect
                means that, for example, the threat event might: (i) cause a significant degradation in mission
                capability to an extent and duration that the organization is able to perform its primary functions,
                but the effectiveness of the functions is significantly reduced; (ii) result in significant damage to
                organizational assets; (iii) result in significant financial loss; or (iv) result in significant harm to
                individuals that does not involve loss of life or serious life-threatening injuries.");
            nistDescriptions.Add("Low", @"The threat event could be expected to have a limited adverse effect on organizational operations,
                organizational assets, individuals other organizations, or the Nation. A limited adverse effect
                means that, for example, the threat event might: (i) cause a degradation in mission capability to an
                extent and duration that the organization is able to perform its primary functions, but the
                effectiveness of the functions is noticeably reduced; (ii) result in minor damage to organizational
                assets; (iii) result in minor financial loss; or (iv) result in minor harm to individuals.");
            nistDescriptions.Add("Very Low", @"The threat event could be expected to have a negligible adverse effect on organizational
                operations, organizational assets, individuals other organizations, or the Nation.");

            blsDescriptions = new Dictionary<string, string>();
            blsDescriptions.Add("Very High", "The impact is extremely high to the target, and could have catastrophic effect (loss of life, business failure, etc.)");
            blsDescriptions.Add("High", "The impact to the target is severe, and could cause substantial business or mission impact (failure of a mission, severe impact, major damage to reputation, etc.)");
            blsDescriptions.Add("Moderate", "The impact is fairly major, such as degrading business function, impacting revenue, minor damage to reputation, etc.");
            blsDescriptions.Add("Low", "The impact to the organization is fairly minor and could be recovered from quickly");
            blsDescriptions.Add("Very Low", "The impact is of little to no concern to the organization, or no impact is possible");
        }
    }
}
