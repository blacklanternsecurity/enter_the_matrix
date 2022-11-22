/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Updated:     08-18-2022
# Copyright:   (c) BLS OPS LLC. 2021
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Enter_The_Matrix.FactorModels
{
    public class Techniques
    {
        public List<string> htmlContent { get; set; }
        private Dictionary<string, string> techniques { get; set; }
        public Dictionary<string, string> techniquesFull { get; set; }
        public Dictionary<string, Dictionary<string, string>> enterpriseTechniquesFull { get; set; }
        
        public Dictionary<string, Dictionary<string, string>> icsTechniquesFull { get; set; }

        public Dictionary<string, Dictionary<string, string>> mobileTechniquesFull { get; set; }
        public Techniques(string selected)
        {
            #region techniquesFull

            // ATT&CK Make Over

            // Parsing Data
            enterpriseTechniquesFull = new Dictionary<string, Dictionary<string, string>>();
            icsTechniquesFull = new Dictionary<string, Dictionary<string, string>>();
            mobileTechniquesFull = new Dictionary<string, Dictionary<string, string>>();
            techniquesFull = new Dictionary<string, string>();

            (string, string) selectedTechnique = ("", "");
            if (selected != null)
            {
                selectedTechnique.Item1 = selected;
            }
            int d = 0;
            // When an update is pushed, remove the four generated JSON files
            // techniquesFull.json, enterpriseTechniquesFull.json, mobileTechniquesFull.json, icsTechniquesFull.json
            // This will force the application to re-parse the MITRE provided JSON files and re-create the minimized datasets
            // Hopefully this will improve runtime
            if (!File.Exists("techniquesFull.json"))
            {
                dynamic enterpriseJsonRaw = JsonConvert.DeserializeObject(File.OpenText("mitre-enterprise.json").ReadToEnd());
                dynamic icsJsonRaw = JsonConvert.DeserializeObject(File.OpenText("mitre-ics.json").ReadToEnd());
                dynamic mobileJsonRaw = JsonConvert.DeserializeObject(File.OpenText("mitre-mobile.json").ReadToEnd());

                List<dynamic> datasets = new List<dynamic>();
                datasets.Add(enterpriseJsonRaw);
                datasets.Add(icsJsonRaw);
                datasets.Add(mobileJsonRaw);
                foreach (var dataset in datasets)
                {
                    Dictionary<string, Dictionary<string, string>> resultantSet = new Dictionary<string, Dictionary<string, string>>();
                    foreach (var tech in dataset.objects)
                    {
                        if (tech.type == "attack-pattern")
                        {
                            if ((tech.x_mitre_deprecated != null && tech.x_mitre_deprecated == true) || (tech.revoked != null && tech.revoked == true))
                            {
                                continue;
                            }
                            foreach (var phase in tech.kill_chain_phases)
                            {
                                // Check that our tactic is added already
                                if (!resultantSet.ContainsKey((String)phase.phase_name))
                                {
                                    resultantSet[(String)phase.phase_name] = new Dictionary<string, string>();
                                }

                                // Add the technique to the appropriate tactics
                                // Formatting technique data to fit the dataset
                                String techId = ((String)tech.external_references[0].external_id).Replace(".", "/");
                                String techName = (String)tech.name;

                                // Adding technique to resultant set
                                resultantSet[(String)phase.phase_name][techId] = techName;

                                // Adding technique to master set
                                techniquesFull[techId] = techName;

                                // Updating selected technique
                                if (techId == selected)
                                {
                                    selectedTechnique.Item2 = techName;
                                }

                            }
                        }
                    }
                    switch (d)
                    {
                        case 0:
                            enterpriseTechniquesFull = resultantSet;
                            File.WriteAllText("enterpriseTechniquesFull.json", JsonConvert.SerializeObject(enterpriseTechniquesFull));
                            break;
                        case 1:
                            icsTechniquesFull = resultantSet;
                            File.WriteAllText("icsTechniquesFull.json", JsonConvert.SerializeObject(icsTechniquesFull));
                            break;
                        case 2:
                            mobileTechniquesFull = resultantSet;
                            File.WriteAllText("mobileTechniquesFull.json", JsonConvert.SerializeObject(mobileTechniquesFull));
                            break;
                    }
                    d = d + 1;
                }
                File.WriteAllText("techniquesFull.json", JsonConvert.SerializeObject(techniquesFull));
            }
            else
            {
                techniquesFull = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.OpenText("techniquesFull.json").ReadToEnd());
                enterpriseTechniquesFull = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.OpenText("enterpriseTechniquesFull.json").ReadToEnd());
                mobileTechniquesFull = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.OpenText("mobileTechniquesFull.json").ReadToEnd());
                icsTechniquesFull = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(File.OpenText("icsTechniquesFull.json").ReadToEnd());
                if (techniquesFull.ContainsKey(selected))
                {
                    selectedTechnique.Item2 = techniquesFull[selected];
                }
                else
                {
                    selectedTechnique.Item2 = "Deprecated";
                }
            }
            #endregion

            //start off our htmlContent
            htmlContent = new List<string>();
            if (selected != "")
            {
                htmlContent.Add("<div style='padding-bottom:15px; padding-top:15px;'>Selected Technique: <span id='selectedTechniqueSpan'>" + selectedTechnique.Item1 + " - " + selectedTechnique.Item2 + "</span></div>");
            }
            else
            {
                htmlContent.Add("<div style='padding-bottom:15px; padding-top:15px;'>Selected Technique: <span id='selectedTechniqueSpan'></span></div>");
            }
            
            htmlContent.Add(@"
<script>
document.addEventListener('DOMContentLoaded', function () {
  const menuElement = document.getElementById('mitre-menu');
            const menu = new SlideMenu(menuElement);
        });
</script>
<nav class='slide-menu' id='mitre-menu'>
  <ul>");

            // Populate our techniques list

            // tactics/enterprise -- count 14 at time of writing
            #region tactics/enterprise

            htmlContent.Add("<li>");
            htmlContent.Add("<a id='filter-home' href='javascript:void(0)'>ENTERPRISE</a>");
            htmlContent.Add("<ul>");

            #region tactics TA0043
            // Reconnaissance TA0043
            // Add the TA0043->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("reconnaissance"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Reconnaissance</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["reconnaissance"], selected);
                htmlContent.Add("</ul></li>");
            }
            #endregion

            #region tactics TA0042
            // Resource Development
            // Add the TA0042->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("resource-development"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Resource Development</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["resource-development"], selected);
                htmlContent.Add("</ul></li>");
            }
            #endregion

            #region tactics TA0001

            // Initial Access
            // Add the TA0001->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("initial-access"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Initial Access</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["initial-access"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0002
            // Execution
            // Add the TA0002->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("execution"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Execution</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["execution"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0003
            // Persistence
            // Add the TA0003->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("persistence"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Persistence</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["persistence"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0004
            // Privilege Escalation
            // Add the TA0004->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("privilege-escalation"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Privilege Escalation</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["privilege-escalation"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0005
            // Defense Evasion
            // Add the TA0005->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("defense-evasion"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Defense Evasion</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["defense-evasion"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0006
            // Credential Access
            // Add the TA0006->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("credential-access"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Credential Access</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["credential-access"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0007
            // Discovery
            // Add the TA0007->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("discovery"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Discovery</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["discovery"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0008
            // Lateral Movement
            // Add the TA0008->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("lateral-movement"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Lateral Movement</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["lateral-movement"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0009
            // Collection
            // Add the TA0009->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("collection"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Collection</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["collection"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0011
            // Command and Control
            // Add the TA0011->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("command-and-control"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Command & Control</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["command-and-control"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0010
            // Exfiltration
            // Add the TA0010->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("exfiltration"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Exfiltration</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["exfiltration"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0040
            // Impact
            // Add the TA0040->techniques to our options list
            if (enterpriseTechniquesFull.ContainsKey("impact"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Impact</a><ul>");
                htmlContent = AddHtml(htmlContent, enterpriseTechniquesFull["impact"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            htmlContent.Add("</ul>");
            htmlContent.Add("</li>");

            #endregion

            // tactics/mobile
            #region tactics/mobile

            //htmlContent.Add("<optgroup label='Mobile'>");

            htmlContent.Add("<li>");
            htmlContent.Add("<a href='javascript:void(0)'>MOBILE</a>");
            htmlContent.Add("<ul>");

            #region tactics TA0027

            // Initial Access
            // Add the TA0027->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("initial-access"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Initial Access</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["initial-access"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0041

            // Execution
            // Add the TA0041->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("execution"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Execution</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["execution"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0028

            // Persistence
            // Add the TA0028->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("persistence"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Persistence</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["persistence"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0029

            // Privilege Escalation
            // Add the TA0029->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("privilege-escalation"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Privilege Escalation</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["privilege-escalation"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0030

            // Defense Evasion
            // Add the TA0030->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("defense-evasion"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Defense Evasion</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["defense-evasion"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0031

            // Credential Access
            // Add the TA0031->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("credential-access"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Credential Access</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["credential-access"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0032

            // Discovery
            // Add the TA0032->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("discovery"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Discovery</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["discovery"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0033

            // Lateral Movement
            // Add the TA0033->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("lateral-movement"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Lateral Movement</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["lateral-movement"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0035

            // Collection
            // Add the TA0035->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("collection"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Collection</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["collection"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0037

            // Command and Control
            // Add the TA0037->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("command-and-control"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Command & Control</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["command-and-control"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0036

            // Exfiltration
            // Add the TA0036->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("exfiltration"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Exfiltration</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["exfiltration"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0034

            // Impact
            // Add the TA0034->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("impact"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Impact</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["impact"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0038

            // Network Effects
            // Add the TA0038->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("network-effects"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Network Effects</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["network-effects"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics TA0039

            // Remote Service Effects
            // Add the TA0039->techniques to our options list
            if (mobileTechniquesFull.ContainsKey("remote-service-effects"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Remote Service Effects</a><ul>");
                htmlContent = AddHtml(htmlContent, mobileTechniquesFull["remote-service-effects"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            htmlContent.Add("</ul>");
            htmlContent.Add("</li>");

            #endregion

            // tactics/ICS
            #region ICS

            htmlContent.Add("<li>");
            htmlContent.Add("<a href='javascript:void(0)'>ICS</a>");
            htmlContent.Add("<ul>");

            #region tactics Collection
            // Collection
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("collection-ics"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Collection</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["collection-ics"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics Command and Control

            // Command and Control
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("command-and-control-ics"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Command & Control</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["command-and-control-ics"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics Discovery

            // Discovery
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("discovery-ics"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Discovery</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["discovery-ics"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics Evasion

            // Evasion
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("evasion-ics"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Evasion</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["evasion-ics"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics Execution

            // Execution
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("execution-ics"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Execution</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["execution-ics"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics Impact

            // Impact
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("impact-ics"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Impact</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["impact-ics"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics Impair Process Control

            // Impair Process Control
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("impair-process-control"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Impair Process Control</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["impair-process-control"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics Inhibit Response Function

            // Inhibit Response Function
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("inhibit-response-function"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Inhibit Response Function</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["inhibit-response-function"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics Initial Access

            // Initial Access
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("initial-access-ics"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Initial Access</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["initial-access-ics"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics Lateral Movement

            // Lateral Movement
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("lateral-movement-ics"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Lateral Movement</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["lateral-movement-ics"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics Persistence

            // Persistence
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("persistence-ics"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Persistence</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["persistence-ics"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            #region tactics Privilege Escalation

            // Privilege Escalation
            // Add the techniques to our options list
            if (icsTechniquesFull.ContainsKey("privilege-escalation"))
            {
                htmlContent.Add("<li><a href='javascript:void(0)'>Privilege Escalation</a><ul>");
                htmlContent = AddHtml(htmlContent, icsTechniquesFull["privilege-escalation"], selected);
                htmlContent.Add("</ul></li>");
            }

            #endregion

            htmlContent.Add("</ul>");
            htmlContent.Add("</li>");

            #endregion

            htmlContent.Add("</ul>");
            htmlContent.Add("</nav>");
        }

        private List<string> AddHtml(List<string> htmlContent, Dictionary<string, string> techniques, string selected)
        {
            List<string> keyList = techniques.Keys.ToList<string>();
            keyList.Sort();
            String prevTech = "";
            foreach (var technique in keyList)
            {
                
                // New
                if (prevTech == "")
                {
                    // Situation where we are first
                    htmlContent.Add("<li>");
                }
                else if (prevTech != "" && technique.Contains(prevTech))
                {
                    // Situation where previous was T1000 and this is T1000/100
                    htmlContent.Add("<ul><li>");
                }
                else if (prevTech != "" && technique.Split("/")[0] == prevTech.Split("/")[0])
                {
                    // Situation where previous was T1000/100 and this is T1000/101
                    htmlContent.Add("</li><li>");
                }
                else if (prevTech != "" && prevTech.Contains("/") && !technique.Contains("/"))
                {
                    // Situation where previous was T1000/101 and this is T1001
                    htmlContent.Add("</li></ul><li>");
                }
                else
                {
                    // Previous technique was T1001 and this is T1002
                    htmlContent.Add("</li><li>");
                }

                // Adding bells and whistles
                if (selected == technique)
                {
                    htmlContent.Add("<input class='form-check-input-mitre' value='" + technique + "' id='radio-" + technique + "' type='radio' name='mitreId' checked='checked' onclick='document.getElementById(\"selectedTechniqueSpan\").innerHTML=\"" + technique + " - " + techniques[technique] + "\";'>");
                }
                else { htmlContent.Add("<input class='form-check-input-mitre' value='" + technique + "' id='radio-" + technique + "' type='radio' name='mitreId' onclick='document.getElementById(\"selectedTechniqueSpan\").innerHTML=\"" + technique + " - " + techniques[technique] + "\";'>"); }

                htmlContent.Add("<a href='javascript:void(0)'>" + technique.ToUpper() + " - " + techniques[technique] + "</a>");

                string url = @"https://attack.mitre.org/techniques/";
                string tech = technique + "/";
                if (technique.StartsWith("ics-")) { url = @"https://collaborate.mitre.org/attackics/index.php/Technique/"; tech = technique.Substring(4); }
                //htmlContent.Add("<a onclick=\"window.open(\'" + url + tech + "\', \'_blank\');\" style='margin-left: 5px;' href='" + url + tech + "' target='_blank'>");
                htmlContent.Add("<span onclick=\"window.open(\'" + url + tech + "\', \'_blank\');\" style='margin-left: 5px;'>");
                htmlContent.Add("<svg width='1em' height='1em' viewBox='0 0 16 16' class='bi bi-info-square' fill='currentColor' xmlns='http://www.w3.org/2000/svg'>");
                htmlContent.Add("<path fill-rule='evenodd' d='M14 1H2a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1zM2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2z' />");
                htmlContent.Add("<path d='M8.93 6.588l-2.29.287-.082.38.45.083c.294.07.352.176.288.469l-.738 3.468c-.194.897.105 1.319.808 1.319.545 0 1.178-.252 1.465-.598l.088-.416c-.2.176-.492.246-.686.246-.275 0-.375-.193-.304-.533L8.93 6.588z' />");
                htmlContent.Add("<circle cx='8' cy='4.5' r='1' />");
                htmlContent.Add("</svg>");
                //htmlContent.Add("</a>");
                htmlContent.Add("</span>");


                prevTech = technique;

                // Old
                /*
                htmlContent.Add("<div class='card-body bg-dark' id='" + technique + "'>");
                htmlContent.Add("<div class='card' style='border:none;'>");
                htmlContent.Add("<div class='card-header bg-dark' id='heading-" + technique + "'>");
                

                htmlContent.Add("</div>");
                htmlContent.Add("</div>");
                htmlContent.Add("</div>");
                */
            }

            // We now need to properly close things
            if (!prevTech.Contains("/"))
            {
                // Situation where the last technique was a parent (T1000)
                // Close parent li
                htmlContent.Add("</li>");
            }
            else if (prevTech.Contains("/"))
            {
                // Situation where the last technique was a child (T1000/100)
                // Close child, close children ul, close parent li
                htmlContent.Add("</li></ul></li>");
            }

            return htmlContent;
        }
    }
}
