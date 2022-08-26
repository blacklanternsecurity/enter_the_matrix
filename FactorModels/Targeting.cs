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
    public class Targeting
    {
        public Dictionary<string, string> nistDescriptions { get; set; }
        public Dictionary<string, string> blsDescriptions { get; set; }
        public HtmlString methodology { get; set; }
        public Targeting()
        {
            methodology = new HtmlString("How does your threat source choose their targets? <ul><li>Randomly or broadly scaled typically falls within [0-4]<ul><li>Spray and pray, hot new CVE, mass phishing campaign using OSINT</li></ul></li><li>Certain market verticals would typically be between [5-7]<ul><li>They target banks/financial institutions using OSINT</li></ul></li><li>Certain organization would be between [8-10]<ul><li>They only target a certain corporation/organization using OSINT and information from past attacks</li></ul></li></ul>");

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
