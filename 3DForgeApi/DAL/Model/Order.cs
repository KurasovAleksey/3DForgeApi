using MongoDB.Bson.Serialization.Attributes;
using System;

namespace _3DForgeApi.DAL.Model
{
    public class Order
    {
        [BsonId]
        public string Id { get; set; }
        public string ContactEmail { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public bool IsApproved { get; set; }
        public bool IsValidated { get; set; }
        public Card PaymentCard { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTime Date { get; set; }
    }

}
