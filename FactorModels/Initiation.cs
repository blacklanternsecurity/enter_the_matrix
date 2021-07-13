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
    public class Initiation
    {
        public Dictionary<string, string> nistDescriptions { get; set; }
        public Dictionary<string, string> blsDescriptions { get; set; }

        public Initiation()
        {
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
