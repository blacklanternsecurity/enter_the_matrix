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
    public class Initiation
    {
        public Dictionary<string, string> nistDescriptions { get; set; }
        public Dictionary<string, string> blsDescriptions { get; set; }
        public HtmlString methodology { get; set; }


        public Initiation()
        {
            methodology = new HtmlString("This decides how likely your threat source is to execute your scenario. It is important to understand that any threat source can be placed in a scenario. The likelihood of them attempting that scenario should change based on the threat source and the scenario. This will affect the overall risk of what you are constructing. Some examples: <ul><li>A low C:I:T individual is unlikely to ninja their way into a government facility to thermite critical infrastructure.</li><li>A high C:I:T nation state is unlikely to make itself known just to cause a small network outage at a random corporation.</li><li>A high C:I:T state sponsored organization is very likely to execute a carefully thought out attack to exfiltrate intellectual property.</li><li>A low C:I:T organized group is very likely to deface political organizations based on ideology.</li></ul>");

            nistDescriptions = new Dictionary<string, string>();
            nistDescriptions.Add("Very High", "Adversary is almost certain to initiate the threat event.");
            nistDescriptions.Add("High", "Adversary is highly likely to initiate the threat event.");
            nistDescriptions.Add("Moderate", "Adversary is somewhat likely to initiate the threat event.");
            nistDescriptions.Add("Low", "Adversary is unlikely to initiate the threat event.");
            nistDescriptions.Add("Very Low", "Adversary is highly unlikely to initiate the threat event");

            blsDescriptions = new Dictionary<string, string>();
            blsDescriptions.Add("Very High", "An adversary almost certainly is motivated to perform an attack and currently has the resources (financial, technical, etc) to do so");
            blsDescriptions.Add("High", "An adversary is likely to perform the attack and likely has either motivation or resources (financial, technical, etc) to do so");
            blsDescriptions.Add("Moderate", "An adversary may perform an attack, but is not directly motivated to do so (e.g. they may be an opportunistic attacker) or currently lacks sufficient resources to do so");
            blsDescriptions.Add("Low", "An adversary will likely not attack due to a lack of capability, resources or motivation");
            blsDescriptions.Add("Very Low", "An adversary is likely unable to initiate an attack due to a lack of capability, resources, or motivation");
        }
    }
}
