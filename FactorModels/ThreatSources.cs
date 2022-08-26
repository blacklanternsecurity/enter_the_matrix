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
    public class ThreatSources
    {
        public List<string> sources { get; set; }
        public string nistDescription { get; set; }
        public string blsDescription { get; set; }
        public HtmlString methodology { get; set; }
        public ThreatSources()
        {
            methodology = new HtmlString("Choose the threat source you are modeling. This basically falls into one of individual, group, or nation state.");

            sources = new List<string>();
            // Per NIST guidelines:
            sources.Add("Insider");
            sources.Add("Outsider");
            sources.Add("Trusted Insider");
            sources.Add("Privileged Insider");
            sources.Add("Ad Hoc Group");
            sources.Add("Established Group");
            sources.Add("Competitor Organization");
            sources.Add("Supplier Organization");
            sources.Add("Partner Organization");
            sources.Add("Customer Organization");
            sources.Add("Nation State");

            nistDescription = @"Individuals, groups, organizations, or states that seek to
                exploit the organization’s dependence on cyber
                resources (i.e., information in electronic form, information
                and communications technologies, and the
                communications and information-handling capabilities
                provided by those technologies)";

            blsDescription = @"When selecting your threat source, it is important to model 
                the associated Capability, Intent, and Targeting characteristics in a way that is consistent
                across the entire scenario. Keep in mind that, in most cases, the threat source
                is not changing during the scenario. This means that the threat source's modeled characteristics
                should remain the same across each event. If you are unsure, refer to some of 
                the templated threat sources for examples.";
        }
    }
}
