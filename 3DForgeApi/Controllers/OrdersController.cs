using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _3DForgeApi.DAL.Model;

namespace _3DForgeApi.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _orderRepository.GetAllOrders();
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<Order> Get(string id)
        {
            return await _orderRepository.GetOrder(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            
        }

       

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _orderRepository.RemoveOrder(id);
        }
    }
}
