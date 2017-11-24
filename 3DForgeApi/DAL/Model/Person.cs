using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3DForgeApi.DAL.Model
{
    public enum Role
    {
        Admin = 0,
        Manager = 1
    }

    public class Person
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Role Role { get; set; }
    }
}
