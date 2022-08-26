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

namespace Enter_The_Matrix.FactorModels
{
    public class Conditions
    {
        public HtmlString methodology { get; set; }

        public Conditions()
        {
            methodology = new HtmlString("This is something that is not necessarily technical but is considered an issue. For example: <ul><li>Predisposing Condition: IT/Admin workstations were found unlocked and signed in during assessment</li></ul>");
        }
    }
}
