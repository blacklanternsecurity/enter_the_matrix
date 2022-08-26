// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function createKey() {
    // Grabbing Key Name
    var keyName = document.getElementById("keyName").value;

    // Grabbing Privilegegs
    var assessmentPrivileges = [];
    if (document.getElementById("assessmentPrivC").checked) { assessmentPrivileges.push(document.getElementById("assessmentPrivC").value); }
    if (document.getElementById("assessmentPrivR").checked) { assessmentPrivileges.push(document.getElementById("assessmentPrivR").value); }
    if (document.getElementById("assessmentPrivU").checked) { assessmentPrivileges.push(document.getElementById("assessmentPrivU").value); }
    if (document.getElementById("assessmentPrivD").checked) { assessmentPrivileges.push(document.getElementById("assessmentPrivD").value); }

    var scenarioPrivileges = [];
    if (document.getElementById("scenarioPrivC").checked) { scenarioPrivileges.push(document.getElementById("scenarioPrivC").value); }
    if (document.getElementById("scenarioPrivR").checked) { scenarioPrivileges.push(document.getElementById("scenarioPrivR").value); }
    if (document.getElementById("scenarioPrivU").checked) { scenarioPrivileges.push(document.getElementById("scenarioPrivU").value); }
    if (document.getElementById("scenarioPrivD").checked) { scenarioPrivileges.push(document.getElementById("scenarioPrivD").value); }

    var eventPrivileges = [];
    if (document.getElementById("eventPrivC").checked) { eventPrivileges.push(document.getElementById("eventPrivC").value); }
    if (document.getElementById("eventPrivR").checked) { eventPrivileges.push(document.getElementById("eventPrivR").value); }
    if (document.getElementById("eventPrivU").checked) { eventPrivileges.push(document.getElementById("eventPrivU").value); }
    if (document.getElementById("eventPrivD").checked) { eventPrivileges.push(document.getElementById("eventPrivD").value); }

    var templatePrivileges = [];
    if (document.getElementById("templatePrivC").checked) { templatePrivileges.push(document.getElementById("templatePrivC").value); }
    if (document.getElementById("templatePrivR").checked) { templatePrivileges.push(document.getElementById("templatePrivR").value); }
    if (document.getElementById("templatePrivU").checked) { templatePrivileges.push(document.getElementById("templatePrivU").value); }
    if (document.getElementById("templatePrivD").checked) { templatePrivileges.push(document.getElementById("templatePrivD").value); }

    var metricsPrivileges = [];
    if (document.getElementById("metricsPrivC").checked) { metricsPrivileges.push(document.getElementById("metricsPrivC").value); }
    if (document.getElementById("metricsPrivR").checked) { metricsPrivileges.push(document.getElementById("metricsPrivR").value); }
    if (document.getElementById("metricsPrivU").checked) { metricsPrivileges.push(document.getElementById("metricsPrivU").value); }
    if (document.getElementById("metricsPrivD").checked) { metricsPrivileges.push(document.getElementById("metricsPrivD").value); }

    $.ajax({
        url: "/AdminDashboard/CreateKey",
        data: {
            "keyName": keyName,
            "assessmentPrivileges": assessmentPrivileges,
            "scenarioPrivileges": scenarioPrivileges,
            "eventPrivileges": eventPrivileges,
            "templatePrivileges": templatePrivileges,
            "metricsPrivileges": metricsPrivileges
        },
        type: "POST",
        success: function (retKey) {
            $("#createKeyModal").modal("hide");
            displayKey(retKey);
        }
    });
}

function displayKey(retKey) {
    $("#newKey").text(retKey.replaceAll("\"", ""));
    $("#displayKeyModal").modal({ backdrop: 'static', keyboard: false });
}