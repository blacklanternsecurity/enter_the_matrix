/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     06-07-2021
# Copyright:   (c) BLS OPS LLC. 2021
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enter_The_Matrix.FactorModels
{
    public class Intent
    {
        public Dictionary<string, string> nistDescriptions { get; set; }
        public Dictionary<string, string> blsDescriptions { get; set; }

        public Intent()
        {
            nistDescriptions = new Dictionary<string, string>();
            nistDescriptions.Add("Very High", @"The adversary seeks to undermine, severely impede, or destroy a core mission or business
                function, program, or enterprise by exploiting a presence in the organization’s information systems
                or infrastructure. The adversary is concerned about disclosure of tradecraft only to the extent that it
                would impede its ability to complete stated goals.");
            nistDescriptions.Add("High", @"The adversary seeks to undermine/impede critical aspects of a core mission or business function,
                program, or enterprise, or place itself in a position to do so in the future, by maintaining a presence
                in the organization’s information systems or infrastructure. The adversary is very concerned about
                minimizing attack detection/disclosure of tradecraft, particularly while preparing for future attacks.");
            nistDescriptions.Add("Moderate", @"The adversary seeks to obtain or modify specific critical or sensitive information or usurp/disrupt
                the organization’s cyber resources by establishing a foothold in the organization’s information
                systems or infrastructure. The adversary is concerned about minimizing attack detection/disclosure
                of tradecraft, particularly when carrying out attacks over long time periods. The adversary is willing
                to impede aspects of the organization’s missions/business functions to achieve these ends.");
            nistDescriptions.Add("Low", @"The adversary actively seeks to obtain critical or sensitive information or to usurp/disrupt the
                organization’s cyber resources, and does so without concern about attack detection/disclosure of
                tradecraft.");
            nistDescriptions.Add("Very Low", @"The adversary seeks to usurp, disrupt, or deface the organization’s cyber resources, and does so
                without concern about attack detection/disclosure of tradecraft.");

            blsDescriptions = new Dictionary<string, string>();
            blsDescriptions.Add("Very High", "The adversary is directly targeting an organization with specific intent to cause a specific effect to the organization (severe business impact, destruction of reputation, affecting lives or safety, financial gain, etc.) and is willing to expend significant resources to do so (time, money, and potentially expose trade-craft to achieve target objective)");
            blsDescriptions.Add("High", "The adversary directly targets an organization with the intent of causing an impact achieving some effect, and is willing to expend some resources to do so (they would be willing to expend time and money, but will not disclose trade-craft to do so)");
            blsDescriptions.Add("Moderate", "The adversary directly targets the organization with the objective to achieve business impact or cause an effect on the organization but is concerned with resources or trade-craft");
            blsDescriptions.Add("Low", "The adversary is looking to cause a substantial impact to the organization (business impact), or obtain sensitive information (PII, credit cards, etc.)");
            blsDescriptions.Add("Very Low", "The adversary looks to cause an impact to the organization (defacing a website, etc) or is an opportunistic attacker with no concern to impact");
        }
    }
}
