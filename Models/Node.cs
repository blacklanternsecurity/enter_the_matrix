using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enter_The_Matrix.Models
{
    public class Node
    {
        public string[] ParentId { get; set; }
        public string EntityType { get; set; }
        public string EntityDescription { get; set; }
        public string Risk { get; set; }
        public string Id { get; set; }

        public Node(string[] parentId, string entityType, string entityDescription, string risk, string id)
        {
            ParentId = parentId;
            Id = id;
            EntityType = entityType;
            EntityDescription = entityDescription;
            Risk = risk;
        }

        public string GetRiskColor()
        {
            if (Risk == "Very Low") { return "cyan"; }
            else if (Risk == "Low") { return "lawngreen"; }
            else if (Risk == "Moderate") { return "gold"; }
            else if (Risk == "High") { return "darkorange"; }
            else if (Risk == "Very High") { return "red"; }
            else return "";
        }

        public string GetEntityIcon()
        {
            return "../icons/" + EntityType + ".png";
        }

    }
}
