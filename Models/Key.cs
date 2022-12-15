using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Enter_The_Matrix.Models
{
    public class Key
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public List<string> AssessmentPrivileges { get; set; }
        public List<string> ScenarioPrivileges { get; set; }
        public List<string> EventPrivileges { get; set; }
        public List<string> TemplatePrivileges { get; set; }
        public List<string> MetricsPrivileges { get; set; }
        public string AssessmentId { get; set; }
    }
}
