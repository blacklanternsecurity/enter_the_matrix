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
    public class Severity
    {
        public Dictionary<string, string> nistDescriptions { get; set; }
        public Dictionary<string, string> blsDescriptions { get; set; }
        public HtmlString methodology { get; set; }

        public Severity()
        {
            methodology = new HtmlString("This is only used if there is a finding reference. This should mirror the CVSS score of the finding. In the case of multiple findings, use the most severe as the severity value. For example: <ul><li>Finding Reference: [T001] Big Bad CVE (had a CVSS of 9.8)</li><li>Vulnerability Severity: 9</li></ul>");

            nistDescriptions = new Dictionary<string, string>();
            nistDescriptions.Add("Very High", @"The vulnerability is exposed and exploitable, and its exploitation could result in severe impacts.
                Relevant security control or other remediation is not implemented and not planned; or no security
                measure can be identified to remediate the vulnerability.");
            nistDescriptions.Add("High", @"The vulnerability is of high concern, based on the exposure of the vulnerability and ease of
                exploitation and/or on the severity of impacts that could result from its exploitation.
                Relevant security control or other remediation is planned but not implemented; compensating
                controls are in place and at least minimally effective.");
            nistDescriptions.Add("Moderate", @"The vulnerability is of moderate concern, based on the exposure of the vulnerability and ease of
                exploitation and/or on the severity of impacts that could result from its exploitation.
                Relevant security control or other remediation is partially implemented and somewhat effective.");
            nistDescriptions.Add("Low", @"The vulnerability is of minor concern, but effectiveness of remediation could be improved.
                Relevant security control or other remediation is fully implemented and somewhat effective");
            nistDescriptions.Add("Very Low", @"The vulnerability is not of concern.
                Relevant security control or other remediation is fully implemented, assessed, and effective");

            blsDescriptions = new Dictionary<string, string>();
            blsDescriptions.Add("Very High", "Exploitation of the vulnerability is highly likely, with publicly available and trivial exploits available. Exploitation would result in severe impact to the organization, and no relevant controls are in place or possible");
            blsDescriptions.Add("High", "Exploitation of the vulnerability is likely, and exploits are available with limited complexity. The organization is aware and has a plan for remediation or implementing mitigating factors");
            blsDescriptions.Add("Moderate", "The vulnerability is possible to exploit, however publicly available exploit code may not be available, or mitigating factors have been implemented to reduce likelihood or impact");
            blsDescriptions.Add("Low", "Exploitation is unlikely due to a lack of available code, or technical information available to develop an exploit. Mitigation or remediation is likely implemented, however some exposure may still exist");
            blsDescriptions.Add("Very Low", "The vulnerability is not possible to exploit or has extensive mitigating factors implemented");
        }
    }
}
