/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     10-15-2020
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Enter_The_Matrix.FactorModels
{
    public class ThreatSources
    {
        public List<string> sources { get; set; }
        public string nistDescription { get; set; }
        public ThreatSources()
        {
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
        }
    }
}
