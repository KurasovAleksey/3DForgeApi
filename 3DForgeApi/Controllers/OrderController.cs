using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _3DForgeApi.DAL.Model;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace _3DForgeApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET api/values
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task GetOrders(string email, int page = 1, int pageSize = 10, bool isClosed = false, bool? isValid = null)
        {
            var orders = (await _orderRepository.GetAllOrders()).OrderByDescending(o => o.Date).ToList();

            var emailFilter = (!String.IsNullOrEmpty(email)) ?
                orders.Where(order => order.ContactEmail == email).ToList() : orders.ToList();

            var orderList = emailFilter
                .Where(order => order.IsClosed == isClosed)
                .Where(order => order.IsValid == isValid)
                .ToList();

            double totalCount = orderList.Count;
            int pages = (int)Math.Ceiling(totalCount / pageSize);
            var ordersPerPage = orderList
               .Skip((page - 1) * pageSize)
               .Take(pageSize);

            var response = new { currentPage = page, totalPages = pages, orders = ordersPerPage };

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }


        // GET api/values/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<Order> Get(string id)
        {
            return await _orderRepository.GetOrder(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task Put([FromBody]Order order)
        {
            Order o = await _orderRepository.GetOrder(order.Id);
            o.ShippingAddress = order.ShippingAddress;
            o.Info = order.Info;
            o.TotalPrice = order.TotalPrice;
            await _orderRepository.UpdateOrder(o.Id, o);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Order order)
        {
            order.PaymentCard = order.PaymentCard ?? new Card();
            order.ShippingAddress = order.ShippingAddress ?? new ShippingAddress();
            _orderRepository.AddOrder(order);
        }

        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFile()
        {
            var file = Request.Form.Files.First();
            string fileId = (await _orderRepository.UploadFile(file)).ToString();
            return Json(fileId);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("downloadfile/{id}")]
        public async Task<IActionResult> DownloadFile(string id)
        {
            var stream = await _orderRepository.DownloadFile(id);
            stream.Position = 0;
            return File(stream, "application/octet-stream", "file.stl");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("rejectorder/{id}")]
        public async Task RejectOrder(string id)
        {
            Order o = await _orderRepository.GetOrder(id);
            o.IsValid = false;
            await _orderRepository.UpdateOrder(id, o);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approveorder/{id}")]
        public async Task ApproveOrder(string id)
        {
            Order o = await _orderRepository.GetOrder(id);
            o.IsValid = true;
            await _orderRepository.UpdateOrder(id, o);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("closeorder/{id}")]
        public async Task CloseOrder(string id)
        {
            Order o = await _orderRepository.GetOrder(id);
            o.IsClosed = true;
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
