using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace _3DForgeApi.DAL.Model
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContext _context = null;

        public OrderRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders.Find(_ => true).ToListAsync();
        }

        public async Task<Order> GetOrder(string id)
        {
            var filter = Builders<Order>.Filter.Eq("Id", id);
            return await _context.Orders
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task AddOrder(Order order)
        {
            await _context.Orders.InsertOneAsync(order);
        }

        public async Task<UpdateResult> UpdateOrder(string id, Order order)
        {
            var filter = Builders<Order>.Filter.Eq("Id", id);
            var update = Builders<Order>.Update
                .Set(o => o.IsApproved, order.IsApproved)
                .Set(o => o.IsValidate, order.IsValidate)
                .Set(o => o.PaymentCard, order.PaymentCard)
                .Set(o => o.ShippingAddress, order.ShippingAddress);

            return await _context.Orders.UpdateOneAsync(filter, update);

        }

        public async Task<DeleteResult> RemoveOrder(string id)
        {
            var filter = Builders<Order>.Filter.Eq("Id", id);
            return await _context.Orders.DeleteOneAsync(filter);
        }

        public async Task<ObjectId> UploadFile(IFormFile file)
        {
            try
            {
                var stream = file.OpenReadStream();
                var filename = file.FileName;
                return await _context.Bucket.UploadFromStreamAsync(filename, stream);
            }
            catch (Exception ex)
            {
                return new ObjectId(ex.ToString());
            }
        }

        public async Task<MemoryStream> DownloadFile(string id)
        {
            ObjectId objectId = new ObjectId(id);
            MemoryStream destination = new MemoryStream();
            await _context.Bucket.DownloadToStreamAsync(objectId, destination);
            return destination;

        }
    }
}
