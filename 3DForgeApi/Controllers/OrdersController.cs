using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _3DForgeApi.DAL.Model;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace _3DForgeApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET api/values
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orderRepository.GetAllOrders();
        }


        // GET api/values/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<Order> Get(string id)
        {
            return await _orderRepository.GetOrder(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Order order)
        {
            _orderRepository.AddOrder(order);
        }

        [HttpPost("uploadFile")]
        public async Task<string> UploadFile(IFormFile file)
        {
            return (await _orderRepository.UploadFile(file)).ToString();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("downloadFile/{id}")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            var stream = await _orderRepository.DownloadFile(id);
            stream.Position = 0;
            return File(stream, "application/octet-stream", "file.stl");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("rejectOrder/{id}")]
        public async Task RejectOrder(string id)
        {
            Order o = await _orderRepository.GetOrder(id);
            o.IsValidate = false;
            await _orderRepository.UpdateOrder(id, o);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approveOrder/{id}")]
        public async Task ApproveOrder(string id)
        {
            Order o = await _orderRepository.GetOrder(id);
            o.IsApproved = true;
            await _orderRepository.UpdateOrder(id, o);
        }

        [Authorize(Roles = "Admin")]
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _orderRepository.RemoveOrder(id);
        }
    }
}
