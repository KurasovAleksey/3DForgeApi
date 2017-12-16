using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3DForgeApi.DAL.Model
{
    public enum CardBrand
    {
        Visa = 0,
        MasterCard = 1
    }

    public class Card
    {
        public Card()
        {
            Brand = CardBrand.Visa;
        }

        public string Number { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public CardBrand Brand { get; set; }
    }
}
