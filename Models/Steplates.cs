using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Enter_The_Matrix.Models
{
    public class Steplates
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Event is required")]
        public string Event { get; set; }
        [Required(ErrorMessage = "Threat source is required")]
        public string ThreatSource { get; set; }
        [Required(ErrorMessage = "Capability is required")]
        public Int32 Capability { get; set; }
        [Required(ErrorMessage = "Intent is required")]
        public Int32 Intent { get; set; }
        [Required(ErrorMessage = "Targeting is required")]
        public Int32 Targeting { get; set; }
        [Required(ErrorMessage = "Relevance is required")]
        public string Relevance { get; set; }
        [Required(ErrorMessage = "Initiation is required")]
        public Int32 Initiation { get; set; }
        [Required(ErrorMessage = "Vulnerability is required")]
        public string Vulnerability { get; set; }
        [Required(ErrorMessage = "Severity is required")]
        public Int32 Severity { get; set; }
        [Required(ErrorMessage = "Predisposing condition is required")]
        public string Condition { get; set; }
        [Required(ErrorMessage = "Pervasiveness is required")]
        public Int32 Pervasiveness { get; set; }
        [Required(ErrorMessage = "Mitigation is required")]
        public string Mitigation { get; set; }
        [Required(ErrorMessage = "Adverse is required")]
        public Int32 Adverse { get; set; }
        [Required(ErrorMessage = "Likelihood is required")]
        public string Likelihood { get; set; }
        [Required(ErrorMessage = "Impact is required")]
        public Int32 Impact { get; set; }
        [Required(ErrorMessage = "Risk is required")]
        public string Risk { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Added by is required")]
        public string AddedBy { get; set; }
        public Node GraphNode { get; set; }
        public string MitreId { get; set; }
    }
}
