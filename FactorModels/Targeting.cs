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
    public class Targeting
    {
        public Dictionary<string, string> nistDescriptions { get; set; }
        public Dictionary<string, string> blsDescriptions { get; set; }
        public Targeting()
        {
            nistDescriptions = new Dictionary<string, string>();
            nistDescriptions.Add("Very High", @"The adversary analyzes information obtained via reconnaissance and attacks to target persistently
                a specific organization, enterprise, program, mission or business function, focusing on specific
                high-value or mission-critical information, resources, supply flows, or functions; specific employees
                or positions; supporting infrastructure providers/suppliers; or partnering organizations.");
            nistDescriptions.Add("High", @"The adversary analyzes information obtained via reconnaissance to target persistently a specific
                organization, enterprise, program, mission or business function, focusing on specific high-value or
                mission-critical information, resources, supply flows, or functions, specific employees supporting
                those functions, or key positions.");
            nistDescriptions.Add("Moderate", @"The adversary analyzes publicly available information to target persistently specific high-value
                organizations (and key positions, such as Chief Information Officer), programs, or information.");
            nistDescriptions.Add("Low", @"The adversary uses publicly available information to target a class of high-value organizations or
                information, and seeks targets of opportunity within that class.");
            nistDescriptions.Add("Very Low", @"The adversary may or may not target any specific organizations or classes of organizations.");

            blsDescriptions = new Dictionary<string, string>();
            blsDescriptions.Add("Very High", "The adversary will target a highly specific organization or other target in an attempt to cause a specific effect. They will perform extensive information gathering and research to identify the appropriate target or attack path to achieve the mission objective. (This attacker will likely target a specific person who has the access to the resources needed to achieve the objective, or will contain highly specific research to be able to perform the attack)");
            blsDescriptions.Add("High", "The adversary will target a specific organization or other resource with the intent of achieving a particular effect, they will likely perform information gathering and reconnaissance to identify the appropriate target or attack path");
            blsDescriptions.Add("Moderate", "The adversary will likely target a specific organization or resource from an organization to achieve the objective effect");
            blsDescriptions.Add("Low", "The adversary is seeking to achieve an effect; however, the target is largely irrelevant. This example would be an attacker attempting to use open source resources (Shodan, Censys, etc) to find any systems vulnerable to a certain exploit, or any organizations within a class of organizations (targeting hospitals, retail, government, etc.)");
            blsDescriptions.Add("Very Low", "No specific target. This would likely be an opportunistic attacker");
        }
    }
}
