function ifRange(value, min, max) {
    return value <= max && value >= min;
}

function CalculateOverallLikelihood() {
    var initiationLikelihood = document.getElementById("initiation-range").value;
    var impactLikelihood = document.getElementById("adverse-range").value;

    var overallLikelihood = "";

    // Very Low Overall
    if (ifRange(initiationLikelihood, 0, 1) && ifRange(impactLikelihood, 0, 4)) {
        overallLikelihood = "Very Low";
    }
    else if (ifRange(initiationLikelihood, 0, 4) && ifRange(impactLikelihood, 0, 1)) {
        overallLikelihood = "Very Low";
    }
    // Low Overall
    else if (ifRange(initiationLikelihood, 0, 1) && ifRange(impactLikelihood, 5, 10)) {
        overallLikelihood = "Low";
    }
    else if (ifRange(initiationLikelihood, 2, 4) && ifRange(impactLikelihood, 2, 7)) {
        overallLikelihood = "Low";
    }
    else if (ifRange(initiationLikelihood, 5, 7) && ifRange(impactLikelihood, 0, 4)) {
        overallLikelihood = "Low";
    }
    else if (ifRange(initiationLikelihood, 8, 10) && ifRange(impactLikelihood, 0, 1)) {
        overallLikelihood = "Low";
    }
    // Moderate Overall
    else if (ifRange(initiationLikelihood, 2, 4) && ifRange(impactLikelihood, 8, 10)) {
        overallLikelihood = "Moderate";
    }
    else if (ifRange(initiationLikelihood, 5, 7) && ifRange(impactLikelihood, 5, 9)) {
        overallLikelihood = "Moderate";
    }
    else if (ifRange(initiationLikelihood, 8, 9) && ifRange(impactLikelihood, 2, 7)) {
        overallLikelihood = "Moderate";
    }
    else if (ifRange(initiationLikelihood, 10, 10) && ifRange(impactLikelihood, 2, 4)) {
        overallLikelihood = "Moderate";
    }
    // High Overall
    else if (ifRange(initiationLikelihood, 5, 7) && ifRange(impactLikelihood, 10, 10)) {
        overallLikelihood = "High";
    }
    else if (ifRange(initiationLikelihood, 8, 9) && ifRange(impactLikelihood, 8, 9)) {
        overallLikelihood = "High";
    }
    else if (ifRange(initiationLikelihood, 10, 10) && ifRange(impactLikelihood, 5, 7)) {
        overallLikelihood = "High";
    }
    // Very High Overall
    else if (ifRange(initiationLikelihood, 8, 9) && ifRange(impactLikelihood, 10, 10)) {
        overallLikelihood = "Very High";
    }
    else if (ifRange(initiationLikelihood, 10, 10) && ifRange(impactLikelihood, 8, 10)) {
        overallLikelihood = "Very High";
    }

    document.getElementById("likelihood").value = overallLikelihood;

    CalculateOverallRisk();
}

function CalculateOverallRisk() {
    var overallLikelihood = document.getElementById('likelihood').value;
    var impact = document.getElementById("impact-range").value;
    var overallRisk = "";

    if (overallLikelihood === "Very Low" || overallLikelihood === "") {
        if (ifRange(impact, 0, 7)) {
            overallRisk = "Very Low";
        }
        else if (ifRange(impact, 8, 10)) {
            overallRisk = "Low";
        }
    }
    else if (overallLikelihood === "Low") {
        if (ifRange(impact, 0, 1)) {
            overallRisk = "Very Low";
        }
        else if (ifRange(impact, 2, 9)) {
            overallRisk = "Low";
        }
        else if (ifRange(impact, 10, 10)) {
            overallRisk = "Moderate";
        }
    }
    else if (overallLikelihood === "Moderate") {
        if (ifRange(impact, 0, 1)) {
            overallRisk = "Very Low";
        }
        else if (ifRange(impact, 2, 4)) {
            overallRisk = "Low";
        }
        else if (ifRange(impact, 5, 9)) {
            overallRisk = "Moderate";
        }
        else if (ifRange(impact, 10, 10)) {
            overallRisk = "High"
        }
    }
    else if (overallLikelihood === "High" || overallLikelihood === "Very High") {
        if (ifRange(impact, 0, 1)) {
            overallRisk = "Very Low";
        }
        else if (ifRange(impact, 2, 4)) {
            overallRisk = "Low";
        }
        else if (ifRange(impact, 5, 7)) {
            overallRisk = "Moderate";
        }
        else if (ifRange(impact, 8, 9)) {
            overallRisk = "High";
        }
        else if (ifRange(impact, 10, 10)) {
            overallRisk = "Very High";
        }
    }

    document.getElementById('risk').value = overallRisk;
}