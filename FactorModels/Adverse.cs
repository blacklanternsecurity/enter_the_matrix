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
    public class Adverse
    {
        public Dictionary<string, string> nistDescriptions;
        public Dictionary<string, string> blsDescriptions;

        public Adverse()
        {
            nistDescriptions = new Dictionary<string, string>();
            nistDescriptions.Add("Very High", "If the threat event is initiated or occurs, it is almost certain to have adverse impacts.");
            nistDescriptions.Add("High", "If the threat event is initiated or occurs, it is highly likely to have adverse impacts.");
            nistDescriptions.Add("Moderate", "If the threat event is initiated or occurs, it is somewhat likely to have adverse impacts.");
            nistDescriptions.Add("Low", "If the threat event is initiated or occurs, it is unlikely to have adverse impacts.");
            nistDescriptions.Add("Very Low", "If the threat event is initiated or occurs, it is highly unlikely to have adverse impacts.");

            blsDescriptions = new Dictionary<string, string>();
            blsDescriptions.Add("Very High", "The attack will definitely cause an adverse impact");
            blsDescriptions.Add("High", "The attack is very likely to cause an adverse impact but is not guaranteed");
            blsDescriptions.Add("Moderate", "It is possible that the attack could cause an adverse impact, but other factors largely influence this");
            blsDescriptions.Add("Low", "It is not likely that the attack could cause an adverse impact, but there is a small chance of affect");
            blsDescriptions.Add("Very Low", "There is almost no chance that the attack could cause an adverse impact");
        }
    }
}
