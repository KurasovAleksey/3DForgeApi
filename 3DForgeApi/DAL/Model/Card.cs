using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3DForgeApi.DAL.Model
{
    public enum CardBrend
    {
        Visa,
        MasterCard
    }

    public class Card
    {
        public string Number { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [BsonRepresentation(BsonType.String)]
        public CardBrend Brend { get; set; }
    }
}
