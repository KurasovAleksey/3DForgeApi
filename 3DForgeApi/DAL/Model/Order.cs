using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _3DForgeApi.DAL.Model
{
    public class Order
    {
        public Order()
        {
            Date = DateTime.Now;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ContactEmail { get; set; }
        public string Info { get; set; }
        public int Quantity { get; set; }
        public string Material { get; set; }
        public int TotalPrice { get; set; }
        public string AttachedFileId { get; set; }
        public bool IsClosed { get; set; } = false;
        public bool? IsValid { get; set; }
        public Card PaymentCard { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTime Date { get; set; }
    }

}
