using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Enter_The_Matrix.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string PasswordHash { get; set; }
    }
}
