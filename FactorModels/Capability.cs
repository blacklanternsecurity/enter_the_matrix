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
    public class Capability
    {
        public Dictionary<string, string> nistDescriptions { get; set; }
        public Dictionary<string, string> blsDescriptions { get; set; }
        public HtmlString methodology { get; set; }
        public Capability()
        {
            methodology = new HtmlString("What are your threat source's capabilities?<ul><li>Capabilities consist of:<ul><li>Financing (are they financed)</li><li>Knowledge (what is their background/training)</li><li>Infrastructure (physical assets - cloud, server farms, computing power)</li><li>Time (number of able bodies and their time allotted)</li></ul></li><li>For example:<ul><li>Individuals [0-4]</li><li>Groups [5-7]</li><li>Nation State/State Sponsored [8-10]</li></ul></li></ul>");

            nistDescriptions = new Dictionary<string, string>();
            nistDescriptions.Add("Very High", @"The adversary has a very sophisticated level of expertise, is well-resourced, and can generate
                opportunities to support multiple successful, continuous, and coordinated attacks");
            nistDescriptions.Add("High", @"The adversary has a sophisticated level of expertise, with significant resources and opportunities
                to support multiple successful coordinated attacks.");
            nistDescriptions.Add("Moderate", @"The adversary has moderate resources, expertise, and opportunities to support multiple successful
                attacks.");
            nistDescriptions.Add("Low", @"The adversary has limited resources, expertise, and opportunities to support a successful attack.");
            nistDescriptions.Add("Very Low", @"The adversary has very limited resources, expertise, and opportunities to support a successful
                attack.");

            blsDescriptions = new Dictionary<string, string>();
            blsDescriptions.Add("Very High", "The adversary has extensive resources and motivation (state-sponsored), as well as expertise required to perform an attack or maintain an extended campaign against a target");
            blsDescriptions.Add("High", "The adversary has available resources and motivation to perform the attack, and likely has the expertise required to perform the attack");
            blsDescriptions.Add("Moderate", "The adversary has the resources and capability to perform an attack, but may not be able to sustain an extended campaign or lacks the ability to do so in a sophisticated manner");
            blsDescriptions.Add("Low", "The adversary has few resources or is lacking a critical resource, capability or opportunity (lack of funding, technical ability or access)");
            blsDescriptions.Add("Very Low", "The adversary has little to no available resources, capability or opportunity to perform an attack");
        }
    }
}
