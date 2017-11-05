using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3DForgeApi.DAL.Model
{
    interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrder(string id);
        Task AddOrder(Order order);
        Task<DeleteResult> RemoveOrder(string id);
        Task<UpdateResult> UpdateOrder(string id, bool isApproved, bool isValidated);
    }
}
