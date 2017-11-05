using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace _3DForgeApi.DAL.Model
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersContext _context = null;

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

        public async Task<UpdateResult> UpdateOrder(string id, bool isApproved, bool isValidated)
        {
            var filter = Builders<Order>.Filter.Eq("Id", id);
            var update = Builders<Order>.Update
                .Set("IsApproved", isApproved)
                .Set("IsValidated", isValidated);

            return await _context.Orders.UpdateOneAsync(filter, update);

        }

        public async Task<DeleteResult> RemoveOrder(string id)
        {
            var filter = Builders<Order>.Filter.Eq("Id", id);
            return await _context.Orders.DeleteOneAsync(filter);
        }

        
    }
}
