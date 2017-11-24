using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _3DForgeApi.DAL.Model
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrder(string id);
        Task AddOrder(Order order);
        Task<DeleteResult> RemoveOrder(string id);
        Task<UpdateResult> UpdateOrder(string id, Order order);
        Task<ObjectId> UploadFile(IFormFile file);
        Task<MemoryStream> DownloadFile(string id);
    }
}
