/*
# -------------------------------------------------------------------------------
# Author:      Cody Martin <cody.martin@blacklanternsecurity.com>
#
# Updated:     08-25-2022
# Copyright:   (c) BLS OPS LLC. 2020
# Licence:     GPL
# -------------------------------------------------------------------------------
*/

using System.Collections.Generic;

namespace Enter_The_Matrix.FactorModels
{
    public class EntityTypes
    {
        private Dictionary<string, string> Entities { get; set; }
        public EntityTypes()
        {
            Entities = new Dictionary<string, string>();

            Entities.Add("autonomous-system", "Autonomous System");
            Entities.Add("business-leader", " Business Leader");
            Entities.Add("business-meeting", "Business Meeting");
            Entities.Add("camera-device", "Camera");
            Entities.Add("cell-phone", "Cellphone");
            Entities.Add("company", "Company");
            Entities.Add("compromised-host", "Compromised Host");
            Entities.Add("cve", "CVE");
            Entities.Add("desktop", "Desktop");
            Entities.Add("dns-name", "DNS Name");
            Entities.Add("document", "Document");
            Entities.Add("domain", "Domain");
            Entities.Add("email-conversation", "Email Conversation");
            Entities.Add("employee", "Employee");
            Entities.Add("exploit", "Exploit");
            Entities.Add("group", "Group");
            Entities.Add("guard-personnel", "Guard Personnel");
            Entities.Add("hash", "Hash");
            Entities.Add("hashtag", "Hashtag");
            Entities.Add("incident", "Incident");
            Entities.Add("ipv4-address", "IPv4 Address");
            Entities.Add("laptop", "Laptop");
            Entities.Add("malicious-actor", "Malicious Actor");
            Entities.Add("malicious-leader", "Malicious Leader");
            Entities.Add("malicious-process", "Malicious Process");
            Entities.Add("mx-record", "MX Record");
            Entities.Add("netblock", "Netblock");
            Entities.Add("network-service-banner", "Network Service Banner");
            Entities.Add("ns-record", "NS Record");
            Entities.Add("online-group", "Online Group");
            Entities.Add("organization", "Organization");
            Entities.Add("person", "Person");
            Entities.Add("phone-number", "Phone Number");
            Entities.Add("port", "Port");
            Entities.Add("server", "Server");
            Entities.Add("social-meeting", "Social Meeting");
            Entities.Add("unknown-suspect", "Unknown Suspect");
            Entities.Add("url-title", "URL Title");
            Entities.Add("user-account", "User Account");
            Entities.Add("vulnerability-id", "Vulnerability ID");
            Entities.Add("vulnerability", "Vulnerability");
            Entities.Add("web-directory", "Web Directory");
            Entities.Add("website", "Website");
        }
        
        public Dictionary<string, string> getTypes()
        {
            return Entities;
        }

    }

}
