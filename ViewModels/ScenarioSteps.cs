/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Created:     10-15-2020
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enter_The_Matrix.Models;

namespace Enter_The_Matrix.ViewModels
{
    public class ScenarioSteps
    {
        public Scenarios scenario { get; set; }
        public List<Steps> stepList { get; set; }
        public Assessments assessment { get; set; }
    }
}
