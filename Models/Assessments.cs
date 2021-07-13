using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Enter_The_Matrix.Models
{
    public class Assessments
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Created by is required")]
        public string CreatedBy { get; set; }
        public string[] Scenarios { get; set; }
        public string ThreatTreeId { get; set; }

    }
}
